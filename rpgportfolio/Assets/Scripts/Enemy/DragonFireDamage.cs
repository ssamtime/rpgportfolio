using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireDamage : MonoBehaviour
{
    GameObject player;
    GameManager gameManager;

    ParticleSystem fireParticleSystem;
    PlayerMove playerMoveScript;
    float attackDelay;
    float elapsedTime;
    int attackPower;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMoveScript = player.GetComponent<PlayerMove>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        attackDelay = 0.02f;
        elapsedTime = 0.02f;
        attackPower = 10;

        fireParticleSystem = gameObject.GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.GetComponent<PlayerMove>()!=null)
        {
            // 0.02초마다 데미지
            elapsedTime += Time.deltaTime;
            if (elapsedTime > attackDelay)
            {
                elapsedTime = 0;                
                if (playerMoveScript != null)
                {
                    if (attackPower - gameManager.armorPower >= 1)
                    {
                        playerMoveScript.DamageAction(attackPower - gameManager.armorPower);
                    }
                    else
                    {
                        playerMoveScript.DamageAction(1);
                    }
                }
            }
        }      
    }

}
