using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;
using UICustom;

namespace SimpleGameMusic {
	public class CUILocalSetting : CMonoSingleton<CUILocalSetting> {

		#region Properties

		[SerializeField]	private GameObject m_GroupButton;
		[SerializeField]	private CButton m_LAPrefabButton;

		private CRootTask m_RootTask;

		#endregion

		#region Implementation MonoBehavious

		protected virtual void Start() {
			this.m_RootTask = CRootTask.GetInstance ();
		}

		#endregion

		#region Main methods

		public void LoadListLanguage(List<CLanguageData> languages) {
			for (int i = 0; i < languages.Count; i++) {
				var optionButton = Instantiate<CButton> (m_LAPrefabButton);
				var laData = languages [i];
				optionButton.transform.SetParent (this.m_GroupButton.transform);
				optionButton.gameObject.SetActive (true);
				optionButton.SetText (laData.laDisplay);
				optionButton.onClick.RemoveAllListeners ();
				optionButton.onClick.AddListener (() => {
					SelectLanguage(laData.laName);
				});
			}
			this.m_LAPrefabButton.gameObject.SetActive (false);
		}

		public void SelectLanguage(string name) {
			CTaskUtil.REFERENCES [CTaskUtil.LA_SETTING] = name;
			this.m_RootTask.GetCurrentTask ().OnTaskCompleted ();
		}

		#endregion

	}
}
