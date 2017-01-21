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
        
            var horizontalValue = CrossPlatformInputManager.GetAxis("MoveHorizontal_" + playerNumber);
            rigidbody.AddForce(transform.right * (horizontalValue * speed));
        
            var verticalValue = -CrossPlatformInputManager.GetAxis("MoveVertical_" + playerNumber);
            rigidbody.AddForce(transform.forward * (verticalValue * speed));
     }
   
    ConfigurableJoint ConfigureJoint(ConfigurableJoint joint)
    {
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        return joint;
    }
}
