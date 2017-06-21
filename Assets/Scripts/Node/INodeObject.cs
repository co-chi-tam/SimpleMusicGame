using System;
using UnityEngine;

namespace SimpleMusicGame {
	public interface INodeObject {

		float GetValue();
		void SetValue(float value);
		bool GetActive();
		void SetActive(bool value);
		
	}
}
