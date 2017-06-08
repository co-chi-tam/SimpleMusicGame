using System;
using UnityEngine;

namespace SimpleGameMusic {
	public interface ITask {

		void StartTask();
		void UpdateTask(float dt);
		void EndTask();

		bool IsCompleteTask ();

		string GetTaskName();

	}
}
