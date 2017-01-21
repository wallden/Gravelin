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
           var cam = GetComponentInChildren<Camera>();
            cam.enabled = true;
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
            var horizontal = CrossPlatformInputManager.GetAxis("Horizontal_" + playerNumber) + CrossPlatformInputManager.GetAxis("Joy Horizontal");
            rigidbody.AddForce(transform.right * (horizontal * speed));
            var vertical = CrossPlatformInputManager.GetAxis("Vertical_" + playerNumber) - CrossPlatformInputManager.GetAxis("Joy Vertical");
            rigidbody.AddForce(transform.forward * (vertical * speed));
    }
    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}