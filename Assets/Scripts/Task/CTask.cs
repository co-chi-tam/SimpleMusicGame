using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CTask : ITask {

		public string taskName;

		public string nextTask;

		public Action OnCompleteTask;

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

		public virtual void OnTaskFail() {
			this.m_IsCompleteTask = false;
		}

		public virtual void OnSceneLoading() {

		}

		public virtual void OnSceneLoaded() {

		}

		public virtual bool IsCompleteTask() {
			return this.m_IsCompleteTask;
		}

		public virtual bool IsHiddenTask() {
			return false;
		}

		public virtual string GetTaskName() {
			return this.taskName;
		}

	}
}
