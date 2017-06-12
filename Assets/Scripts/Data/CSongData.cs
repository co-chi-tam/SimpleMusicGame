using System;
using UnityEngine;

namespace SimpleGameMusic {
	[Serializable]
	public class CSongData {

		public string songName;
		public string displaySongName;
		public int hardPoint;
		public int simpleNodeScore;
		public int holdNodeScore;
		public string categoryName;

		public CSongData ()
		{
			this.songName 			= string.Empty;
			this.displaySongName 	= string.Empty;
			this.hardPoint 			= 0;
			this.simpleNodeScore	= 0;
			this.holdNodeScore		= 0;
			this.categoryName 		= string.Empty;
		}

		public override string ToString ()
		{
			return string.Format ("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"", 
				this.songName, 
				this.displaySongName, 
				this.hardPoint, 
				this.simpleNodeScore, 
				this.holdNodeScore,
				this.categoryName
			);
		}

	}
}
