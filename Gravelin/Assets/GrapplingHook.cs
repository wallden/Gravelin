using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class GrapplingHook : MonoBehaviour
{
    private Transform _camera;
    private GameObject _hookPointTemplate;
    private GameObject _hookLineTemplate;
    private Rigidbody _rigidBody;
    private bool _grappling;
    private GameObject _hookPoint;
    private GameObject _hookLine;
 
    public void Start()
    {
        _camera = GetComponentInChildren<Camera>().transform;
        _hookPointTemplate = Resources.Load<GameObject>("HookOld");
        if (_hookPointTemplate == null)
        {
            throw new Exception("Can't load hook point");
        }
       
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
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
    private void CmdPerformNewGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, _camera.transform.forward, out hit))
        {
            _hookPoint = Instantiate(_hookPointTemplate);
            _hookPoint.transform.position = hit.point;
            var line = _hookPoint.transform.FindChild("Line");
            line.rotation = Quaternion.LookRotation(transform.position - line.transform.position);
            line.localScale = new Vector3(line.transform.localScale.x, line.transform.localScale.y, hit.distance);
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
            
        }
    }
    private void CmdReleaseGrapple()
    {
        Destroy(_hookLine);
        Destroy(_hookPoint);
        _grappling = false;
    }
}
