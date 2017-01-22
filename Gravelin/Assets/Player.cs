using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using Camera = UnityEngine.Camera;

public class Player : MonoBehaviour
{

    // Use this for initialization
    Rigidbody rigidbody;
    public string playerNumber;
    public float speed;
    public float jumpForce;
    [HideInInspector]
    public bool isAlive = true;

    public bool isAirborn = false;

    private MouseOrbitImproved _camera;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<MouseOrbitImproved>();
        var cam = GetComponentInChildren<Camera>();
        cam.enabled = true;
        GetComponent<MeshRenderer>().material.color = Color.blue;
        speed = 20f;
        jumpForce = 9f;
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

        var horizontalValue = CrossPlatformInputManager.GetAxis("MoveHorizontal_" + playerNumber);
        rigidbody.AddForce(transform.right * (horizontalValue * speed));

        var forwardValue = -CrossPlatformInputManager.GetAxis("MoveVertical_" + playerNumber);
        rigidbody.AddForce(transform.forward * (forwardValue * speed));

        if (CrossPlatformInputManager.GetButtonDown("Jump_" + playerNumber))
        {
             rigidbody.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
            isAirborn = true;
        }
        if (!isAirborn && horizontalValue == 0f && forwardValue == 0f)
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        isAirborn = false;
    }
    ConfigurableJoint ConfigureJoint(ConfigurableJoint joint)
    {
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        return joint;
    }
}
