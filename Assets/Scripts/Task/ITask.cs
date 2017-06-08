using System;
using UnityEngine;

namespace SimpleGameMusic {
	public interface ITask {

		void StartTask();
		void UpdateTask(float dt);
		void EndTask();
		void Transmission();

		bool IsCompleteTask ();

		string GetTaskName();

	}
}
