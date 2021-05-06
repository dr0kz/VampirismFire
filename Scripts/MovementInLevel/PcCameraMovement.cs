using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PcCameraMovement : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [SerializeField] private GameObject cameraLimiter_1;
    [SerializeField] private GameObject cameraLimiter_2;

    [SerializeField] private float cameraUpwardsLimiter;
    [SerializeField] private float cameraDownwardsLimiter;

    private const int sideOffsetBeforeMovement = 25; //pixels
    private const float speed = 10f;
    private void Start()
    {
        Cursor.visible = true;
    }
    private void Update()
    {
        MouseMovement();
        MouseZoomer();
        LimitCameraMovement();
    }
    private void MouseMovement()
    {
        if (Input.mousePosition.x < sideOffsetBeforeMovement) Camera.transform.Translate(Vector3.left * Time.deltaTime * speed, Space.Self);
        if (Input.mousePosition.y < sideOffsetBeforeMovement)
        {
            Vector3 vector = new Vector3(-1f, 0f, -0.6f);
            Camera.transform.Translate(vector * Time.deltaTime * speed, Space.World);
        }
        if (Input.mousePosition.x > Screen.width - sideOffsetBeforeMovement) Camera.transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
        if (Input.mousePosition.y > Screen.height - sideOffsetBeforeMovement)
        {
            Vector3 vector = new Vector3(1f, 0f, 0.6f);
            Camera.transform.Translate(vector * Time.deltaTime * speed, Space.World);
        }
    }
    private void MouseZoomer()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y + 1.0f, Camera.transform.position.z);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y - 1.0f, Camera.transform.position.z);
        }
    }
    private void LimitCameraMovement()
    {
        if (Camera.transform.position.y < cameraDownwardsLimiter)
            Camera.transform.position = new Vector3(Camera.transform.position.x, cameraDownwardsLimiter, Camera.transform.position.z);
        else if (Camera.transform.position.y > cameraUpwardsLimiter)
            Camera.transform.position = new Vector3(Camera.transform.position.x, cameraUpwardsLimiter, Camera.transform.position.z);

        if (Camera.transform.position.x < cameraLimiter_1.transform.position.x)
            Camera.transform.position = new Vector3(cameraLimiter_1.transform.position.x, Camera.transform.position.y, Camera.transform.position.z);
        if (Camera.transform.position.z < cameraLimiter_1.transform.position.z)
            Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y, cameraLimiter_1.transform.position.z);

        if (Camera.transform.position.x > cameraLimiter_2.transform.position.x)
            Camera.transform.position = new Vector3(cameraLimiter_2.transform.position.x, Camera.transform.position.y, Camera.transform.position.z);
        if (Camera.transform.position.z > cameraLimiter_2.transform.position.z)
            Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y, cameraLimiter_2.transform.position.z);
    }
}
