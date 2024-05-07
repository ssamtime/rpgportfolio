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
        // ���� �ʱ�ȭ
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
        // ���콺 �����ӿ� ���� ī�޶� ȸ��
        if (gameManager.canScreenRotate)
        {
            rotX += -1 * Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        }
        rotX = Mathf.Clamp(rotX, minClampAngle, maxClampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        // ����ī�޶� �ڽ����� ������ �ִ� ������Ʈ�� �޷��ִ� ��ũ��Ʈ
        transform.rotation = rot;
    }
    private void LateUpdate()
    {
        // objectTofollow�� ĳ���� �߽ɿ� �ִ� ����ӿ�����Ʈ
        transform.position = objectTofollow.position;

        // dirNormalized�� ����ī�޶�� �θ������Ʈ�� �븻����
        // ���� ���͸� �θ� ������Ʈ�� ȸ������ ��ȯ
        Vector3 worldDirNormalized = transform.rotation * dirNormalized;

        // ����ī�޶� �θ��� ��ġ + worldDirNormalized * �ִ�Ÿ�
        finalPos = transform.position + worldDirNormalized * maxDistance;
        // finalPos = transform.TransformPoint(dirNormalized * maxDistance);
        // �̷��Ե� ����

        RaycastHit hit;
        // ����ī�޶��� �θ�� finalPos ���̿� ������Ʈ�� �����Ǹ�
        if (Physics.Linecast(transform.position, finalPos, out hit))
            // finalDistance�� ���̿� �浹�� ��ġ ������ �Ÿ��� ����
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        else
            finalDistance = maxDistance;
        // ����ī�޶� ��ġ�� ĳ���Ϳ� finalDistance�� �߰��������� �̵�
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition,
            dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }

}
