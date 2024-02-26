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

    // �ָԳ������� ����Ǵ� �ִϸ��̼� �̺�Ʈ
    public void PunchDamageEvent()
    {
        // �� Ž��
        RaycastHit hit2;
        // �÷��̾� �ָ���ġ���� ���� �߻�
        if (Physics.Raycast(transform.position + (transform.forward * 0.2f) + new Vector3(0, 1.5f, 0), transform.forward, out hit2, punchRange))
        {
            OrcManager enemy = hit2.collider.GetComponent<OrcManager>();
            if (enemy != null)
            {
                transform.LookAt(hit2.transform);
                enemy.TakeDamage(punchDamage); // ������ ���� ������
            }
        }
    }
}
