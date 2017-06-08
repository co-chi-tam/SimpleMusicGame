using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CSelectGameTask : CSimpleTask {

		private float m_Timer = 3f;

		public CSelectGameTask () : base ()
		{
			this.taskName = "SelectGame";
			this.nextTask = "PlayGame";
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
