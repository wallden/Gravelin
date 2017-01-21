using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class JavelinThrow : MonoBehaviour
{
	public Transform Camera;

	private GameObject _spearTemplate;
    private Player _player;
	private Camera _cameraComponent;

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
		_cameraComponent = Camera.GetComponent<Camera>();
	}

	public void Update()
	{
	    if (_player.isAlive && CrossPlatformInputManager.GetButtonDown("Fire2_"+_player.playerNumber))
		{
			ThrowSpear();
		}
	}

    void ThrowSpear()
    {
        var spear = Instantiate(_spearTemplate);
        spear.transform.position = transform.position + transform.right + transform.forward + transform.up;
		var aimPoint = _cameraComponent.ScreenToWorldPoint(new Vector3(_cameraComponent.pixelWidth, _cameraComponent.pixelHeight, 2) * 0.5f);
	    spear.transform.rotation = Quaternion.LookRotation((aimPoint - Camera.position).normalized) * Quaternion.Euler(-5, 0, 0);
        var rigidBody = spear.GetComponent<Rigidbody>();
        rigidBody.velocity = spear.transform.forward * 80;
    }
}
