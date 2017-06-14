using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pul;

namespace SimpleGameMusic {
	public class CSelectGameTask : CSimpleTask {

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
			var listSongs = CTaskUtil.REFERENCES [CTaskUtil.LIST_SONG] as List<CSongData>;
			if (listSongs != null) {
				CUISelectGame.Instance.LoadCategories (listSongs);
			} else {
				CLog.LogError ("Error: Can not load song data.");
			}
		}

		public override void UpdateTask (float dt)
		{
			base.UpdateTask (dt);
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
		}

		#endregion
		
	}
}
