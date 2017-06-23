using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMusicGame {
	public class CPlayGameTask : CSimpleTask {

		#region Properties

		private CUIManager m_UIManager;

		#endregion

		#region Constructor

		public CPlayGameTask () : base () {
			this.taskName = "PlayGame";
			this.nextTask = "SelectSong";
		}

		#endregion

		#region Implementation Task

		public override void StartTask ()
		{
			base.StartTask ();
			this.m_UIManager = CUIManager.GetInstance ();

			var soundVolume = (float)CTaskUtil.Get (CTaskUtil.GAME_SOUND_VOLUME);
			this.m_UIManager.SetSoundVolume (soundVolume);
		}

		#endregion

	}
}
