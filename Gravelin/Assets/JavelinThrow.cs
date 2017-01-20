using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class JavelinThrow : MonoBehaviour
{
	public Transform Camera;

	private GameObject _spearTemplate;

	private void Start()
	{
		_spearTemplate = Resources.Load<GameObject>("Spear");
		if (_spearTemplate == null)
		{
			throw new Exception("Can't load spear");
		}
		if (Camera == null)
		{
			throw new Exception("Camera reference");
		}
	}

	private void Update()
	{
		if (CrossPlatformInputManager.GetButtonDown("Fire1"))
		{
			var spear = Instantiate(_spearTemplate);
			spear.transform.position = transform.position + transform.right + transform.forward;
			spear.transform.rotation = transform.rotation*Quaternion.Euler(Camera.transform.eulerAngles.x, 0, 0);
			var rigidBody = spear.GetComponent<Rigidbody>();
			rigidBody.AddForce(spear.transform.forward * 1000);
		}
	}
}
