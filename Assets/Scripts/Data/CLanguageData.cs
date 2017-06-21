using System;
using UnityEngine;

namespace SimpleMusicGame {
	[Serializable]
	public class CLanguageData {

		public string laName;
		public string laDisplay;
		public string laFile;

		public CLanguageData ()
		{
			this.laName 	= string.Empty;
			this.laDisplay 	= string.Empty;
			this.laFile 	= string.Empty;
		}

		public override string ToString ()
		{
			return string.Format ("\"{0}\",\"{1}\",\"{2}\"", 
				this.laName, 
				this.laDisplay, 
				this.laFile
			);
		}
		
	}
}
