﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleSingleton;

namespace SimpleGameMusic {
	public class CRootTask : CMonoSingleton<CRootTask> {

		private CMapTask m_MapTask;
		private CTask m_CurrentTask;
		private string m_PrevertTask;

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
			this.m_MapTask = new CMapTask ();
			this.m_CurrentTask = this.m_MapTask.GetFirstTask ();
		}

		protected virtual void Start ()
		{
			this.m_CurrentTask.OnCompleteTask += NextTask;
			this.m_CurrentTask.StartTask ();
			SceneManager.activeSceneChanged += (Scene oldScene, Scene currentScene) => {
				this.m_PrevertTask = oldScene.name;
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
				this.m_CurrentTask.OnCompleteTask += NextTask;
				this.m_CurrentTask.StartTask ();
			}
		}

		public void PrevertTask() {
			this.m_CurrentTask.EndTask ();
			this.m_CurrentTask = this.m_MapTask.GetTask (this.m_PrevertTask);
			if (this.m_CurrentTask != null) {
				this.m_CurrentTask.OnCompleteTask += NextTask;
				this.m_CurrentTask.StartTask ();
			}
		}

		public virtual CTask GetCurrentTask() {
			return this.m_CurrentTask;
		}
		
	}
}
