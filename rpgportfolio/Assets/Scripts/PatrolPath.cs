using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{

    void Start()
    {        
    }

    void Update()
    {        
    }

    private void OnDrawGizmos()
    {
        for (int i = 0;i<transform.childCount;i++)
        {
            int j = i + 1;
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.GetChild(i).position, 0.3f);
            if (j >= transform.childCount) return;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(j).position);
        }
    }

}
