using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CTask : ITask {

		public string taskName;

		public string nextTask;

		public Action OnCompleteTask;

		public static Dictionary<string, object> taskReferences = new Dictionary<string, object>();

		protected bool m_IsCompleteTask = false;

		public CTask ()
		{
			this.taskName = string.Empty;
			this.nextTask = string.Empty;
		}

		public virtual void StartTask() {
			
		}

		public virtual void UpdateTask(float dt) {
			
		}

		public virtual void EndTask() {
			this.OnCompleteTask = null;
			this.m_IsCompleteTask = false;
		}

		public virtual void Transmission() {
		
		}

		public virtual void OnTaskCompleted() {
			this.m_IsCompleteTask = true;
			if (this.OnCompleteTask != null) {
				this.OnCompleteTask ();
			}
		}

		public virtual void OnSceneLoading() {

		}

		public virtual void OnSceneLoaded() {

		}

		public virtual bool IsCompleteTask() {
			return this.m_IsCompleteTask;
		}

		public virtual string GetTaskName() {
			return this.taskName;
		}
		
	}
}
