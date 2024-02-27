using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HegaCore;
using HegaCore.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnuGames;
using Random = UnityEngine.Random;

public partial class UIWorkoutLiteScreen : UIManScreen
{
	#region Fields
	[Serializable]
	public class WorkOutLiteSceneData
	{
		[Tooltip("WorkoutLiteData Address")] public string dataPath;
		[Tooltip("H Pose ID")] public string firstPoseID;
	}

	public static void Show(int girlIndex, Action onShowCompleted = null, Action onHide = null,
		Action onHideCompleted = null)
	{
		if (!DataManager.Instance.DarkLord)
		{
			return;
		}
		
		AudioManager.Instance.Player.StopMusic();
		UIDefaultActivity.Show(1, false,
			() =>
			{
				UIMan.Instance.ShowScreen<UIWorkoutLiteScreen>(girlIndex, onShowCompleted, onHide, onHideCompleted);
			});
	}
	
	private Dictionary<string, HPoseController> hPoseControllers;

	[SerializeField]
	private int girlIndex;
	private WorkOutLiteSceneData currentSceneData;
	private Action onShowCompleted;
	private Action onHide;
	private Action onHideCompleted;
	private bool busy = false;

	public WorkoutCommandProcessor CommandProcessor;
	
	public GameObject nextBtn;
	public GameObject prevBtn;
	
	private WorkoutLiteData currentWorkoutLiteData;
	private bool isQuiting = false;
	private bool needClick = true;

	public List<WorkOutLiteSceneData> scenesDatas;
	
	[BoxGroup("Debug Row"), ShowInInspector, ReadOnly]
	private int currentRowIndex = 0;
	[BoxGroup("Debug Row"),ShowInInspector, ReadOnly]
	private WorkoutLiteData.Row currentRow;
	[BoxGroup("Debug Row"),ShowInInspector, ReadOnly]
	private int rowCount;
	
	#endregion

	#region Built-in Events

	public override void OnShow(params object[] args)
	{
		base.OnShow(args);
		CommandProcessor.Init();
		
		busy = true;
		//Live2DCharControl.Instance.DisableAll();
		hPoseControllers = new Dictionary<string, HPoseController>();
		nextBtn.SetActive(false);
		prevBtn.SetActive(false);
		var index = 0;
		args.GetThenMoveNext(ref index, out this.girlIndex)
			.GetThenMoveNext(ref index, out this.onShowCompleted)
			.GetThenMoveNext(ref index, out this.onHide)
			.GetThenMoveNext(ref index, out this.onHideCompleted);
		
		currentSceneData = scenesDatas[girlIndex];
		
		isQuiting = false;

		DataManager.Instance.GetWorkoutLiteData(currentSceneData.dataPath, (result) =>
		{
			UnuLogger.Log("Loaded Workoutlite: "  + currentSceneData.dataPath);
			currentWorkoutLiteData = result;
			rowCount = currentWorkoutLiteData.NumRows();
			PrepareHscene().Forget();
		});
	}

