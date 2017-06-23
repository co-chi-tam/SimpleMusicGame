using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Pul;

namespace SimpleMusicGame {
	public class CSelectSongTask : CSimpleTask {

		#region Properties

		private CUISelectSong m_UISelectGame;
		private CPlayerEnergy m_PlayerEnergy;

		#endregion

		#region Constructor

		public CSelectSongTask () : base ()
		{
			this.taskName = "SelectSong";
			this.nextTask = "PlayGame";
		}

		#endregion

		#region Implementation Task

		public override void StartTask ()
		{
			base.StartTask ();
			this.m_UISelectGame = CUISelectSong.GetInstance ();
			this.m_PlayerEnergy = CTaskUtil.REFERENCES [CTaskUtil.PLAYER_ENERGY] as CPlayerEnergy; 

			var listSongs = CTaskUtil.REFERENCES [CTaskUtil.LIST_SONG] as List<CSongData>;
			if (listSongs != null) {
				this.m_UISelectGame.LoadListSongs (listSongs);
			} else {
				CLog.LogError ("Error: Can not load song data.");
			}

			var energyText = string.Format ("{0}/{1}", this.m_PlayerEnergy.currentEnergy, this.m_PlayerEnergy.maxEnergy);
			this.m_UISelectGame.SetEnergyDisplayText (energyText);
			this.m_PlayerEnergy.OnUpdatePoint = null;
			this.m_PlayerEnergy.OnUpdatePoint += () => {
				var energyText2 = string.Format ("{0}/{1}", this.m_PlayerEnergy.currentEnergy, this.m_PlayerEnergy.maxEnergy);
				this.m_UISelectGame.SetEnergyDisplayText (energyText2);
			};

			this.m_PlayerEnergy.OnUpdateTimer = null;
			this.m_PlayerEnergy.OnUpdateTimer += (timeUpdate) => {
				float minute = (int)(timeUpdate / 60f);
				float second = (int)(timeUpdate % 60f);
				this.m_UISelectGame.SetEnergyReloadText (string.Format ("{0}:{1}", minute.ToString("00"), second.ToString("00")));
			};

			var soundVolume = (float)CTaskUtil.Get (CTaskUtil.GAME_SOUND_VOLUME);
			this.m_UISelectGame.SetSoundVolume (soundVolume);
		}

		public override void OnTaskCompleted ()
		{
			var selectedSong = CTaskUtil.REFERENCES [CTaskUtil.SELECTED_SONG] as CSongData;
			if (this.m_PlayerEnergy.currentEnergy > 0 
				&& this.m_PlayerEnergy.currentEnergy >= selectedSong.hardPoint) {
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
			var selectedSong = CTaskUtil.REFERENCES [CTaskUtil.SELECTED_SONG] as CSongData;
			if (selectedSong != null) {
				var currentEnergy = this.m_PlayerEnergy.currentEnergy - selectedSong.hardPoint;
				this.m_PlayerEnergy.SetEnergy (currentEnergy);
				PlayerPrefs.SetInt (CTaskUtil.PLAYER_ENERGY, this.m_PlayerEnergy.currentEnergy);
				PlayerPrefs.Save ();
			} else {
				CLog.LogError ("Error: Can not load song data.");
			}
			this.m_PlayerEnergy.OnUpdatePoint = null;
			this.m_PlayerEnergy.OnUpdateTimer = null;
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
