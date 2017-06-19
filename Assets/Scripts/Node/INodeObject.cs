using System;
using UnityEngine;

namespace SimpleGameMusic {
	public interface INodeObject {

		float GetValue();
		void SetValue(float value);
		bool GetActive();
		void SetActive(bool value);
		
	}
}
