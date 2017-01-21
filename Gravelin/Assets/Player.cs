using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using Camera = UnityEngine.Camera;

public class Player : NetworkBehaviour
{

    // Use this for initialization
    Rigidbody rigidbody;
    public string playerNumber;
    public float speed = 100f;

    [HideInInspector]
    public bool isAlive = true;

    private MouseOrbitImproved _camera;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<MouseOrbitImproved>();
        if (isLocalPlayer)
        {
            _camera.GetComponent<Camera>().enabled = true;
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isLocalPlayer)
            return;
        _camera.UpdateCameraPosition();
    }
    void Update()
    {
        if (!isLocalPlayer)
            return;
        DoPlayerMovement();

    }

    private void DoPlayerMovement()
    {
        if (CrossPlatformInputManager.GetButton("Horizontal_" + playerNumber))
        {
            var value = CrossPlatformInputManager.GetAxis("Horizontal_" + playerNumber);
            rigidbody.AddForce(transform.right * (value * speed));
        }
        if (CrossPlatformInputManager.GetButton("Vertical_" + playerNumber))
        {
            var value = CrossPlatformInputManager.GetAxis("Vertical_" + playerNumber);
            rigidbody.AddForce(transform.forward * (value * speed));

        }
    }
    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
    ConfigurableJoint ConfigureJoint(ConfigurableJoint joint)
    {
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        return joint;
    }
}
