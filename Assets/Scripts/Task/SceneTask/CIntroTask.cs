using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMusicGame {
	public class CIntroTask : CSimpleTask {

		#region Properties

		private float m_Timer = 3f;

		#endregion

		#region Constructor

		public CIntroTask () : base ()
		{
			this.taskName = "Intro";
			this.nextTask = "LoadingTask";
#if TEST_CLEAR_PLAYERPREFS
			PlayerPrefs.DeleteAll ();
#endif
		}

		#endregion

		#region Implementation Task

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
				this.m_Timer = float.MaxValue;
			}
		}

		#endregion
		
	}
}
