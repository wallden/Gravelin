using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JavelinPhysics : MonoBehaviour
{
	private const int MaxTurnPerSecond = 40;
	private const int LockOnDistance = 20;
	private Rigidbody _rigidBody;
	private IEnumerable<Collider> _playerColliders;
	private GameObject _owner;

	public void Start()
	{
		_rigidBody = GetComponent<Rigidbody>();
		_playerColliders =
			FindObjectsOfType<GameObject>()
				.Where(x => x.name.Contains("Player") && x != _owner)
				.Select(x => x.GetComponent<Collider>())
				.ToList();
	}

	public void Update()
	{
		var playerTargetsInReach = _playerColliders
			.Select(x => x.bounds.center - transform.position)
			.Where(x => x.magnitude < LockOnDistance)
			.OrderBy(x => x.magnitude);

		var selectedTarget = SelectTarget(playerTargetsInReach);

		if(selectedTarget != null)
		{
			Debug.DrawLine(transform.position, selectedTarget.Position + transform.position, Color.red);

			var targetAimRotation = Quaternion.LookRotation(selectedTarget.Position.normalized);
			var turnRatio = Mathf.Min((MaxTurnPerSecond/selectedTarget.Angle)*Time.deltaTime, 1);
			var turnToTargetRotation = Quaternion.Slerp(transform.rotation, targetAimRotation, turnRatio);
			Debug.DrawLine(transform.position, transform.position + turnToTargetRotation*Vector3.forward*5);
			_rigidBody.velocity = turnToTargetRotation*Vector3.forward*_rigidBody.velocity.magnitude;
			transform.rotation = turnToTargetRotation;
		}
		else
		{
			transform.rotation = Quaternion.LookRotation(_rigidBody.velocity);
		}
	}

	private class SpearTarget
	{
		public Vector3 Position { get; private set; }
		public float Angle { get; private set; }

		public SpearTarget(Vector3 position, float angle)
		{
			Angle = angle;
			Position = position;
		}
	}

	private SpearTarget SelectTarget(IOrderedEnumerable<Vector3> playerTargetsInReach)
	{
		if (playerTargetsInReach.Any())
		{
			var playerTarget = playerTargetsInReach.First();
			var angle = Vector3.Angle(transform.forward, playerTarget.normalized);
			if (angle < 90)
			{
				return new SpearTarget(playerTarget, angle);
			}
		}
		return null;
	}

	public void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag != "Spear")
		{
			GetComponent<Collider>().isTrigger = true;

			transform.position = collision.contacts[0].point - transform.forward;
			gameObject.AddComponent<FixedJoint>().connectedBody = collision.rigidbody;

			var delayedDestroy = gameObject.AddComponent<DelayedDestroy>();
			delayedDestroy.DelayTime = 10;

			Destroy(this);
		}
	}

	public void SetOwner(GameObject owner)
	{
		_owner = owner;
	}
}