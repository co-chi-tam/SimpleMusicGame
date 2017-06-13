using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleGameMusic {
	public class CMapTask {

		#region Properties

		private Dictionary<string, CTask> m_Map;

		#endregion

		#region Constructor

		public CMapTask ()
		{
			this.m_Map = new Dictionary<string, CTask> ();
			this.LoadMap ();
		}

		#endregion

		#region Main methods

		public virtual void LoadMap() {
			this.m_Map ["Intro"] 			= new CIntroTask ();
			this.m_Map ["LoadingResource"] 	= new CLoadingResourceTask ();
			this.m_Map ["LocalSetting"] 	= new CLocalSettingTask ();
			this.m_Map ["Tutorial"] 		= new CTutorialTask ();
			this.m_Map ["SelectGame"] 		= new CSelectGameTask ();
			this.m_Map ["PlayGame"] 		= new CPlayGameTask ();
		}

		#endregion

		#region Getter && Setter

		public virtual CTask GetFirstTask() {
			var keys = this.m_Map.Keys.ToList();
			var firstTask = this.m_Map[keys[0]];
			firstTask.nextTask = this.m_Map[keys[1]].GetTaskName();
			return firstTask;
		}

		public virtual CTask GetTask(string name) {
			if (this.m_Map.ContainsKey (name)) {
				return this.m_Map [name];
			}
			return null;
		}

		#endregion
		
	}
}
