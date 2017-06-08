using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			CTask.taskReferences ["SELECTED_GAME"] = "Yeu-5";
		}
		
	}
}
