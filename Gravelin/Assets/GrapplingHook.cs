using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GrapplingHook : MonoBehaviour
{
	public Transform Camera;

	private GameObject _hookLineTemplate;
	private Rigidbody _rigidBody;
	private bool _grappling;
	private GameObject _hookLine;

	public void Start()
	{
		_hookLineTemplate = Resources.Load<GameObject>("HookLine");
		if (_hookLineTemplate == null)
		{
			throw new Exception("Can't load hook line");
		}
		_rigidBody = GetComponent<Rigidbody>();
	}

	public void Update ()
	{
		CheckGrapple();
	}

	private void CheckGrapple()
	{
		if (CrossPlatformInputManager.GetButtonDown("Fire1"))
		{
			if (!_grappling)
			{
				PerformNewGrapple();
			}
			else
			{
				ReleaseGrapple();
			}
		}
	}

	private void PerformNewGrapple()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, Camera.transform.forward, out hit))
		{
			_hookLine = Instantiate(_hookLineTemplate);
			_hookLine.transform.position = hit.point;
			var line = _hookLine.transform.FindChild("Line");
			line.rotation = Quaternion.LookRotation(transform.position - line.transform.position);
			line.localScale = new Vector3(line.localScale.x, line.localScale.y, hit.distance);
			var lineJoint = line.GetComponent<ConfigurableJoint>();
			lineJoint.connectedBody = _rigidBody;
			_grappling = true;
		}
	}

	private void ReleaseGrapple()
	{
		Destroy(_hookLine);
		_grappling = false;
	}
}
