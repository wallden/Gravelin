using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class JavelinThrow : MonoBehaviour
{
	public Transform Camera;

	private GameObject _spearTemplate;
    private Player _player;

	public void Start()
	{
	    _player = GetComponent<Player>();
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

	public void Update()
	{
	    if (_player.isAlive && CrossPlatformInputManager.GetButtonDown("Fire2"))
		{
			CmdThrowSpear();
		}
	}

    void CmdThrowSpear()
    {
        var spear = Instantiate(_spearTemplate);
        spear.transform.position = transform.position + transform.right + transform.forward;
        spear.transform.rotation = transform.rotation * Quaternion.Euler(Camera.transform.eulerAngles.x, 0, 0);
        var rigidBody = spear.GetComponent<Rigidbody>();
        rigidBody.velocity = spear.transform.forward * 80;
    }
}
