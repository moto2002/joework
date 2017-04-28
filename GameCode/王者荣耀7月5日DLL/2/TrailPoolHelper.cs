using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TrailPoolHelper : MonoBehaviour, IPooledMonoBehaviour
{
	private bool _awaked;

	private bool _started;

	private TrailRenderer _trail;

	public void OnCreate()
	{
		if (this._awaked)
		{
			return;
		}
		this._awaked = true;
		this._trail = base.GetComponent<TrailRenderer>();
	}

	public void OnGet()
	{
		if (this._started)
		{
			return;
		}
		this._started = true;
		this.DoStart();
	}

	public void OnRecycle()
	{
		this._started = false;
	}

	private void Start()
	{
		this.OnGet();
	}

	private void Awake()
	{
		this.OnCreate();
	}

	private void DoStart()
	{
		base.StartCoroutine(TrailPoolHelper.ResetTrail(this._trail));
	}

	[DebuggerHidden]
	private static IEnumerator ResetTrail(TrailRenderer trail)
	{
		TrailPoolHelper.<ResetTrail>c__IteratorD <ResetTrail>c__IteratorD = new TrailPoolHelper.<ResetTrail>c__IteratorD();
		<ResetTrail>c__IteratorD.trail = trail;
		<ResetTrail>c__IteratorD.<$>trail = trail;
		return <ResetTrail>c__IteratorD;
	}
}
