using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GrapplingHook : MonoBehaviour
{
	public Transform Camera;

	public void Update ()
	{
		Grapple();
	}

	private void Grapple()
	{
		if (CrossPlatformInputManager.GetButtonDown("Fire1"))
		{
			RaycastHit hit;

			if (Physics.Raycast(transform.position, Camera.transform.forward, out hit))
			{
				var hitSurface = hit.transform;
				Debug.Log(hit.point);
				var joint = hitSurface.transform.gameObject.AddComponent<ConfigurableJoint>();
				ConfigureJoint(joint);
				joint.connectedBody = transform.gameObject.GetComponent<Rigidbody>();
			}
		}
	}

	ConfigurableJoint ConfigureJoint(ConfigurableJoint joint)
	{
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;

		return joint;
	}
}
