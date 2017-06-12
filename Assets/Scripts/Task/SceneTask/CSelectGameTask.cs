using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pul;

namespace SimpleGameMusic {
	public class CSelectGameTask : CSimpleTask {

		private List<CSongData> m_ListSong;

		public CSelectGameTask () : base ()
		{
			this.taskName = "SelectGame";
			this.nextTask = "PlayGame";
		}

		public override void StartTask ()
		{
			base.StartTask ();
			var listSongTextAsset = CAssetBundleManager.LoadResourceOrBundle <TextAsset> ("List-song");
			if (listSongTextAsset != null) {
				this.m_ListSong = CSVUtil.ToObject<CSongData> (listSongTextAsset.text);
			} else {
				CLog.LogError ("Error: Can not load song data.");
			}
			CUISelectGame.Instance.LoadListSong (this.m_ListSong);
		}

		public override void EndTask ()
		{
			base.EndTask ();
			var name = CTaskUtil.REFERENCES [CTaskUtil.SELECTED_SONG].ToString();
			for (int i = 0; i < this.m_ListSong.Count; i++) {
				if (this.m_ListSong [i].songName.Equals (name)) {
					CTaskUtil.REFERENCES [CTaskUtil.DATA_SONG] = this.m_ListSong [i];
					break;
				}
			}
		}
		
	}
}
