using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMusicGame {
	public class CPlayerEnergy {

		public int currentEnergy;
		public int maxEnergy;
		public int incrementEnergy;
		public float timePerEnergy;
		public long currentTimer;
		public long saveTimer;
		public long firstTimer;

		public Action OnUpdateEnergy;
		public Action<float> OnUpdate;

		private float m_PerOneUpdate;
		private bool m_StartCouting;

		public CPlayerEnergy ()
		{
			this.currentEnergy 	= 10;
			this.maxEnergy 		= 10;
			this.incrementEnergy = 1;
			this.timePerEnergy 	= 60 * 15f; // 15 Minute
			this.currentTimer	= DateTime.UtcNow.Ticks;
			this.saveTimer 		= DateTime.UtcNow.Ticks;
			this.firstTimer		= DateTime.UtcNow.Ticks;
			this.m_PerOneUpdate = this.timePerEnergy;
		}

		public CPlayerEnergy(int current, int max, int increment, float timePerEnergy, long timer, long save, long first) {
			this.currentEnergy 	= current;
			this.maxEnergy 		= max;
			this.incrementEnergy = increment;
			this.timePerEnergy 	= timePerEnergy;
			this.currentTimer	= timer;
			this.saveTimer		= save;
			this.firstTimer		= first;
			this.m_PerOneUpdate = this.timePerEnergy;
		}

		public void StartCounting() {
			CHandleEvent.Instance.AddEvent (this.HandleUpdateCounting(Time.fixedDeltaTime), null);
			this.m_StartCouting = true;
			this.CalculateTimer();
#if TEST_ONE_HOURS_ENERGY
			this.saveTimer = DateTime.UtcNow.AddHours (-1).Ticks;
#elif TEST_ONE_DAY_ENERGY
			this.saveTimer = DateTime.UtcNow.AddDays (-1).Ticks;
#elif TEST_END_ENERGY
			this.currentEnergy = 0;
#endif
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
					var energy = this.currentEnergy + this.incrementEnergy;
					this.SetEnergy (energy);
					this.saveTimer = this.currentTimer;
					if (this.OnUpdateEnergy != null) {
						this.OnUpdateEnergy ();
					}
				}
			}
		}

		public void CalculateEnergy() {
			var lostTime = this.currentTimer - this.saveTimer;
			var result = lostTime / (this.timePerEnergy * TimeSpan.TicksPerSecond);
			var energy = this.currentEnergy + Mathf.CeilToInt(result);
			this.SetEnergy (energy);
		}

		public void CalculateTimer() {
			var lostTimer = this.currentTimer - this.firstTimer;
			var result = lostTimer / TimeSpan.TicksPerSecond;
			this.m_PerOneUpdate = this.timePerEnergy - (result % this.timePerEnergy);
		}

		public void SetEnergy(int value) {
#if TEST_ENERGY_FULL
			this.currentEnergy = this.maxEnergy;
#else
			this.currentEnergy = value <= 0 ? 0 : value >= this.maxEnergy ? this.maxEnergy : value;
#endif
		}

		public override string ToString ()
		{
			return string.Format ("Energy {0}/{1}", this.currentEnergy, this.maxEnergy);
		}

	}

}
