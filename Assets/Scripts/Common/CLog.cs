using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLog {

	public enum ELogMode : byte {
		None 	= 0,
		Debug 	= 1, 
		Warning = 2,
		Error 	= 3
	}

	public static void LogDebug(string text, ELogMode mode = ELogMode.Debug) {
#if UNITY_EDITOR
		Debug.Log (text);
#endif
		var callerName = GetCallerName () + " ";
		CLogGUI.Instance.AddDebugMessage (callerName + text);
	}

	public static void LogWarning(string text, ELogMode mode = ELogMode.Warning) {
#if UNITY_EDITOR
		Debug.LogWarning (text);
#endif
		var callerName = GetCallerName () + " ";
		CLogGUI.Instance.AddWarningMessage (callerName + text);
	}

	public static void LogError(string text, ELogMode mode = ELogMode.Error) {
#if UNITY_EDITOR
		Debug.LogError (text);
#endif
		var callerName = GetCallerName () + " ";
		CLogGUI.Instance.AddErrorMessage (callerName + text);
	}

	private static string GetCallerName()
	{
		try {
			System.Diagnostics.StackTrace stack = new System.Diagnostics.StackTrace(false);
			if (stack.FrameCount < 3) {
				return "Unknown";
			}
			return stack.GetFrame(2).GetMethod().ReflectedType + ":" + stack.GetFrame(2).GetMethod().Name;
		} catch (Exception) {
			// GetCallerName失敗
			return "Unknown";
		}
	}

}

public class CLogGUI: MonoBehaviour {

	#region Singleton

	protected static CLogGUI m_Instance;
	private static object m_SingletonObject = new object();
	public static CLogGUI Instance {
		get { 
			lock (m_SingletonObject) {
				if (m_Instance == null) {
					GameObject go = new GameObject ("CLogGUI");
					m_Instance = go.AddComponent<CLogGUI> ();
					go.SetActive (true);
				}
				return m_Instance;
			}
		}
	}

	public static CLogGUI GetInstance() {
		return Instance;
	}

	#endregion

	#region Properties

	[SerializeField]	private List<string> m_Messages;

	private Rect m_MessageRect;
	private Vector2 m_MessageScroll;
	private bool m_NeedShowLog;

	#endregion

	#region Implementation Monobehavious

	protected virtual void Awake ()
	{
		m_Instance = this;
		m_Messages = new List<string> ();
		m_MessageRect = new Rect (10f, 10f, Screen.width - 20f, Screen.height - 20f);
	} 

	protected virtual void OnGUI() {
		if (m_NeedShowLog == false)
			return;
		m_MessageRect = GUILayout.Window (0, m_MessageRect, DrawMessageGUI, "Debug");
	}

	#endregion

	#region Main methods

	private void DrawMessageGUI(int id) {
		m_MessageScroll = GUILayout.BeginScrollView (m_MessageScroll, false, true);
		for (int i = 0; i < m_Messages.Count; i++) {
			GUILayout.Label (m_Messages[i]);
		}
		GUILayout.EndScrollView ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Close")) {
			m_NeedShowLog = false;
		}
	}

	public void AddDebugMessage(string message) {
		var formatMessage = "*Debug: " + message;
		m_Messages.Add (formatMessage);
		m_NeedShowLog = true;
	}

	public void AddWarningMessage(string message) {
		var formatMessage = "**Warning: " + message;
		m_Messages.Add (formatMessage);
		m_NeedShowLog = true;
	}

	public void AddErrorMessage(string message) {
		var formatMessage = "***Error: " + message;
		m_Messages.Add (formatMessage);
		m_NeedShowLog = true;
	}

	#endregion

}
