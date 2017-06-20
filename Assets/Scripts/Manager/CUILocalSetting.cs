using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;
using SimpleGameMusic.UICustom;

namespace SimpleGameMusic {
	public class CUILocalSetting : CMonoSingleton<CUILocalSetting> {

		#region Properties

		[SerializeField]	private GameObject m_GroupButton;
		[SerializeField]	private CUILanguageItem m_LAPrefabButton;

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
				var optionButton = Instantiate<CUILanguageItem> (m_LAPrefabButton);
				var laData = languages [i];
				var spriteLa = CAssetBundleManager.LoadResourceOrBundle <Sprite> (laData.laName);
				optionButton.transform.SetParent (this.m_GroupButton.transform);
				optionButton.gameObject.SetActive (true);
				optionButton.SetUpItem (laData.laDisplay, spriteLa, () => {
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
