using System.Collections;
using System.Collections.Generic;
using HegaCore;
using UnityEngine;
using UnityEngine.UI;
using UnuGames;

public class DelayCommandData
{
    public float delayTime;
    public string[] param;
}

public class WorkoutCommandProcessor : MonoBehaviour
{
	private readonly string H_POSE_ANIMATOR_PHASE_ID = "ID";
	public Image BG;
	public Image FG;
	public RawImage charRenderImage;

	public MoveTweener MoveTweener => BackgroundManager.Instance.MoveTweener;
	public ZoomTweener ZoomTweener=> BackgroundManager.Instance.ZoomTweener;
	public ShakeTweener ShakeTweener=> BackgroundManager.Instance.ShakeTweener;
	
	public RotateTweener RotateTweener=> BackgroundManager.Instance.RotateTweener;
	
	public EyeBlinkTweener EyeBlinkTweener;

	public float autoNextSeconds;
	public float waitForSeconds;
	public float forceWait;
	
	private Dictionary<string, HPoseController> hPoseControllers;
	private HPoseController currentPose;
	
	private List<DelayCommandData> DelayCommandDatas;
	private float currentDelayCharged;
	
#if UNITY_EDITOR
	void OnGUI()
	{
		GUI.Label(new Rect(220, 20, 100, 50), currentDelayCharged.ToString());
	}
#endif
	
	public void Init()
	{
		DelayCommandDatas = new List<DelayCommandData>();
		hPoseControllers = new Dictionary<string, HPoseController>();
		waitForSeconds = 0;
		forceWait = 0;
		autoNextSeconds = 0;
		//charRenderImage.texture = Live2DManager.Instance.RenderTexture;
	}

	public void UpdateProcessor()
	{
		float deltaTime = Time.deltaTime;
		
		if (waitForSeconds > 0)
			waitForSeconds -= deltaTime;
		
		if (autoNextSeconds > 0)
			autoNextSeconds -= deltaTime;
		else if (forceWait > 0)
			forceWait -= deltaTime;
		
		currentDelayCharged += Time.deltaTime;
		if (DelayCommandDatas.Count > 0)
		{
			for (int i = DelayCommandDatas.Count - 1; i >= 0; i--)
			{
				if (DelayCommandDatas[i].delayTime < currentDelayCharged)
				{
					UnuLogger.Log(currentDelayCharged + " Do by delay: " + DelayCommandDatas[i].delayTime.ToString().AddColor("green"));
					DoCommand(DelayCommandDatas[i].param);
					DelayCommandDatas.RemoveAt(i);
				}
			}
		}
	}

	public void ClearDelayCommands()
	{
		DelayCommandDatas.Clear();
		currentDelayCharged = 0;
	}
	
	public void AnalysisCommandGroup(string commands)
	{
		var lines = commands.Split('|');
		foreach (var commandData in lines)
		{
			var strings = commandData.Split(':');
			
			if (string.Equals(strings[0], "Delay")) // Delay Command
			{
				AddDelayCommand(strings);
			}
			else
			{
				DoCommand(strings);
			}
		}
	}

	public void AddDelayCommand(string[] delayData)
	{
		DelayCommandData newCommand = new DelayCommandData();
		newCommand.delayTime = delayData[1].ParseFloatUS();
		newCommand.param = new string[delayData.Length - 2];
		for (int i = 2; i < delayData.Length; i++)
		{
			newCommand.param[i - 2] = delayData[i];
		}
		UnuLogger.Log($"Add:  {newCommand.delayTime} -- {newCommand.param}");
		DelayCommandDatas.Add(newCommand);
	}
	
