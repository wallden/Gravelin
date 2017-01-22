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
    public float dashForce;

    public bool isAlive = true;
    public bool isAirborn;

    private double _dashCooldown;
    private MouseOrbitImproved _camera;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<MouseOrbitImproved>();
        var cam = GetComponentInChildren<Camera>();
        cam.enabled = true;
        speed = 0.6f;
        jumpForce = 100;
        dashForce = 100;
        rigidbody.mass = 10;
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
            rigidbody.AddForce(transform.right*(horizontalValue*(isAirborn ? speed/2 : speed)), ForceMode.VelocityChange);

            var forwardValue = -CrossPlatformInputManager.GetAxis("MoveVertical_" + playerNumber);
            rigidbody.AddForce(transform.forward*(forwardValue* (isAirborn ? (speed*0.1f) : speed)), ForceMode.VelocityChange);

            if (!isAirborn && horizontalValue == 0f && forwardValue == 0f)
            {
                rigidbody.velocity = Vector3.zero;
            }
        
        if (CrossPlatformInputManager.GetButtonDown("Jump_" + playerNumber))
        {
            if (!isAirborn)
            {
                rigidbody.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
                isAirborn = true;
            }
        }
        if (CrossPlatformInputManager.GetButtonDown("Dash_" + playerNumber))
        {
            if (isAirborn && _dashCooldown == 0f)
            {
                rigidbody.AddForce(transform.forward * dashForce, ForceMode.Impulse);
                _dashCooldown = 1f;
            }
        }
    }

    
    void OnCollisionEnter(Collision collision)
    {
        isAirborn = false;
        _dashCooldown = 0f;
    }
    ConfigurableJoint ConfigureJoint(ConfigurableJoint joint)
    {
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        return joint;
    }
}
