using System;
using UnityEngine;
using UnityEngine.Networking;

public class JavelinThrow : MonoBehaviour
{
	public Transform Camera;

	private GameObject _spearTemplate;
    private Player _player;
    private TriggerButton _weaponButton;

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

        _weaponButton = new TriggerButton("Weapon_" + _player.playerNumber);
	}

	public void Update()
	{
	    if (_player.isAlive && _weaponButton.IsPressed())
		{
			ThrowSpear();
		}
	}

    void ThrowSpear()
    {
	    var spearSpawnOffset = transform.right + transform.forward + transform.up*2;
        var spear = Instantiate(_spearTemplate);
        spear.transform.position = transform.position + spearSpawnOffset;
		var aimPoint = Camera.transform.forward;
		spear.transform.rotation = Quaternion.LookRotation(aimPoint) * Quaternion.Euler(-2, 0, 0);
        var rigidBody = spear.GetComponent<Rigidbody>();
        rigidBody.velocity = spear.transform.forward * 80;
    }
}