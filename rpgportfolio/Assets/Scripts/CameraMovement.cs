using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform objectTofollow;
    public float followspeed =10f;
    public float sensitivity = 100f;
    public float clampAngle = 70f;
    private float rotX;
    private float rotY;
    public Transform realCamera;
    public Vector3 dirNormalized;
    public Vector3 finalDir;
    public float minDistance;
    public float maxDistance;
    public float finalDistance;
    public float smoothness = 10f;

    private Vector3 cameraPos = new Vector3(0, 3, -3);

    void Start()
    {
        // 변수 초기화
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;
        dirNormalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;

        // 커서 안보이게
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }


    void Update()
    {
        rotX += -1 * Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }
    private void LateUpdate()
    {
        //transform.position = Vector3.MoveTowards(transform.position,
        //objectTofollow.position, followspeed * Time.deltaTime);
        transform.position = objectTofollow.position;
        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        RaycastHit hit;

        if(Physics.Linecast(transform.position,finalDir,out hit))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDistance = maxDistance;
        }

        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition,
            dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }

}
