using System;
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
    public float xSpeed = 1.0f;
    public float ySpeed = 3.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;
    public float yOffset = 3f;
    public float xOffset = 3f;

    private Rigidbody rigidbody;
    private Player player;
    float x = 0.0f;
    float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();
        player = target.gameObject.GetComponent<Player>();
        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateCameraPosition()
    {
        if(!player)
            return;
        x += CrossPlatformInputManager.GetAxis("Mouse X_" + player.playerNumber) * xSpeed * 0.02f;
	    y -= CrossPlatformInputManager.GetAxis("Mouse Y_" + player.playerNumber) * ySpeed * 0.02f;

	    x += CrossPlatformInputManager.GetAxis("Joy X_" + player.playerNumber) * xSpeed * 0.6f;
		y += CrossPlatformInputManager.GetAxis("Joy Y_" + player.playerNumber) * ySpeed * 0.6f;

		y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        distance = Mathf.Clamp(desiredDistance - CrossPlatformInputManager.GetAxis("Mouse ScrollWheel_" + player.playerNumber) * 5, distanceMin, distanceMax);
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
        if (player.isAlive)
            target.transform.rotation = rotation;

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