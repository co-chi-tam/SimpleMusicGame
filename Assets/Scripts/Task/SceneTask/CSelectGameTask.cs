using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Pul;

namespace SimpleGameMusic {
	public class CSelectGameTask : CSimpleTask {

		#region Properties

		private CUISelectGame m_UISelectGame;
		private CPlayerEnergy m_PlayerEnergy;

		#endregion

		#region Constructor

		public CSelectGameTask () : base ()
		{
			this.taskName = "SelectGame";
			this.nextTask = "PlayGame";
		}

		#endregion

		#region Implementation Task

		public override void StartTask ()
		{
			base.StartTask ();
			this.m_UISelectGame = CUISelectGame.GetInstance ();
			this.m_PlayerEnergy = CTaskUtil.REFERENCES [CTaskUtil.PLAYER_ENERGY] as CPlayerEnergy; 

			var listSongs = CTaskUtil.REFERENCES [CTaskUtil.LIST_SONG] as List<CSongData>;
			if (listSongs != null) {
				this.m_UISelectGame.LoadCategories (listSongs);
			} else {
				CLog.LogError ("Error: Can not load song data.");
			}
			this.m_UISelectGame.SetEnergyDisplayText (this.m_PlayerEnergy.ToString());
			this.m_PlayerEnergy.OnUpdateEnergy = null;
			this.m_PlayerEnergy.OnUpdateEnergy += () => {
				this.m_UISelectGame.SetEnergyDisplayText (this.m_PlayerEnergy.ToString());
				PlayerPrefs.SetInt (CTaskUtil.PLAYER_ENERGY, this.m_PlayerEnergy.currentEnergy);
				PlayerPrefs.Save ();
			};
			this.m_PlayerEnergy.OnUpdate = null;
			this.m_PlayerEnergy.OnUpdate += (timeUpdate) => {
				float minute = (int)(timeUpdate / 60f);
				float second = (int)(timeUpdate % 60f);
				this.m_UISelectGame.SetEnergyReloadText (string.Format ("{0}:{1}", minute.ToString("00"), second.ToString("00")));
			};
		}

		public override void OnTaskCompleted ()
		{
			if (this.m_PlayerEnergy.currentEnergy > 0) {
				base.OnTaskCompleted ();
			} else {
				if (Advertisement.IsReady("rewardedVideo"))
				{
					var options = new ShowOptions { resultCallback = HandleShowResult };
					Advertisement.Show("rewardedVideo", options);
				}
			}
		}

		public override void EndTask ()
		{
			base.EndTask ();
			var listSong = CTaskUtil.REFERENCES [CTaskUtil.LIST_SONG] as List<CSongData>;
			var name = CTaskUtil.REFERENCES [CTaskUtil.SELECTED_SONG].ToString();
			if (listSong != null) {
				for (int i = 0; i < listSong.Count; i++) {
					if (listSong [i].songName.Equals (name)) {
						CTaskUtil.REFERENCES [CTaskUtil.DATA_SONG] = listSong [i];
						break;
					}
				}
			} else {
				CLog.LogError ("Error: Can not load song data.");
			}
			this.m_PlayerEnergy.OnUpdateEnergy = null;
			this.m_PlayerEnergy.OnUpdate = null;
			var currentEnegy = this.m_PlayerEnergy.currentEnergy;
			this.m_PlayerEnergy.currentEnergy = currentEnegy - 1 <= 0 ? 0 : currentEnegy - 1;
			PlayerPrefs.SetInt (CTaskUtil.PLAYER_ENERGY, this.m_PlayerEnergy.currentEnergy);
			PlayerPrefs.Save ();
		}

		#endregion

		#region Main methods

		private void HandleShowResult(ShowResult result)
		{
			switch (result)
			{
			case ShowResult.Finished:
				// COMPLETE
				base.OnTaskCompleted ();
				break;
			case ShowResult.Skipped:
				// COMPLETE
				base.OnTaskCompleted ();
				break;
			case ShowResult.Failed:
				
				break;
			}
		}

		#endregion

	}
}
