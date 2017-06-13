using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CTutorialTask : CSimpleTask {

		#region Properties

		private CUITutorialManager m_TutorialManager;
		private float m_Timer = 3f;

		#endregion

		#region Constructor

		public CTutorialTask () : base ()
		{
			this.taskName = "Tutorial";
			this.nextTask = "SelectGame";
		}

		#endregion

		#region Implementation Task

		public override void StartTask ()
		{
			base.StartTask ();
			this.m_TutorialManager = CUITutorialManager.GetInstance ();
		}

		public override void UpdateTask (float dt)
		{
			base.UpdateTask (dt);
			if (this.m_TutorialManager == null)
				return;
			if (this.m_TutorialManager.GetTutorialPhase () == CUITutorialManager.ETutorialPhase.End) {
				m_Timer -= dt;
				if (m_Timer <= 0f) {
					this.OnTaskCompleted ();
					m_Timer = float.MaxValue;
				}
			}
		}

		#endregion
		
	}
}
