using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float desiredDistance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;
    public float yOffset = 3f;
    public float xOffset = 3f;

    private Rigidbody _rigidbody;
    private Player _player;
    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        _rigidbody = GetComponent<Rigidbody>();
        _player = target.gameObject.GetComponent<Player>();
        if (_rigidbody != null)
        {
            _rigidbody.freezeRotation = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }
    public void UpdateCameraPosition()
    {
        if(!_player)
            return;
        x += (CrossPlatformInputManager.GetAxis("LookHorizontal_" + _player.playerNumber) + CrossPlatformInputManager.GetAxis("Debug_LookHorizontal_1"))
			* xSpeed * distance * 0.02f;
        y += (CrossPlatformInputManager.GetAxis("LookVertical_" + _player.playerNumber) - CrossPlatformInputManager.GetAxis("Debug_LookVertical_1"))
			* ySpeed * 0.02f;

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        //distance = Mathf.Clamp(desiredDistance - CrossPlatformInputManager.GetAxis("Mouse ScrollWheel_" + player.playerNumber) * 5, distanceMin, distanceMax);
        distance = 5f;
        desiredDistance = distance;
        RaycastHit hit;
        var offsetPosition = (transform.rotation * new Vector3(xOffset, yOffset)) + target.position;
        if (Physics.Raycast(offsetPosition, transform.position - offsetPosition, out hit, desiredDistance))
        {
            distance = hit.distance - 1;
        }
        Vector3 negDistance = new Vector3(0, 0, -distance);
        Vector3 position = rotation * negDistance + offsetPosition;

        transform.rotation = rotation;
        transform.position = position;
        if (_player.isAlive)
        {
            _player.transform.rotation = Quaternion.Euler(0, x, 0);
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}