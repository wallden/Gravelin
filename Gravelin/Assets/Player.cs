using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using Camera = UnityEngine.Camera;

public class Player : MonoBehaviour
{

    // Use this for initialization
    Rigidbody rigidbody;
    public int playerNumber;
    public float speed;
    public float maxX;
    public float maxY;
    public float maxZ;
    public float jumpForce;
    public float dashForce;

    public bool isAlive = true;
    public bool isAirborn;
    public bool usedDoubleJump;

    private double _dashCooldown;
    private MouseOrbitImproved _camera;
    private GrapplingHook _grapplingHook;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        _grapplingHook = GetComponent<GrapplingHook>();
        _camera = GetComponentInChildren<MouseOrbitImproved>();
        var cam = GetComponentInChildren<Camera>();
        cam.enabled = true;
        speed = 0.6f;
        jumpForce = 100;
        dashForce = 170;
        rigidbody.mass = 10;
        maxX = 10;
        maxY = 20;
        maxZ = 10;
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
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("main");
        }
        if (!isAlive)
            return;


        var horizontalValue = CrossPlatformInputManager.GetAxis("MoveHorizontal_" + playerNumber);
        rigidbody.AddForce(transform.right * (horizontalValue * (isAirborn ? speed / 2 : speed)), ForceMode.VelocityChange);

        var forwardValue = -CrossPlatformInputManager.GetAxis("MoveVertical_" + playerNumber);
        rigidbody.AddForce(transform.forward * (forwardValue * (isAirborn ? (speed * 0.1f) : speed)), ForceMode.VelocityChange);

        if (!isAirborn && horizontalValue == 0f && forwardValue == 0f)
        {
            rigidbody.velocity = Vector3.zero;
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump_" + playerNumber))
        {
            if (!isAirborn){
               
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isAirborn = true;
            }
            if (isAirborn && !usedDoubleJump){
               
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isAirborn = true;
                usedDoubleJump = true;
            }
            if (_grapplingHook.Grappling && isAirborn)
            {
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _grapplingHook.CmdReleaseGrapple();
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
        if (!isAirborn && _dashCooldown == 0f)
        {
            rigidbody.velocity = new Vector3(Mathf.Clamp(rigidbody.velocity.x, -maxX, maxX),
                Mathf.Clamp(rigidbody.velocity.y, -maxY, maxY),
                Mathf.Clamp(rigidbody.velocity.z, -maxZ, maxZ));
        }


    }


    void OnCollisionEnter(Collision collision)
    {

        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5)
            {
                if (isAirborn)
                {
                    usedDoubleJump = false;
                    isAirborn = false;
                }

                _dashCooldown = 0f;
                
            }
        }

    }
}
