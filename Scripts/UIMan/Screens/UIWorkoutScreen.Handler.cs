using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HegaCore;
using HegaCore.UI;
using UnityEngine;
using UnityEngine.UI;
using UnuGames;
using Sirenix.OdinInspector;
using AudioType = HegaCore.AudioType;

[Serializable]
public class WorkOutSceneData
{
	[Tooltip("WorkoutData Address")] public string dataPath;
	[Tooltip("H Pose ID")] public string firstPoseID;
	[Tooltip("BG Music")] public string BGMusic;
}

public partial class UIWorkoutScreen : UIManScreen
{
	#region Fields

	public static void Show(int girlIndex, Action onShowCompleted = null, Action onHide = null,
		Action onHideCompleted = null)
	{
		BackgroundManager.Instance.Deinitialize();
		AudioManager.Instance.Player.StopMusic();
		UIDefaultActivity.Show(1, false,
			() =>
			{
				UIMan.Instance.ShowScreen<UIWorkoutScreen>(girlIndex, onShowCompleted, onHide, onHideCompleted);
			});
	}



	[SerializeField]
	private int girlIndex;
	private WorkOutSceneData currentSceneData;
	private Action onShowCompleted;
	private Action onHide;
	private Action onHideCompleted;
	private bool busy = false;
	
	public WorkoutCommandProcessor CommandProcessor;
	public Text voiceText;

	private WorkoutData currentWorkoutData;
	private WorkoutTextData currentWorkoutTextData;
	private bool isQuiting = false;
	private bool waitForClick = false;
	private bool needClick = true;
	public List<WorkOutSceneData> scenesDatas;

	private GameSettings _gameSettings;
	
	#endregion

	#region Built-in Events

	public override void OnShow(params object[] args)
	{
		base.OnShow(args);
		CommandProcessor.Init();
		busy = true;
		//Live2DCharControl.Instance.DisableAll();
		
		var index = 0;
		args.GetThenMoveNext(ref index, out this.girlIndex)
			.GetThenMoveNext(ref index, out this.onShowCompleted)
			.GetThenMoveNext(ref index, out this.onHide)
			.GetThenMoveNext(ref index, out this.onHideCompleted);
		
		currentSceneData = scenesDatas[girlIndex];
		
		isQuiting = false;

		#if TEST_HSCENE
		needClick = true;
		#else
		needClick = false;
		#endif
		
		// G4DataManager.Instance.GetWorkoutData(currentSceneData.dataPath, (work, worktext) =>
		// {
		// 	currentWorkoutData = work;
		// 	currentWorkoutTextData = worktext;
		// 	PrepareHscene().Forget();
		// });
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
		CommandProcessor.StopShake();
		StartCoroutine(ReturnToPreviousScreenOrDialog());
	}

	private void Update()
	{
#if UNITY_EDITOR
// Work F H code, to hide h scene instantly
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			CanvasGroup.alpha = 0;
		}
#endif
		CommandProcessor.UpdateProcessor();

		if (Input.GetKeyDown(KeyCode.Escape) && !busy)
		{
			//AudioManager.Instance.Player.PlaySound("sfx_click");
			UIButton_Option();
		}
		
#if !TEST_HSCENE
		if (!busy && !isQuiting)
		{
			if ((Input.GetMouseButtonUp(0) || 
			     Input.GetKeyUp(KeyCode.Return) || 
			     Input.GetKeyUp(KeyCode.Space)))
			{
				ClickNext();
			}
		}
#endif
	}

	#endregion

	#region Custom implementation

	void ClickNext()
	{
		waitForClick = false;
		CommandProcessor.autoNextSeconds = 0;
	}
	
