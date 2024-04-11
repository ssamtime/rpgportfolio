using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Vector3 playerFoward;
    float fireBallSpeed;
    int fireBallDamage;

    [SerializeField]  GameObject explosionEffectPrefab; //expolsion A


    void Start()
    {
        playerFoward = GameObject.FindWithTag("Player").transform.forward;
        fireBallSpeed = 8f;
        fireBallDamage = 30;

        Destroy(gameObject,10f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(playerFoward * fireBallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyFSM enemyFSM1 = other.GetComponent<EnemyFSM>();
            RedDragonFSM enemyFSM2 = other.GetComponent<RedDragonFSM>();
            DarkBlueFSM enemyFSM3 = other.GetComponent<DarkBlueFSM>();

            if (enemyFSM1 != null)
                enemyFSM1.HitEnemy(fireBallDamage);
            else if(enemyFSM2 != null)
                enemyFSM2.HitEnemy(fireBallDamage);
            else if(enemyFSM3!=null)
                enemyFSM3.HitEnemy(fireBallDamage);

            // Æø¹ß ÆÄÆ¼Å¬ instantiate
            GameObject explosionFX = Instantiate<GameObject>(explosionEffectPrefab,
                other.gameObject.transform.position +new Vector3(0,1.1f,0),
                Quaternion.identity);
            Destroy(explosionFX, 3f);
            Destroy(gameObject);
        }

    }

}
