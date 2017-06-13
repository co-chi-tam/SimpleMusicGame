using System;
using UnityEngine;

namespace SimpleGameMusic {
	[Serializable]
	public class CTranslateData  {

		public string transCode;
		public string transDisplay;

		public CTranslateData ()
		{
			this.transCode = string.Empty;
			this.transDisplay = string.Empty;
		}

		public override string ToString ()
		{
			return string.Format ("\"{0}\",\"{1}\"", 
				this.transCode, 
				this.transDisplay
			);
		}
		
	}
}
