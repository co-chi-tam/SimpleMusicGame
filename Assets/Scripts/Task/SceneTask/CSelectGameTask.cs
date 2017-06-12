using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pul;

namespace SimpleGameMusic {
	public class CSelectGameTask : CSimpleTask {

		public CSelectGameTask () : base ()
		{
			this.taskName = "SelectGame";
			this.nextTask = "PlayGame";
		}

		public override void StartTask ()
		{
			base.StartTask ();
			var listSong = CTaskUtil.REFERENCES [CTaskUtil.LIST_SONG] as List<CSongData>;
			if (listSong != null) {
				CUISelectGame.Instance.LoadListSong (listSong);
			} else {
				CLog.LogError ("Error: Can not load song data.");
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
		}
		
	}
}
