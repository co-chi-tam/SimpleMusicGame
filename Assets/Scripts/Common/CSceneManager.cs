using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleSingleton;

public class CSceneManager: CMonoSingleton<CSceneManager> {

	#region Properties


	[SerializeField]	private Color m_ScreenLoadingColor = Color.white;
	[Range(0.25f, 5f)]
	[SerializeField]	private float m_ScreenLoadingTime = 1f;

	public Action<Scene, Scene> activeSceneChanged;

	private Texture2D m_LoadingScreenTexture;
	private Rect m_FullScreenRect;
	private bool m_IsFadeOut = false;
	private bool m_NeedDraw = false;

	#endregion

	#region Implementation MonoBehavious

	protected override void Awake ()
	{
		base.Awake ();
		DontDestroyOnLoad (this.gameObject);
		m_FullScreenRect = new Rect (0f, 0f, Screen.width, Screen.height);
		SceneManager.activeSceneChanged += delegate(Scene arg0, Scene arg1) {
			if (activeSceneChanged != null) {
				activeSceneChanged (arg0, arg1);
			}	
			OnFadeOutScreen();
		};
	}

	protected virtual void Start() {
		OnFadeOutScreen();
	}

	protected virtual void OnGUI() {
		if (Event.current.type.Equals (EventType.Repaint) && m_NeedDraw) {
			GUI.DrawTexture (m_FullScreenRect, m_LoadingScreenTexture, ScaleMode.StretchToFill);
			var currentColor = m_LoadingScreenTexture.GetPixels () [0];
			var fadeAlpha = 1f / m_ScreenLoadingTime * Time.deltaTime; 
			currentColor.a = m_IsFadeOut ? currentColor.a - fadeAlpha : currentColor.a + fadeAlpha;
			m_LoadingScreenTexture.SetPixels (new Color[] { currentColor });
			m_LoadingScreenTexture.Apply ();
			m_NeedDraw = m_IsFadeOut ? currentColor.a > 0f : true;
		}
	}

	#endregion

	#region Main methods

	private void OnRepairTexture(float alpha) {
		m_LoadingScreenTexture = new Texture2D (1, 1);
		m_ScreenLoadingColor.a = alpha;
		m_LoadingScreenTexture.SetPixels (new Color[] { m_ScreenLoadingColor });
		m_LoadingScreenTexture.Apply ();
	}

	private void OnFadeOutScreen() {
		OnRepairTexture (1f);
		m_IsFadeOut = true;
		m_NeedDraw = true;
	}

	private void OnFadeInScreen() {
		OnRepairTexture (0f);
		m_IsFadeOut = false;
		m_NeedDraw = true;
	}

	public IEnumerator LoadSceneAsync(string sceneName) {
		OnFadeInScreen ();
		yield return WaitHelper.WaitForShortSeconds;
		yield return SceneManager.LoadSceneAsync (sceneName);
		OnFadeOutScreen ();
		yield return WaitHelper.WaitForShortSeconds;
	}

	#endregion

	#region Getter && Setter

	public string GetActiveSceneName () {
		return SceneManager.GetActiveScene ().name;
	}

	#endregion

}
