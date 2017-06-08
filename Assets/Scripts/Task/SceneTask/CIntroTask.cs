using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CIntroTask : CSimpleTask {

		private float m_Timer = 3f;

		public CIntroTask () : base ()
		{
			this.taskName = "Intro";
			this.nextTask = "LoadingTask";
		}

		public override void UpdateTask (float dt)
		{
			base.UpdateTask (dt);
			if (this.m_IsCompleteTask == false) {
				this.m_Timer -= dt;
				this.m_IsCompleteTask = this.m_Timer <= 0f;
			} else {
				if (this.OnCompleteTask != null) {
					this.OnCompleteTask ();
				}
			}
		}
		
	}
}
