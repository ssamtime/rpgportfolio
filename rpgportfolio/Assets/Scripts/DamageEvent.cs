using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : MonoBehaviour
{
    public float punchRange;
    public int punchDamage;

    // Start is called before the first frame update
    void Start()
    {
        punchRange = 0.5f;
        punchDamage = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 주먹내지를때 실행되는 애니메이션 이벤트
    public void PunchDamageEvent()
    {
        // 적 탐지
        RaycastHit hit2;
        // 플레이어 주먹위치에서 레이 발사
        if (Physics.Raycast(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0), transform.forward, out hit2, punchRange))
        {
            OrcManager enemy = hit2.collider.GetComponent<OrcManager>();
            if (enemy != null)
            {
                transform.LookAt(hit2.transform);
                enemy.TakeDamage(punchDamage); // 적에게 피해 입히기
            }
        }
    }
}
