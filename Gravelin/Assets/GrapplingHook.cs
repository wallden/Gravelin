using System;
using Assets;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public bool Grappling;

    private Camera _camera;
    private GameObject _hookPointTemplate;
	private GameObject _hookLineSegmentTemplate;
	private Rigidbody _rigidBody;
	private Player _player;
	private bool _grappling;
	private TriggerButton _grappleButton;

	private float _dragValue;
	private Vector3 _hookOffset;
	private GameObject _hookRoot;

	public float MaxHookLength = 30;

	public void Start()
    {
	    _camera = GetComponentInChildren<Camera>();
        _hookPointTemplate = Resources.Load<GameObject>("HookPoint");
        if (_hookPointTemplate == null)
        {
            throw new Exception("Can't load hook point");
        }

        _hookLineSegmentTemplate = Resources.Load<GameObject>("HookLineSegment");
        if (_hookLineSegmentTemplate == null)
        {
            throw new Exception("Can't load hook line segment");
        }

		_hookOffset = new Vector3(0, 2.5f, 0);

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
            if (!Grappling)
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
	    var playerHookOrigin = transform.position + _hookOffset;
	    var reticleTarget = _camera.RayCastReticleTarget(playerHookOrigin);
		if (reticleTarget != null && reticleTarget.ToTarget.magnitude < MaxHookLength)
		{
			_hookRoot = new GameObject("HookRoot");
			var hookPoint = Instantiate(_hookPointTemplate);
            hookPoint.transform.position = reticleTarget.TargetPoint;
			hookPoint.transform.SetParent(_hookRoot.transform, true);

			var grapplingHookLength = (hookPoint.transform.position - playerHookOrigin).magnitude - 1;
			var lineLength = 2;
	        var segments = (int) (grapplingHookLength/lineLength);
			var remainingSegment = (grapplingHookLength/lineLength - segments);

			var lineRotation = Quaternion.LookRotation(playerHookOrigin - hookPoint.transform.position);
			var initialHookOffset = new Vector3(0, 0, 1) + _hookOffset;
	        var lastLinePosition = playerHookOrigin;
	        var lastBody = _rigidBody;
	        for (int i = 0; i < segments; i++)
	        {
				var newLine = Instantiate(_hookLineSegmentTemplate);
				newLine.transform.SetParent(_hookRoot.transform, true);
		        newLine.transform.position = lastLinePosition + lineRotation*Vector3.back*lineLength;
		        newLine.transform.rotation = lineRotation;
		        newLine.transform.localScale = new Vector3(1, 1, lineLength);

		        var lineJoint = newLine.gameObject.GetComponent<ConfigurableJoint>();
		        lineJoint.connectedAnchor = initialHookOffset;
		        lineJoint.connectedBody = lastBody;

		        lastBody = newLine.GetComponent<Rigidbody>();
		        lastLinePosition = newLine.transform.position;
				initialHookOffset = Vector3.zero;
	        }

			// Last remaining line connection
			var lastLine = Instantiate(_hookLineSegmentTemplate);
			lastLine.transform.SetParent(_hookRoot.transform, true);
			lastLine.transform.position = lastLinePosition + lineRotation * Vector3.back * remainingSegment;
			lastLine.transform.rotation = lineRotation;
			lastLine.transform.localScale = new Vector3(1, 1, remainingSegment);

			var lastLineJoint = lastLine.gameObject.GetComponent<ConfigurableJoint>();
			lastLineJoint.connectedBody = lastBody;

			lastBody = lastLine.GetComponent<Rigidbody>();

			hookPoint.GetComponent<ConfigurableJoint>().connectedBody = lastBody;

            Grappling = true;
            _rigidBody.drag = 0;
		}
	}

    public void CmdReleaseGrapple()
    {
        Destroy(_hookRoot);
        Grappling = false;
        _rigidBody.drag = _dragValue;
    }
}
