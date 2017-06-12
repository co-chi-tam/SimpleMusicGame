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
			CLog.Init ();
		}

		protected virtual void Start ()
		{
			// First load
			this.m_CurrentTask.Transmission ();
			this.m_PrevertTask = this.m_CurrentTaskName;
			this.SetupTask();
			// Other load
			CSceneManager.Instance.activeSceneChanged += (Scene oldScene, Scene currentScene) => {
				this.m_PrevertTask = oldScene.name;
				this.SetupTask();
			};
		}

		protected virtual void Update ()
		{
			if (this.m_CurrentTask != null) {
				this.m_CurrentTask.UpdateTask (Time.deltaTime);
			}
		}

		public void NextTask() {
			this.TransmissionTask (this.m_CurrentTask.nextTask);
		}

		public void PrevertTask() {
			this.TransmissionTask (this.m_PrevertTask);
		}

		private void SetupTask() {
			this.m_CurrentTask.OnCompleteTask -= NextTask;
			this.m_CurrentTask.OnCompleteTask += NextTask;
			this.m_CurrentTask.StartTask ();
			this.m_CurrentTaskName = this.m_CurrentTask.GetTaskName ();
		}

		private void TransmissionTask(string taskName) {
			this.m_CurrentTask.EndTask ();
			this.m_CurrentTask = this.m_MapTask.GetTask (taskName);
			if (this.m_CurrentTask != null) {
				this.m_CurrentTask.Transmission ();
				if (this.m_CurrentTask.taskName != CSceneManager.Instance.GetActiveSceneName ()) {
					CHandleEvent.Instance.AddEvent (this.LoadScene (this.m_CurrentTask.taskName), null);	
				}
			}
			this.m_CurrentTaskName = this.m_CurrentTask.GetTaskName ();
		}

		protected virtual IEnumerator LoadScene(string name) {
			var sceneLoading = CSceneManager.Instance.LoadSceneAsync (name);
			this.m_CurrentTask.OnSceneLoading ();
			yield return sceneLoading;
			this.m_CurrentTask.OnSceneLoaded ();
		}

		public virtual CTask GetCurrentTask() {
			return this.m_CurrentTask;
		}

	}

}
