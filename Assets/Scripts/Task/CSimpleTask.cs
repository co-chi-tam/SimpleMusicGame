using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleGameMusic {
	public class CSimpleTask : CTask {

		public CSimpleTask () : base ()
		{
			this.taskName = string.Empty;
			this.nextTask = string.Empty;
		}

		public override void Transmission ()
		{
			base.Transmission ();
		}
		
	}
}
