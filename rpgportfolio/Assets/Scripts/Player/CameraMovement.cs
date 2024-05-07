using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform objectTofollow;
    public float sensitivity = 100f;
    public float maxClampAngle;
    public float minClampAngle;
    public float rotX;
    private float rotY;
    public Transform realCamera;
    public Vector3 dirNormalized;
    public Vector3 finalPos;
    public float minDistance;
    public float maxDistance;
    public float finalDistance;
    public float smoothness = 10f;

    GameManager gameManager;
    GameObject player;

    void Start()
    {
        // 변수 초기화
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;
        dirNormalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;

        minDistance = 1f;
        maxDistance = 10f;
        minClampAngle = 0f;
        maxClampAngle = 70f;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player");

    }
    private void Awake()
    {
        var obj = FindObjectsOfType<GameManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 마우스 움직임에 따라 카메라 회전
        if (gameManager.canScreenRotate)
        {
            rotX += -1 * Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        }
        rotX = Mathf.Clamp(rotX, minClampAngle, maxClampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        // 메인카메라를 자식으로 가지고 있는 오브젝트에 달려있는 스크립트
        transform.rotation = rot;
    }
    private void LateUpdate()
    {
        // objectTofollow는 캐릭터 중심에 있는 빈게임오브젝트
        transform.position = objectTofollow.position;

        // dirNormalized는 메인카메라와 부모오브젝트의 노말벡터
        // 방향 벡터를 부모 오브젝트의 회전으로 변환
        Vector3 worldDirNormalized = transform.rotation * dirNormalized;

        // 메인카메라 부모의 위치 + worldDirNormalized * 최대거리
        finalPos = transform.position + worldDirNormalized * maxDistance;
        // finalPos = transform.TransformPoint(dirNormalized * maxDistance);
        // 이렇게도 가능

        RaycastHit hit;
        // 메인카메라의 부모와 finalPos 사이에 오브젝트가 감지되면
        if (Physics.Linecast(transform.position, finalPos, out hit))
            // finalDistance을 레이와 충돌한 위치 사이의 거리로 설정
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        else
            finalDistance = maxDistance;
        // 메인카메라 위치를 캐릭터와 finalDistance의 중간지점으로 이동
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition,
            dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }

}
