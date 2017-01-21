using System;
using Assets;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GrapplingHook : MonoBehaviour
{
    private Camera _camera;
    private GameObject _hookPointTemplate;
    private Rigidbody _rigidBody;
    private Player _player;
    private bool _grappling;
    private GameObject _hookPoint;
    private TriggerButton _grappleButton;

    private float _dragValue;

    public void Start()
    {
	    _camera = GetComponentInChildren<Camera>();
        _hookPointTemplate = Resources.Load<GameObject>("HookOld");
        if (_hookPointTemplate == null)
        {
            throw new Exception("Can't load hook point");
        }
       
        _rigidBody = GetComponent<Rigidbody>();
        _dragValue = _rigidBody.drag;
        _player = GetComponent<Player>();

        _grappleButton = new TriggerButton("Grapple_" + _player.playerNumber);
    }

    public void Update()
    {
        CheckGrapple();
    }

    private void CheckGrapple()
    {
        if (_player.isAlive && _grappleButton.IsPressed())
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
    private void CmdPerformNewGrapple()
    {
		var reticleTarget = _camera.RayCastReticleTarget(transform.position);
		if (reticleTarget != null)
        {
            _hookPoint = Instantiate(_hookPointTemplate);
            _hookPoint.transform.position = reticleTarget.TargetPoint;
            var line = _hookPoint.transform.FindChild("Line");
            line.rotation = Quaternion.LookRotation(transform.position - line.transform.position);
            line.localScale = new Vector3(line.transform.localScale.x, line.transform.localScale.y, (_hookPoint.transform.position - transform.position).magnitude - 1);
            var myJoint = line.gameObject.AddComponent<ConfigurableJoint>();
            myJoint.autoConfigureConnectedAnchor = false;
            myJoint.anchor = new Vector3(0, 0, 1);
            myJoint.connectedAnchor = new Vector3(0, 0, 1);
            myJoint.axis = new Vector3(1, 0, 0);
            myJoint.xMotion = ConfigurableJointMotion.Locked;
            myJoint.yMotion = ConfigurableJointMotion.Locked;
            myJoint.zMotion = ConfigurableJointMotion.Locked;
            myJoint.angularXMotion = ConfigurableJointMotion.Free;
            myJoint.angularYMotion = ConfigurableJointMotion.Free;
            myJoint.angularZMotion = ConfigurableJointMotion.Free;
            myJoint.connectedBody = _rigidBody;

            _grappling = true;
            _rigidBody.drag = 0;

        }
    }

    private void CmdReleaseGrapple()
    {
        Destroy(_hookPoint);
        _grappling = false;
        _rigidBody.drag = _dragValue;
    }
}
