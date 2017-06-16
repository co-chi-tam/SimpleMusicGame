using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleSingleton;

namespace SimpleGameMusic {
	public class CRootTask : CMonoSingleton<CRootTask> {

		#region Properties

		[SerializeField]	private string m_CurrentTaskName;	

		private CTask m_CurrentTask;
		private CMapTask m_MapTask;
		private string m_PreviousTask;

		#endregion

		#region Implementation MonoBehavious

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
			this.m_PreviousTask = this.m_CurrentTask.GetTaskName();
			this.SetupTask();
			// Other load
			CSceneManager.Instance.activeSceneChanged += (Scene oldScene, Scene currentScene) => {
				this.SetupTask();
			};
		}

		protected virtual void Update ()
		{
			if (this.m_CurrentTask != null) {
				this.m_CurrentTask.UpdateTask (Time.deltaTime);
			}
		}

		protected virtual void OnDestroy() {
			
		}

		protected virtual void OnApplicationQuit() {
			this.SaveTask ();
		}

		protected virtual void OnApplicationPause(bool value) {
			this.SaveTask ();
		}

		protected virtual void OnApplicationFocus(bool value) {
			this.SaveTask ();
		}

		#endregion

		#region Main methods

		public void NextTask() {
			this.TransmissionTask (this.m_CurrentTask.nextTask);
		}

		public void PreviousTask() {
			this.TransmissionTask (this.m_PreviousTask);
		}

		private void SetupTask() {
			this.m_CurrentTask.OnCompleteTask -= NextTask;
			this.m_CurrentTask.OnCompleteTask += NextTask;
			this.m_CurrentTask.StartTask ();
			this.m_CurrentTaskName = this.m_CurrentTask.GetTaskName ();
		}

		private void TransmissionTask(string taskName) {
			this.m_PreviousTask = this.m_CurrentTask.GetTaskName();
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

		private void SaveTask() {
			var playerEnergy = CTaskUtil.REFERENCES [CTaskUtil.PLAYER_ENERGY] as CPlayerEnergy;
			playerEnergy.CalculateTimer ();
			playerEnergy.CalculateEnergy ();
			PlayerPrefs.SetInt (CTaskUtil.PLAYER_ENERGY, playerEnergy.currentEnergy);
			PlayerPrefs.SetString (CTaskUtil.PLAYER_ENEGY_SAVE_TIMER, playerEnergy.saveTimer.ToString());
			PlayerPrefs.Save ();
		}

		#endregion

	}

}
