using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMusicGame {
	public class CPlayerEnergy : CCountdownTimer {

		#region Properties

		public int incrementEnergy;
		public int currentEnergy;
		public int maxEnergy;

		#endregion

		#region Constructor

		public CPlayerEnergy () : base()
		{
			this.incrementEnergy = 1;
			this.currentEnergy 	= 10;
			this.maxEnergy 		= 10;
		}

		public CPlayerEnergy(int current, int max, int increment, float timePerEnergy, long timer, long save, long first, bool repeating) 
			: base (timePerEnergy, timer, save, first, repeating) {

			this.incrementEnergy = increment;
#if TEST_ENERGY_FULL
			this.currentEnergy 	= max;
#else
			this.currentEnergy 	= current;
#endif
			this.maxEnergy 		= max;
		}

		#endregion

		#region Main methods

		/// <summary>
		/// Starts the counting timer.
		/// Add HandleEvent call OnUpdate, OnUpdatePoint event
		/// </summary>
		public override void StartCounting() {
			base.StartCounting ();
#if TEST_ONE_HOURS_ENERGY
			this.saveTimer = DateTime.UtcNow.AddHours (-1).Ticks;
#elif TEST_ONE_DAY_ENERGY
			this.saveTimer = DateTime.UtcNow.AddDays (-1).Ticks;
#elif TEST_END_ENERGY
			this.currentEnergy = 0;
#endif
		}

		/// <summary>
		/// Countings the comlete.
		/// </summary>
		public override void CountingComlete ()
		{
			base.CountingComlete ();
			this.AddEnergy ();
		}

		/// <summary>
		/// Adds the energy.
		/// </summary>
		public virtual void AddEnergy() {
			var energy = this.currentEnergy + this.incrementEnergy;
			this.SetEnergy (energy);
		}

		/// <summary>
		/// Calculates the energy.
		/// </summary>
		public virtual void CalculateEnergy() {
			var point = this.GetPoint ();
			var savePoint = Mathf.FloorToInt (point);
			var energy = this.currentEnergy + savePoint;
			this.SetEnergy (energy);
		}

		/// <summary>
		/// Calculates the save timer.
		/// </summary>
		public virtual void CalculateSaveTimer() {
			var updateTicks = this.CalculateTimeToTicks (this.timePerPoint - this.m_TimerUpdate);
			this.saveTimer = this.currentTimer - updateTicks;
		}

		/// <summary>
		/// Sets the energy.
		/// </summary>
		public virtual void SetEnergy(int value) {
#if TEST_ENERGY_FULL
			this.currentEnergy = this.maxEnergy;
#else
			this.currentEnergy = value <= 0 ? 0 : value >= this.maxEnergy ? this.maxEnergy : value;
#endif
		}

		/// <summary>
		/// Returns a Energy currentEnergy / maxEnergy.
		/// </summary>
		public override string ToString ()
		{
			return string.Format ("Energy {0}/{1}", this.currentEnergy, this.maxEnergy);
		}

		#endregion

	}

}
