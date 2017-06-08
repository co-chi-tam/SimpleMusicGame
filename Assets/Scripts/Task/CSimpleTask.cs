using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleGameMusic {
	public class CSimpleTask : CTask {

		public CSimpleTask () : base ()
		{
			this.taskName = string.Empty;
			this.nextTask = string.Empty;
		}

		public override void StartTask ()
		{
			base.StartTask ();
			if (this.taskName != SceneManager.GetActiveScene ().name) {
				CHandleEvent.Instance.AddEvent (this.LoadScene (this.taskName), null);	
			}
		}

		protected virtual IEnumerator LoadScene(string name) {
			var sceneLoading = SceneManager.LoadSceneAsync (this.taskName);
			this.OnSceneLoading ();
			yield return sceneLoading;
			this.OnSceneLoaded ();
		}
		
	}
}
