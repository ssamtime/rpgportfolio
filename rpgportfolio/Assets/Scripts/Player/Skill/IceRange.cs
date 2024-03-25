using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRange : MonoBehaviour
{
    int IceRangeDamage;
    float timer;

    void Start()
    {
        IceRangeDamage = 20;

        Destroy(gameObject, 4f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1)
        {
            timer = 0;

            // 적 탐지
            RaycastHit[] hits;
            // 플레이어 위로 구 발사
            hits = Physics.SphereCastAll(transform.position, 4.3f, transform.up, 1f);

            foreach (RaycastHit colliderHit in hits)
            {                
                EnemyFSM enemyFSM1 = colliderHit.collider.GetComponent<EnemyFSM>();
                RedDragonFSM enemyFSM2 = colliderHit.collider.GetComponent<RedDragonFSM>();
                DarkBlueFSM enemyFSM3 = colliderHit.collider.GetComponent<DarkBlueFSM>();

                if (enemyFSM1 != null)
                    enemyFSM1.HitEnemy(IceRangeDamage);
                else if (enemyFSM2 != null)
                    enemyFSM2.HitEnemy(IceRangeDamage);
                else if (enemyFSM3 != null)
                    enemyFSM3.HitEnemy(IceRangeDamage);

                //if (colliderHit.collider.tag == "Enemy")
                //{
                //    GameObject hitEffect = Instantiate<GameObject>(DamageEffectPrefab,
                //        colliderHit.collider.transform.position + new Vector3(0, 1.1f, 0), Quaternion.identity);
                //    Destroy(hitEffect, 2f);
                //}
            }

        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Enemy")
    //    {
                                       
    //    }
    //}
}