	private void DoCommand(string[] strings)
	{
		switch (strings[0])
		{
			case "PoseLoad":
				LoadHPose(strings[1]);
				break;
			
			case "PoseShow":
				PoseShow(strings[1]);
				break;
			
			case "PoseHide":
				PoseHide(strings[1]);
				break;
			
			case "PosePhase":
				PosePhase(strings[1], strings[2]);
				break;
			
			case "PoseTrigger":
				PoseTrigger(strings[1], strings[2]);
				break;
			
			case "PoseBool":
				PoseBool(strings[1], strings[2], string.Equals(strings[3], "true"));
				break;
			
			case "PoseInt":
				PoseInt(strings[1], strings[2], int.Parse(strings[3]));
				break;
			
			case "PoseFloat":
				PoseFloat(strings[1], strings[2], strings[3].ParseFloatUS());
				break;
			
			case "ResetState":
				ResetState(strings[1]);
				break;
			
			case "PlaySFX":
				if (strings.Length == 3)
					PlaySFX(strings[1], !string.IsNullOrEmpty(strings[2]));
				else
					PlaySFX(strings[1]);
				break;
			
			case "VoiceBG":
					PlaySFX(strings[1], true);
				break;
			
			case "Position":
				int plen = strings.Length;
				if (plen == 3)
					RenderPosition(new Vector2(strings[1].ParseFloatUS(), strings[2].ParseFloatUS()), 0);
				else if (plen == 4)
					RenderPosition(new Vector2(strings[1].ParseFloatUS(), strings[2].ParseFloatUS()), strings[3].ParseFloatUS());
				break;
			
			case "Zoom":
				int zlen = strings.Length;
				if(zlen == 2)
					RenderZoom(strings[1].ParseFloatUS());
				else if(zlen == 3)
					RenderZoom(strings[1].ParseFloatUS(), strings[2].ParseFloatUS());
				else if(zlen == 4)
					RenderZoom(strings[1].ParseFloatUS(), strings[2].ParseFloatUS(),strings[3].ParseFloatUS() );
				break;
			
			case "Shake":
				Vector2 from = new Vector2(strings[1].ParseFloatUS(), strings[2].ParseFloatUS());
				Vector2 to = new Vector2(strings[3].ParseFloatUS(), strings[4].ParseFloatUS());
				float time = strings[5].ParseFloatUS();
				if (strings.Length > 6)
				{
					float zoomTo = strings[6].ParseFloatUS();
					Shake(from, to, time, zoomTo);
				}
				else
					Shake(from, to, time);
				break;
			
			case "Rotate":
				Vector3 rotate = new Vector3(strings[1].ParseFloatUS(), strings[2].ParseFloatUS(),strings[3].ParseFloatUS());
				float rotateDuration = -1f;
				if (strings.Length > 4)
					rotateDuration = strings[4].ParseFloatUS();
				Rotate(rotate, rotateDuration);
				break;
			
			case "StopShake":
				StopShake(true);
				break;
			
			case "EyeClose":
				EyeBlinkTweener?.EyeClose();
				break;
			case "EyeOpen":
				EyeBlinkTweener?.EyeOpen();
				break;
			case "EyeBlink":
				EyeBlinkTweener?.DoEyeBlink();
				break;
			case "ForceWait":
				forceWait = strings[1].ParseFloatUS();
				break;
			case "MustWait":
				waitForSeconds = strings[1].ParseFloatUS();
				break;
			case "AutoNext":
				autoNextSeconds = strings[1].ParseFloatUS();
				break;
			case "EnableLipsyns":
				EnableLipsyns(strings[1], string.Equals(strings[2], "true"));
				break;
			case "EnableMouthType":
				EnableMouthType(strings[1], string.Equals(strings[2], "true"));
				break;
			case "SelectMouth":
				SelectMouth(strings[1], int.Parse(strings[2]));
				break;
		}
	}
	
	public void RenderPositionWithRawData(string[] positionData)
	{
		if (positionData.Length == 5)
		{
			RenderPosition(new Vector2(positionData[0].ParseFloatUS(), positionData[1].ParseFloatUS()));
			RenderPosition(new Vector2(positionData[2].ParseFloatUS(), 
					positionData[3].ParseFloatUS()),
				positionData[4].ParseFloatUS());
		}
		else // == 3
		{
			RenderPosition(new Vector2(positionData[0].ParseFloatUS(), positionData[1].ParseFloatUS()),
				positionData[2].ParseFloatUS());
		}
	}
	public void RenderPosition(Vector2 target, float duration = 0)
	{
		MoveTweener.Move(target, duration);
	}
	
	public void RenderZoomWithRawData(string[] zoomData)
	{
		if (zoomData.Length == 1)
		{
			RenderZoom(zoomData[0].ParseFloatUS());
		}
		if (zoomData.Length == 2)
		{
			RenderZoom(zoomData[0].ParseFloatUS(), zoomData[1].ParseFloatUS());
		}
		else if (zoomData.Length == 3)
		{
			RenderZoom(zoomData[0].ParseFloatUS());
			RenderZoom(zoomData[1].ParseFloatUS(), zoomData[2].ParseFloatUS());
		}
	}
	
	public void RenderZoom(float zoom, float duration = 0)
	{
		ZoomTweener.Zoom(zoom, duration);
	}

	public void RenderZoom(float from, float to, float duration)
	{
		ZoomTweener.Zoom(from,to,duration);
	}

	public void Shake(Vector2 from, Vector2 to, float frequency, float zoomTo = 1f)
	{
		ShakeTweener.Shake(from, to, frequency, zoomTo);
	}

	public void Rotate(Vector3 rotateTo, float duration = 0)
	{
		RotateTweener.Rotate(rotateTo, duration);
	}

	public void StopShake(bool reset = false)
	{
		ShakeTweener.StopShake(reset);
	}

	private void ResetState(string target)
	{
		if (hPoseControllers.ContainsKey(target))
		{
			var cur = hPoseControllers[target];
			var state = cur.CubismController.Animator.GetCurrentAnimatorStateInfo(0);
			cur.CubismController.Animator.Play(state.shortNameHash, 0, 0f);
		}
	}

