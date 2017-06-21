using System;
using UnityEngine;

namespace SimpleMusicGame {
	[Serializable]
	public class CSongData {

		public string songName;
		public string displaySongName;
		public int hardPoint;
		public int nodeScore;
		public string categoryName;

		public CSongData ()
		{
			this.songName 			= string.Empty;
			this.displaySongName 	= string.Empty;
			this.hardPoint 			= 0;
			this.nodeScore	= 0;
			this.categoryName 		= string.Empty;
		}

		public override string ToString ()
		{
			return string.Format ("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", 
				this.songName, 
				this.displaySongName, 
				this.hardPoint, 
				this.nodeScore, 
				this.categoryName
			);
		}

	}
}