#if TEST_HSCENE
	void OnGUI()
	{
		if (!busy)
		{
			if (GUI.Button(new Rect(20, 20, 100, 50), ">>"))
			{
				ClickNext();
			}
		}

		if (busy && currentRowIndex > 10)
		{
			if (GUI.Button(new Rect(20, 20, 100, 50), "=="))
			{
				// End Hscene
				UIButton_Back();
			}
		}

		if (needClick)
		{
			if (GUI.Button(new Rect(300, 20, 100, 50), "Mode: Auto"))
				needClick = false;
		}
		else
		{
			if (GUI.Button(new Rect(300, 20, 100, 50), "Mode: Click"))
				needClick = true;
		}

		if (GUI.Button(new Rect(300, 70, 100, 50), "re row : " + currentRowIndex))
		{
			currentRowIndex--;
			ClickNext();
		}
			
	}
#endif

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
	
	/// <summary>
	/// Call from UI: Back button
	/// </summary>
	public void UIButton_Back()
	{
		currentRowIndex = 0;
		busy = true;
		UnuLogger.Log("End Hscene");
		if (isQuiting)
		{
			return;
		}
		UIDefaultActivity.Show();
		CommandProcessor.forceWait = 100;
		isQuiting = true;
		StopAllCoroutines();
		StartCoroutine(HideScene());
	}
	
	IEnumerator HideScene()
	{
		AudioManager.Instance.Player.StopAllVoices();
		AudioManager.Instance.Player.StopMusic();
		onHide?.Invoke(); // call ham truoc de show cover
		yield return new WaitForSeconds(0.8f);// cho cover
		HideMe();

		yield return StartCoroutine(CommandProcessor.HidePoses());

		CommandProcessor.waitForSeconds = 0;
		CommandProcessor.forceWait = 0;
		waitForClick = false;
	}

	IEnumerator ReturnToPreviousScreenOrDialog()
	{
		yield return new WaitForSeconds(1f);
		onHideCompleted?.Invoke();
		UIDefaultActivity.Hide();
	}

	async UniTaskVoid PrepareHscene()
	{
		CommandProcessor.RenderPosition(Vector2.zero);
		CommandProcessor.RenderZoom(1f);
		CommandProcessor.LoadHPose(currentSceneData.firstPoseID);
		voiceText.text = string.Empty;
		
		var rows = currentWorkoutData.NumRows();

		List<string> firstAudio = new List<string>();
		for (int i = 0; i < 5; i++)
		{
			var row_i = currentWorkoutData.GetAt(i);
			firstAudio.Add(row_i.voice);
			if (!string.IsNullOrEmpty(row_i.sfx) && !string.Equals(row_i.sfx, "stop"))
			{
				firstAudio.Add(row_i.sfx);
			}
		}
		await AudioManager.Instance.PrepareVoiceAsync(true, firstAudio.ToArray());
		
		List<string> followAudio = new List<string>();
		for (int i = 5; i < rows; i++)
		{
			var row_i = currentWorkoutData.GetAt(i);
			followAudio.Add(row_i.voice);
			if (!string.IsNullOrEmpty(row_i.sfx) && !string.Equals(row_i.sfx, "stop"))
			{
				followAudio.Add(row_i.sfx);
			}
		}
		
		//yield return new WaitForSeconds(0.5f);
		AudioManager.Instance.Player.PlayAsync(currentSceneData.BGMusic, AudioType.Music);
		onShowCompleted?.Invoke();
		currentRowIndex = 0;
		
		RunHscene();
		await AudioManager.Instance.PrepareVoiceAsync(true, followAudio.ToArray());
	}


	void RunHscene(params object[] args)
	{
		UIDefaultActivity.Hide();
		StartCoroutine(RunHsceneCO());
	}
	

	private int currentRowIndex = 0;
	private WorkoutData.Row currentRow;
	#if UNITY_EDITOR
	[ReadOnly, ShowInInspector] private AudioClip curClip;
	#endif
	IEnumerator RunHsceneCO()
	{
		CommandProcessor.PoseShow(currentSceneData.firstPoseID);
		busy = false;
		currentRow = currentWorkoutData.GetAt(0);
		while (currentRow != null && !isQuiting)
		{
			yield return new WaitUntil(() => (CommandProcessor.waitForSeconds <= 0));
			yield return new WaitUntil(() => !busy);
			UnuLogger.Log("ROW: ==================================== " + currentRowIndex);
			CommandProcessor.waitForSeconds = 0;
			CommandProcessor.forceWait = 0;
			CommandProcessor.ClearDelayCommands();
			
			if (isQuiting)
				break;
			
			// SFX
			CommandProcessor.PlaySFX(currentRow.sfx);

			//Commands
			if (!string.IsNullOrEmpty(currentRow.commands))
				CommandProcessor.AnalysisCommandGroup(currentRow.commands);

			// PHASE
			if (!string.IsNullOrEmpty(currentRow.phase))
			{
				var s = currentRow.phase.Split(':');
				CommandProcessor.PosePhase(s);
			}

			// SHOW
			if (!string.IsNullOrEmpty(currentRow.show))
				CommandProcessor.PoseShow(currentRow.show);

			// HIDE
			if (!string.IsNullOrEmpty(currentRow.hide))
				CommandProcessor.PoseHide(currentRow.hide);

			//Zoom
			if (!string.IsNullOrEmpty(currentRow.zoom))
			{
				var z = currentRow.zoom.Split(':');
				CommandProcessor.RenderZoomWithRawData(z);
			}

			// Position
			if (!string.IsNullOrEmpty(currentRow.position))
			{
				var p = currentRow.position.Split(':');
				CommandProcessor.RenderPositionWithRawData(p);
			}

			// positionDelay
			if (!string.IsNullOrEmpty(currentRow.positionDelay))
			{
				var positionDelayData = currentRow.positionDelay.Split(':');
				CommandProcessor.AddDelayCommand(positionDelayData);
			}
			// zoomDelay
			if (!string.IsNullOrEmpty(currentRow.zoomDelay))
			{
				var zoomDelayData = currentRow.zoomDelay.Split(':');
				CommandProcessor.AddDelayCommand(zoomDelayData);
			}
			
			if (!string.IsNullOrEmpty(currentRow.voice))
			{
				if (AudioManager.Instance.TryGetVoice(currentRow.voice, out var voiceClip))
				{
#if UNITY_EDITOR
					curClip = voiceClip;
#endif
					AudioManager.Instance.Player.PlayVoice(currentRow.voice);
					CommandProcessor.autoNextSeconds = voiceClip.length + 0.25f;
				}
				else
				{
					UnuLogger.Log("Addressable Load Voice Failed: " + currentRow.voice);
					CommandProcessor.autoNextSeconds = 3.5f;
				}
			}
			else
			{
				CommandProcessor.autoNextSeconds = 3.5f;
			}

#if UNITY_EDITOR
			voiceText.text =
				$"[{currentRow.id}] - {currentRow.voice} - {currentWorkoutTextData.GetTextByVoiceID(currentRow.voice, _gameSettings.Language)}"; // - [{currentRow.voice}]  // .Text()
#else
			voiceText.text = currentWorkoutTextData.GetTextByVoiceID(currentRow.voice,_gameSettings.Language);
#endif
			
			yield return new WaitUntil(() => CommandProcessor.waitForSeconds <= 0);
			yield return new WaitUntil(() => !busy);

			waitForClick = true;
			yield return new WaitUntil(() => !waitForClick || (!needClick && CommandProcessor.autoNextSeconds <= 0));
			yield return new WaitUntil(() => CommandProcessor.forceWait <= 0);

			currentRowIndex++;
			currentRow = currentWorkoutData.GetAt(currentRowIndex);
			if (currentRow == null)
			{
				break;
			}
		}

		busy = true;
		yield return  new WaitForSeconds(1f);
		voiceText.text = string.Empty;
#if !TEST_HSCENE
		// End Hscene
		UIButton_Back();
#endif
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
	
	public void OnToggleAutoModeChanged(bool newValue)
	{
		needClick = newValue;
	}
	#endregion

	
	#if UNITY_EDITOR
	[Space(15),BoxGroup("Position"), ReadOnly]
	public string positionCmd;
	[BoxGroup("Position"), ReadOnly]
	public string positionShortCmd;
	[BoxGroup("Position Raw")]
	public string rawPositionData;
	[BoxGroup("Position")]
	public Vector2 editorPositionFrom;
	[BoxGroup("Position")]
	 public Vector2 editorPositionTo;
	[BoxGroup("Position"),MinValue(0)]
	public float posDuration;

	[BoxGroup("Position"),Button("Position", ButtonSizes.Medium)]
	public void EditorPosition()
	{
		CommandProcessor.RenderPosition(editorPositionFrom);
		CommandProcessor.RenderPosition(editorPositionTo,posDuration);
		positionShortCmd = $"Position:{editorPositionTo.x}:{editorPositionTo.y}:{posDuration}";
		positionCmd =
			$"Position:{editorPositionFrom.x}:{editorPositionFrom.y}:{editorPositionTo.x}:{editorPositionTo.y}:{posDuration}";
		rawPositionData =
			$"{editorPositionFrom.x}:{editorPositionFrom.y}:{editorPositionTo.x}:{editorPositionTo.y}:{posDuration}";
	}
	
	[BoxGroup("Position Raw"),Button("Run Position Raw", ButtonSizes.Medium)]
	public void RunEditorPositionWithRaw()
	{
		CommandProcessor.RenderPositionWithRawData(rawPositionData.Split(':'));
	}
	
	[Space(15),BoxGroup("Zoom"), ReadOnly]
	public string zoomCmd;
	[BoxGroup("Zoom"), ReadOnly]
	public string zoomShortCmd;
	[BoxGroup("Zoom Raw")]
	public string rawZoomData;
	[BoxGroup("Zoom"),MinValue(1)]
	public float editorZoomFrom;
	[BoxGroup("Zoom"),MinValue(1)]
	public float editorZoomTo;
	[BoxGroup("Zoom"),MinValue(0)]
	public float zoomDuration;
	
	[BoxGroup("Zoom"), Button("Zoom", ButtonSizes.Medium)]
	public void EditorZoom()
	{
		CommandProcessor.RenderZoom(editorZoomFrom);
		CommandProcessor.RenderZoom(editorZoomTo, zoomDuration);
		zoomCmd = $"Zoom:{editorZoomFrom}:{editorZoomTo}:{zoomDuration}";
		zoomShortCmd = $"Zoom:{editorZoomTo}:{zoomDuration}";
		rawZoomData = $"{editorZoomFrom}:{editorZoomTo}:{zoomDuration}";
	}
	
	[BoxGroup("Zoom Raw"),Button("Run Zoom Raw", ButtonSizes.Medium)]
	public void RunEditorZoomWithRaw()
	{
		CommandProcessor.RenderZoomWithRawData(rawZoomData.Split(':'));
	}
	
	[BoxGroup("Shake")]
	public string shakeCmd;
	[BoxGroup("Shake")] public Vector2Int editorFrom;
	[BoxGroup("Shake")] public Vector2Int editorTo;
	[BoxGroup("Shake"), MinValue(0.1f)] public float editorFrequency;
	[BoxGroup("Shake"), MinValue(1f)] public float editorShakeZoomTo;
	[BoxGroup("Shake"), Button("Shake", ButtonSizes.Medium)]
	public void EditorShake()
	{
		CommandProcessor.Shake(editorFrom,editorTo,editorFrequency,editorShakeZoomTo);
		shakeCmd = $"Shake:{editorFrom.x}:{editorFrom.y}:{editorTo.x}:{editorTo.y}:{editorFrequency}:{editorShakeZoomTo}";
	}

	[BoxGroup("Shake"), Button("Stop Shake", ButtonSizes.Medium)]
	private void EditorForceStopShake()
	{
		CommandProcessor.StopShake(true);
	}
	
#endif

}
