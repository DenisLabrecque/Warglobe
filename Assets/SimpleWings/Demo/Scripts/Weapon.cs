﻿using UnityEngine;

/// <summary>
/// Manages a trail and rigidbody so that a weapon can be dropped by WeaponDropper.
/// Very hastily written and only for the demo.
/// </summary>
public class SWWeapon : MonoBehaviour
{
	public TrailRenderer trail;
	//public Engine engine;

	public Rigidbody Rigidbody { get; internal set; }

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		//if (engine != null)
		//{
		//	engine.throttle = 0.0f;
		//}
		if (Rigidbody != null)
		{
			Rigidbody.isKinematic = true;
		}
		if (trail != null)
		{
			trail.enabled = false;
		}
	}

	public void Fire(Vector3 velocity)
	{
		//if (engine != null)
		//{
		//	engine.throttle = 1.0f;
		//}
		if (Rigidbody != null)
		{
			Rigidbody.velocity = velocity;
			Random.InitState(GetInstanceID());
			Rigidbody.angularVelocity = Random.insideUnitSphere * 15.0f * Mathf.Deg2Rad;
			Rigidbody.isKinematic = false;
		}
		if (trail != null)
		{
			trail.enabled = true;
		}
	}
}
