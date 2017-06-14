using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CPlayerEnergy {

		public int currentEnergy;
		public int maxEnergy;
		public float timePerEnergy;
		public long currentTimer;
		public long saveTimer;

		public Action OnUpdateEnergy;
		public Action<float> OnUpdate;

		private float m_PerOneUpdate;
		private bool m_StartCouting;

		public CPlayerEnergy ()
		{
			this.currentEnergy 	= 10;
			this.maxEnergy 		= 10;
			this.timePerEnergy 	= 60 * 1f; // 15 Minute
			this.currentTimer	= DateTime.UtcNow.Ticks;
			this.saveTimer 		= DateTime.UtcNow.Ticks;
			this.m_PerOneUpdate = this.timePerEnergy;
		}

		public CPlayerEnergy(int current, int max, float timePerEnergy, long timer, long saveTimer) {
			this.currentEnergy 	= current;
			this.maxEnergy 		= max;
			this.timePerEnergy 	= timePerEnergy;
			this.currentTimer	= timer;
			this.saveTimer		= saveTimer;
			this.m_PerOneUpdate = this.timePerEnergy;
		}

		public void StartCounting() {
			CHandleEvent.Instance.AddEvent (this.HandleUpdateCounting(Time.fixedDeltaTime), null);
			this.m_StartCouting = true;
			var lostTime = this.currentTimer - this.saveTimer;
			var result = lostTime / (this.timePerEnergy * 1000f * 1000f) / 10f;
			var reUpdateTime = result - (float)Math.Truncate (result);
			this.m_PerOneUpdate = this.timePerEnergy * reUpdateTime;
		}

		public IEnumerator HandleUpdateCounting(float fdt) {
			while (this.m_StartCouting) {
				this.m_PerOneUpdate -= fdt;
				yield return WaitHelper.WaitFixedUpdate;
				this.currentTimer = DateTime.UtcNow.Ticks;
				if (OnUpdate != null) {
					OnUpdate (this.m_PerOneUpdate);
				}
				if (this.m_PerOneUpdate <= 0f) {
					this.m_PerOneUpdate = this.timePerEnergy;
					var energy = this.currentEnergy + 1;
					this.currentEnergy = energy >= this.maxEnergy ? this.maxEnergy : energy;
					if (this.OnUpdateEnergy != null) {
						this.OnUpdateEnergy ();
					}
				}
			}
		}

		public void CalculateEnergy() {
			var lostTime = this.currentTimer - this.saveTimer;
			var result = lostTime / (this.timePerEnergy * 1000f * 1000f) / 10f;
			var energy = this.currentEnergy + (result >= 1f ? 1 : 0);
			this.currentEnergy = energy >= this.maxEnergy ? this.maxEnergy : energy;
		}

		public override string ToString ()
		{
			return string.Format ("Energy {0}/{1}", this.currentEnergy, this.maxEnergy);
		}

	}

}
