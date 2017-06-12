using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CLocalSettingTask : CSimpleTask {

		public CLocalSettingTask (): base () {
			this.taskName = "LocalSetting";
			this.nextTask = "SelectGame";
		}

		public override void StartTask ()
		{
			base.StartTask ();
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
			PlayerPrefs.SetInt (CTaskUtil.GAME_FIRST_LAUNCH, 1);
			PlayerPrefs.Save ();
		}

		public override bool IsHiddenTask ()
		{
			return true;
			return PlayerPrefs.GetInt (CTaskUtil.GAME_FIRST_LAUNCH, 0) == 1;
		}

	}
}
