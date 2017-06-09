using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleSingleton;

namespace SimpleGameMusic {
	public class CRootTask : CMonoSingleton<CRootTask> {

		[SerializeField]	private string m_CurrentTaskName;	

		private CTask m_CurrentTask;
		private CMapTask m_MapTask;
		private string m_PrevertTask;

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
			this.m_MapTask = new CMapTask ();
			this.m_CurrentTask = this.m_MapTask.GetFirstTask ();
			this.m_CurrentTaskName = this.m_CurrentTask.GetTaskName ();
		}

		protected virtual void Start ()
		{
			// First load
			this.m_CurrentTask.OnCompleteTask += NextTask;
			this.m_CurrentTask.StartTask ();
			// Other load
			CSceneManager.Instance.activeSceneChanged += (Scene oldScene, Scene currentScene) => {
				this.m_PrevertTask = oldScene.name;
				this.m_CurrentTask.OnCompleteTask += NextTask;
				this.m_CurrentTask.StartTask ();
				this.m_CurrentTaskName = this.m_CurrentTask.GetTaskName ();
			};
		}

		protected virtual void Update ()
		{
			if (this.m_CurrentTask != null) {
				this.m_CurrentTask.UpdateTask (Time.deltaTime);
			}
		}

		protected virtual void OnSceneLoaded() {
		
		}

		public void NextTask() {
			this.m_CurrentTask.EndTask ();
			this.m_CurrentTask = this.m_MapTask.GetTask (this.m_CurrentTask.nextTask);
			if (this.m_CurrentTask != null) {
				this.m_CurrentTask.Transmission ();
			}
			this.m_CurrentTaskName = this.m_CurrentTask.GetTaskName ();
		}

		public void PrevertTask() {
			this.m_CurrentTask.EndTask ();
			this.m_CurrentTask = this.m_MapTask.GetTask (this.m_PrevertTask);
			if (this.m_CurrentTask != null) {
				this.m_CurrentTask.Transmission ();
			}
			this.m_CurrentTaskName = this.m_CurrentTask.GetTaskName ();
		}

		public virtual CTask GetCurrentTask() {
			return this.m_CurrentTask;
		}
		
	}
}
