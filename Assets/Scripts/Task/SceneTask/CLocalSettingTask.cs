using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CLocalSettingTask : CSimpleTask {

		public CLocalSettingTask (): base () {
			this.taskName = "LocalSetting";
			this.nextTask = "SelectGame";
		}

		public override void OnSceneLoading ()
		{
			base.OnSceneLoading ();
		}

		public override void StartTask ()
		{
			base.StartTask ();
			var laSetting = PlayerPrefs.GetString (CTaskUtil.LA_SETTING, string.Empty);
			if (string.IsNullOrEmpty (laSetting)) {
				// TODO
			} else {
				CTaskUtil.REFERENCES [CTaskUtil.LA_SETTING] = laSetting;
				this.OnTaskCompleted ();
			}
			var saveListLanguages = CTaskUtil.REFERENCES [CTaskUtil.LA_DISPLAY] as List<CLanguageData>;
			if (saveListLanguages != null) {
				CUILocalSetting.Instance.LoadListLanguage (saveListLanguages);
			} else {
				CLog.LogError ("Error: Can not load song data.");
			}
		}

		public override void EndTask ()
		{
			base.EndTask ();
			var laName = CTaskUtil.REFERENCES [CTaskUtil.LA_SETTING].ToString();
			PlayerPrefs.SetString (CTaskUtil.LA_SETTING, laName);
			PlayerPrefs.Save ();
		}

		protected override bool NeedTransmissionEffect ()
		{
			var laSetting = PlayerPrefs.GetString (CTaskUtil.LA_SETTING, string.Empty);
			return false;
		}
		
	}
}
