using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    [SerializeField] GameObject rotateObject;

    [SerializeField] Transform target;

    void Start()
    {
    }


    void Update()
    {
        //rotateObject.transform.LookAt(target.transform.position);
        // �׻� ī�޶��� ����� ��ġ��Ų��
        transform.forward = target.forward;
    }
}
