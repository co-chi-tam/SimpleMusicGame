using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCountdownTimer {
	
	#region Properties

	public float timePerPoint;
	public long currentTimer;
	public long saveTimer;
	public long firstTimer;
	public bool repeatCounting;

	public Action OnUpdatePoint;
	public Action<float> OnUpdateTimer;

	protected float m_TimerUpdate;
	protected bool m_StartCouting;

	#endregion

	#region Constructor

	public CCountdownTimer ()
	{
		this.timePerPoint 	= 60 * 10f; // 10 Minutes
		this.currentTimer	= DateTime.UtcNow.Ticks;
		this.saveTimer 		= DateTime.UtcNow.Ticks;
		this.firstTimer		= DateTime.UtcNow.Ticks;
		this.repeatCounting = false;
	}

	public CCountdownTimer(float timePerEnergy, long timer, long save, long first, bool repeating) {
		this.timePerPoint 	= timePerEnergy;
		this.currentTimer	= timer;
		this.saveTimer		= save;
		this.firstTimer		= first;
		this.repeatCounting = repeating;
	}

	#endregion

	#region Main methods

	/// <summary>
	/// Starts the counting timer.
	/// Add HandleEvent call OnUpdate, OnUpdatePoint event  
	/// </summary>
	public virtual void StartCounting() {
		this.m_TimerUpdate  = this.timePerPoint;
		CHandleEvent.Instance.AddEvent (this.HandleUpdateCounting(Time.fixedDeltaTime), null);
		this.m_StartCouting = true;
	}

	/// <summary>
	/// Handles the update counting.
	/// </summary>
	public virtual IEnumerator HandleUpdateCounting(float fdt) {
		while (this.m_StartCouting) {
			this.m_TimerUpdate -= fdt;
			yield return WaitHelper.WaitFixedUpdate;
			this.currentTimer = DateTime.UtcNow.Ticks;
			if (OnUpdateTimer != null) {
				OnUpdateTimer (this.m_TimerUpdate);
			}
			if (this.m_TimerUpdate <= 0f) {
				this.m_TimerUpdate = this.timePerPoint;
				if (this.repeatCounting) {
					this.saveTimer = this.currentTimer;
				}
				if (this.OnUpdatePoint != null) {
					this.OnUpdatePoint ();
				}
				this.CountingComlete ();
			}
		}
	}

	public virtual void CountingComlete() {
		
	}

	/// <summary>
	/// Calculates the timer.
	/// Need call first times
	/// </summary>
	public virtual void CalculateTimer() {
		var lostTimer = this.currentTimer - this.saveTimer;
		var result = lostTimer / TimeSpan.TicksPerSecond;
		this.m_TimerUpdate = this.timePerPoint - (result % this.timePerPoint);
	}

	/// <summary>
	/// Calculates the time to ticks.
	/// </summary>
	public virtual long CalculateTimeToTicks(float time) {
		var result = time * TimeSpan.TicksPerSecond;
		return (long) result;
	}

	/// <summary>
	/// Calculates the point.
	/// </summary>
	public virtual int GetPoint() {
		var lostTime = this.GetLostTime();
		var result = lostTime / this.CalculateTimeToTicks(this.timePerPoint);
		return Mathf.FloorToInt (result);
	}

	/// <summary>
	/// Calculates the lost time.
	/// </summary>
	public virtual long GetLostTime() {
		var lostTime = this.currentTimer - this.saveTimer;
		return lostTime;
	}

	#endregion

}