	public override void OnShowComplete()
	{
		base.OnShowComplete();
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	public override void OnHideComplete()
	{
		base.OnHideComplete();
		StartCoroutine(ReturnToPreviousScreenOrDialog());
	}

	private void Update()
	{
		if (busy)
		{
			return;
		}
		
		CommandProcessor.UpdateProcessor();
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			UIButton_Option();
		}

		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			UIButton_PrevRow();
		}
		
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			UIButton_NextRow();
		}
	}
	
	#endregion

	#region Custom implementation
	
	public void UIButton_Close()
	{
		busy = true;
		UIMan.Instance.ShowPopup(L10n.Localize("alert"),
			L10n.Localize("exit-hscene"),
			L10n.Localize("yes"),
			L10n.Localize("no"),
			(agrs) => { UIButton_Back(); },
			(objects => busy = false));
	}
	
	public void UIButton_Back()
	{
		currentRowIndex = 0;
		busy = true;
		UnuLogger.Log("End Hscene");
		if (isQuiting)
		{
			return;
		}
		UIDefaultActivity.Show(0.5f);
		CommandProcessor.forceWait = 100;
		isQuiting = true;
		StopAllCoroutines();
		StartCoroutine(HideScene());
	}
	
	IEnumerator HideScene()
	{
		AudioManager.Instance.Player.StopMusic();
		AudioManager.Instance.Player.StopAllVoices();
		onHide?.Invoke();
		yield return new WaitForSeconds(0.8f);// cho cover
		HideMe();
		
		yield return StartCoroutine(CommandProcessor.HidePoses());
	}

	IEnumerator ReturnToPreviousScreenOrDialog()
	{
		yield return new WaitForSeconds(1f);
		onHideCompleted?.Invoke();
	}

	async UniTaskVoid PrepareHscene()
	{
		CommandProcessor.LoadHPose(currentSceneData.firstPoseID);

		List<string> firstAudioList = new List<string>();
		for (int i = 0; i < 5; i++)
		{
			var row_i = currentWorkoutLiteData.GetAt(i);
			foreach (var id in row_i.voices)
			{
				firstAudioList.Add(id);
			}

			if (!string.IsNullOrEmpty(row_i.sfx) && !string.Equals(row_i.sfx, "stop"))
			{
				firstAudioList.Add(row_i.sfx);
			}
		}

		var firstAudio = firstAudioList.ToArray();
		await AudioManager.Instance.PrepareVoiceAsync(true, firstAudio);

		List<string> followAudio = new List<string>();
		for (int i = 5; i < rowCount; i++)
		{
			var row_i = currentWorkoutLiteData.GetAt(i);
			foreach (var voice in row_i.voices)
			{
				followAudio.Add(voice);
			}

			if (!string.IsNullOrEmpty(row_i.sfx) && !string.Equals(row_i.sfx, "stop"))
			{
				followAudio.Add(row_i.sfx);
			}
		}

		//yield return new WaitForSeconds(0.5f);
		onShowCompleted?.Invoke();
		currentRowIndex = 0;
		CommandProcessor.PoseShow(currentSceneData.firstPoseID);
		voices = new List<string>();
		usedVoices = new List<string>();

		RunHscene();
		await AudioManager.Instance.PrepareVoiceAsync(true, followAudio.ToArray());
	}

	void RunHscene(params object[] args)
	{
		RunCurrentRow();
		//AudioControl.Instance.CheckAndPlayBGM("H_BGM");
		// Random Voice
		StartCoroutine(RandomVoiceCO());
		busy = false;
		UIDefaultActivity.Hide();
	}
	
	void RunCurrentRow()
	{
		nextBtn.SetActive(currentRowIndex < rowCount - 1);
		prevBtn.SetActive(currentRowIndex > 0);
		currentRow = currentWorkoutLiteData.GetAt(currentRowIndex);
		CommandProcessor.ClearDelayCommands();
		UnuLogger.Log($"Run RowID: {currentRow.id}");
		
		//SFX
		if (!string.IsNullOrEmpty(currentRow.sfx))
		{
			if (string.Equals("stop",currentRow.sfx))
				AudioManager.Instance.Player.StopVoiceBG();
			else
				AudioManager.Instance.Player.PlayVoiceBG(currentRow.sfx, !currentRow.loop_sfx);
			
			//AudioControl.Instance.PlaySoundAsyns(currentRow.sfx, AudioControl.AudioType.VoiceBG,0, currentRow.loop_sfx);
		}
		
		// PHASE
		if (!string.IsNullOrEmpty(currentRow.phase))
		{
			var s = currentRow.phase.Split(':');
			CommandProcessor.PoseShow(s[0],true);
			CommandProcessor.PosePhase(s[0],s[1]);
		}
		
		//Command
		if (!string.IsNullOrEmpty(currentRow.command))
			CommandProcessor.AnalysisCommandGroup(currentRow.command);
		
		// Voices
		voices.Clear();
		voices.AddRange(currentRow.voices);
		usedVoices.Clear();
	}

	public void UIButton_NextRow()
	{
		int newRowIndex = currentRowIndex + 1;
		if (newRowIndex < rowCount)
		{
			currentRowIndex = newRowIndex;
			RunCurrentRow();
			OnNext();
		}
	}

	public void UIButton_PrevRow()
	{
		int newRowIndex = currentRowIndex - 1;
		if (newRowIndex >= 0)
		{
			currentRowIndex = newRowIndex;
			RunCurrentRow();
			OnNext();
		}
	}
	
	private bool newRow = false;
	private bool running = false;
	private List<string> voices;
	private List<string> usedVoices;
	IEnumerator RandomVoiceCO()
	{
		while (!isQuiting)
		{
			newRow = false;
			string vID = GetRandomVoiceAddress();
			if (string.IsNullOrEmpty(vID))
			{
				CommandProcessor.waitForSeconds = 1f;
			}
			else
			{
				if (AudioManager.Instance.TryGetVoice(vID, out var voiceClip))
				{
					AudioManager.Instance.Player.PlayVoice(vID);
					CommandProcessor.waitForSeconds = voiceClip.length + currentRow.delay;
				}
				else
				{
					UnuLogger.Log("Addressable Load Voice Failed: " + vID);
					CommandProcessor.waitForSeconds = 3.5f;
				}
			}
			yield return new WaitUntil(()=> CommandProcessor.waitForSeconds <= 0 || newRow || isQuiting);
		}
	}

	private string GetRandomVoiceAddress()
	{
		int count = voices.Count;
		if (count == 0)
		{
			if (usedVoices.Count == 0)
			{
				return string.Empty;
			}
			else
			{
				voices.AddRange(usedVoices);
				count = voices.Count;
				usedVoices.Clear();
			}
		}

		int r = 0;
		if (count > 1)
		{
			r = Random.Range(0, count);
		}
		string result = voices[r];
		usedVoices.Add(result);
		voices.RemoveAt(r);
		return result;
	}
	
	public void UIButton_Option()
	{
		if (busy)
		{
			return;
		}
		busy = true;
		
		UISettingsDialog.Show(()=> busy = false);
		//UIMan.Instance.ShowDialog<UISettingDialog>(new UICallback((objects => busy = false)), false, true);
	}
	
	void OnNext()
	{
		CommandProcessor.waitForSeconds = 0;
		newRow = true;
	}
	#endregion

}
