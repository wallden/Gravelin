using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JavelinPhysics : MonoBehaviour
{
	private Rigidbody _rigidBody;

	public void Start()
	{
		_rigidBody = GetComponent<Rigidbody>();
	}

	public void Update()
	{
		transform.rotation = Quaternion.LookRotation(_rigidBody.velocity);
	}

	public void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag != "Spear")
		{
			_rigidBody.isKinematic = true;
			GetComponent<Collider>().isTrigger = true;

			transform.position = collision.contacts[0].point - transform.forward;

			var delayedDestroy = gameObject.AddComponent<DelayedDestroy>();
			delayedDestroy.DelayTime = 10;

			Destroy(this);
		}
	}
}