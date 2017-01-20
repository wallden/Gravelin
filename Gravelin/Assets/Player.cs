using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Camera = UnityEngine.Camera;

public class Player : MonoBehaviour {

	// Use this for initialization
    Rigidbody rigidbody;
    public string playerNumber;
    public float speed = 100f;
	void Start ()
	{
	   rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        DoPlayerActions();
        DoPlayerMovement();
	   
	    
	}

    private void DoPlayerMovement()
    {
        if (CrossPlatformInputManager.GetButton("Horizontal_"+playerNumber))
        {
            var value = CrossPlatformInputManager.GetAxis("Horizontal_"+playerNumber);
            rigidbody.AddForce(transform.right * (value * speed));
        }
        if (CrossPlatformInputManager.GetButton("Vertical_"+playerNumber))
        {
            var value = CrossPlatformInputManager.GetAxis("Vertical_" + playerNumber);
            rigidbody.AddForce(transform.forward * (value * speed));

        }
    }

    private void DoPlayerActions()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit))
            {
                var hitSurface = hit.transform;
                Debug.Log(hit.point);
                var joint = hitSurface.transform.gameObject.AddComponent<ConfigurableJoint>();
                ConfigureJoint(joint);
                joint.connectedBody = transform.gameObject.GetComponent<Rigidbody>();
            }
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
