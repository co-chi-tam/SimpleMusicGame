using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CLoadingResourceTask : CSimpleTask {

		private CResourceManager m_ResourceManager;

		public CLoadingResourceTask () : base ()
		{
			this.taskName = "LoadingResource";
			this.nextTask = "SelectGame";
		}

		public override void StartTask ()
		{
			base.StartTask ();
			this.m_ResourceManager = new CResourceManager (1, "http://www.dropbox.com/s/ivrd48us6z2hyts/cactus_leafy_go.go?dl=1", true);
		}

		public override void UpdateTask (float dt)
		{
			base.UpdateTask (dt);
		}
	}
}
