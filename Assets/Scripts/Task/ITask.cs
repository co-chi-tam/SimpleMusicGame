﻿using System;
using UnityEngine;

namespace SimpleGameMusic {
	public interface ITask {

		void StartTask();
		void UpdateTask(float dt);
		void EndTask();
		void Transmission();
		void SaveTask();

		void OnTaskCompleted();
		void OnTaskFail();

		bool IsCompleteTask ();

		string GetTaskName();

	}
}
