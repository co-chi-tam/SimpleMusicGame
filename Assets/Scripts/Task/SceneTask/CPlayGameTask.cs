using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CPlayGameTask : CSimpleTask {

		#region Constructor

		public CPlayGameTask () : base () {
			this.taskName = "PlayGame";
			this.nextTask = "SelectSong";
		}

		#endregion

	}
}