	public void PlaySFX(string sfx, bool loop = true)
	{
		UnuLogger.Log($"Play SFX : {sfx}");
		// SFX
		if (!string.IsNullOrEmpty(sfx))
		{
			if (string.Equals("stop", sfx))
			{
				AudioManager.Instance.Player.StopVoiceBG();
				AudioManager.Instance.Player.StopVoice();
			}
			else
				AudioManager.Instance.Player.PlayVoiceBG(sfx,!loop);
		}
	}
	
	public void LoadHPose(string poseID)
	{
		UnuLogger.Log("<color=red>Load H Pose:</color> " + poseID);
		if (hPoseControllers.ContainsKey(poseID))
		{
			return;
		}
		
		foreach (var item in Live2DManager.Instance.hPosePrefabs)
		{
			if (string.Equals(item.name, poseID))
			{
				var obj = Instantiate(item);
				var loadedPose = obj.GetComponent<HPoseController>();
				loadedPose.transform.localScale = Vector3.one;
				loadedPose.transform.localPosition = Vector3.one * 50;
				if (loadedPose.hasBG)
					loadedPose.LoadBG(poseID + "_BG");
				if (loadedPose.hasFG)
					loadedPose.LoadFG(poseID + "_FG");
				loadedPose.CubismController.Animator.enabled = true;
				loadedPose.gameObject.name = poseID;
				loadedPose.CubismController.InitLipsyns();
				hPoseControllers.Add(poseID, loadedPose);
				return;
			}
		}
	}
	
	public void PoseShow(string target, bool hideCurrent = false)
	{
		if (currentPose != null)
		{
			if (string.Equals(target, currentPose.gameObject.name))
			{
				return;
			}
			
			if(hideCurrent)
				currentPose.transform.position = new Vector3(10,50,10);
		}

		if (hPoseControllers.ContainsKey(target))
		{
			var pose = hPoseControllers[target];
			pose.transform.position = Vector3.zero;
			
			//BG.enabled = pose.hasBG;
			if (pose.hasBG)
			{
				//BG.sprite = pose.BG;
				BackgroundManager.Instance.SetBackground(target + "_BG", Color.white, 0f);
			}
			
			//FG.enabled = pose.hasFG;
			if (pose.hasFG)
			{
				//FG.sprite = pose.FG;
				BackgroundManager.Instance.SetForeground(target + "_FG", Color.white, 0f);
			}
			
			currentPose = pose;
		}
	}
	
	public IEnumerator HidePoses()
	{
		currentPose = null;
		foreach (var item in hPoseControllers)
		{
			Destroy(item.Value.gameObject);
			yield return new WaitForSeconds(0.2f);
		}
		hPoseControllers.Clear();
		waitForSeconds = 0;
		forceWait = 0;
	}
	
	public void PoseHide(string target)
	{
		if (hPoseControllers.ContainsKey(target))
		{
			hPoseControllers[target].transform.position = new Vector3(50, 50, 0);
		}
	}

	public void PosePhase(string[] phaseData)
	{
		PosePhase(phaseData[0], phaseData[1]);
	}
	
	public void PosePhase(string target, string phaseID)
	{
		if (hPoseControllers.ContainsKey(target))
			hPoseControllers[target].CubismController.Animator
				.SetInteger(H_POSE_ANIMATOR_PHASE_ID, int.Parse(phaseID));
	}
	
	public void PoseTrigger(string target, string triggerName)
	{
		UnuLogger.Log($"[{target}] -- [{triggerName}]");
		if (hPoseControllers.ContainsKey(target))
			hPoseControllers[target].CubismController.Animator.SetTrigger(triggerName);
	}

	public void PoseBool(string target, string boolName, bool value)
	{
		if (hPoseControllers.ContainsKey(target))
			hPoseControllers[target].CubismController.Animator.SetBool(boolName, value);
	}

	public void PoseInt(string target, string boolName, int value)
	{
		if (hPoseControllers.ContainsKey(target))
			hPoseControllers[target].CubismController.Animator.SetInteger(boolName, value);
	}
	
	public void PoseFloat(string target, string boolName, float value)
	{
		if (hPoseControllers.ContainsKey(target))
			hPoseControllers[target].CubismController.Animator.SetFloat(boolName, value);
	}

	private void EnableLipsyns(string targetName, bool enable)
	{
		if (hPoseControllers.ContainsKey(targetName))
		{
			hPoseControllers[targetName].CubismController.EnableLipsyns(enable);
		}
	}

	private void EnableMouthType(string targetName, bool enable)
	{
		if (hPoseControllers.ContainsKey(targetName))
		{
			hPoseControllers[targetName].CubismController.EnableControlMouthType(enable);
		}
	}
	
	private void SelectMouth(string targetName, int overrideValue)
	{
		if (hPoseControllers.ContainsKey(targetName))
		{
			hPoseControllers[targetName].CubismController.SelectMouth(overrideValue);
		}
	}
	
}
