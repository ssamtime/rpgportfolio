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
        // 항상 카메라의 방향과 일치시킨다
        transform.forward = target.forward;
    }
}
