using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CPlayGameTask : CSimpleTask {

		public CPlayGameTask () : base ()
		{
			this.taskName = "PlayGame";
			this.nextTask = "SelectGame";
		}

		public override void StartTask ()
		{
			base.StartTask ();
		}

	}
}
