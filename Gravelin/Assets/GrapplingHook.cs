using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class GrapplingHook : NetworkBehaviour
{
	public Transform Camera;

	private GameObject _hookPointTemplate;
	private GameObject _hookLineTemplate;
	private Rigidbody _rigidBody;
	private bool _grappling;
	private GameObject _hookPoint;
	private GameObject _hookLine;

	public void Start()
	{
		_hookPointTemplate = Resources.Load<GameObject>("HookPoint");
		if (_hookPointTemplate == null)
		{
			throw new Exception("Can't load hook point");
		}
        _hookLineTemplate = Resources.Load<GameObject>("HookLine");
		if (_hookLineTemplate == null)
		{
			throw new Exception("Can't load hook line");
		}
		_rigidBody = GetComponent<Rigidbody>();
	}

	public void Update ()
	{
	    if (!isLocalPlayer)
	        return;
		CheckGrapple();
	}

	private void CheckGrapple()
	{
		if (CrossPlatformInputManager.GetButtonDown("Fire1"))
		{
			if (!_grappling)
			{
				CmdPerformNewGrapple();
			}
			else
			{
				CmdReleaseGrapple();
			}
		}
	}
    [Command]
    private void CmdPerformNewGrapple()
	{
		RaycastHit hit;
    	if (Physics.Raycast(transform.position, Camera.transform.forward, out hit))
		{
			_hookPoint = Instantiate(_hookPointTemplate);
			_hookPoint.transform.position = hit.point;
            _hookLine = Instantiate(_hookLineTemplate);
		    _hookLine.transform.position = hit.point;
            _hookLine.transform.rotation = Quaternion.LookRotation(transform.position - _hookLine.transform.position);
			_hookLine.transform.localScale = new Vector3(_hookLine.transform.localScale.x, _hookLine.transform.localScale.y, hit.distance);
		    _hookPoint.GetComponent<ConfigurableJoint>().connectedBody = _hookLine.GetComponent<Rigidbody>();
            var lineJoint = _hookLine.GetComponent<ConfigurableJoint>();
			lineJoint.connectedBody = _rigidBody;
			_grappling = true;
            NetworkServer.Spawn(_hookPoint);
            NetworkServer.Spawn(_hookLine);
    	}
	}
    private void CmdReleaseGrapple()
	{

        Destroy(_hookLine);
        Destroy(_hookPoint);
		_grappling = false;
	}
}
