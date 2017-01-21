using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using Camera = UnityEngine.Camera;

public class Player : MonoBehaviour
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
           var cam = GetComponentInChildren<Camera>();
            cam.enabled = true;
        GetComponent<MeshRenderer>().material.color = Color.blue;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        _camera.UpdateCameraPosition();
    }
    void Update()
    {
        DoPlayerMovement();

    }

    private void DoPlayerMovement()
    {
        if (!isAlive)
            return;
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
   
    ConfigurableJoint ConfigureJoint(ConfigurableJoint joint)
    {
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        return joint;
    }
}
