using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Slider hpBar;

    public int parentHP;
    public int parentMaxHP;

    EnemyFSM enemyFSM;
    DarkBlueFSM darkBlueFSM;
    RedDragonFSM redDragonFSM;

    void Start()
    {
        enemyFSM = transform.parent.GetComponent<EnemyFSM>();
        darkBlueFSM = transform.parent.GetComponent<DarkBlueFSM>();
        redDragonFSM = transform.parent.GetComponent<RedDragonFSM>();

        if (enemyFSM != null)
        {
            parentMaxHP = enemyFSM.maxHp;
        }
        else
        {
            if (darkBlueFSM != null)
            {
                parentMaxHP = darkBlueFSM.maxHp;
            }
            else
            {
                parentMaxHP = redDragonFSM.maxHp;
            }
        }
    }


    void Update()
    {
        if (enemyFSM != null)
        {
            parentHP = enemyFSM.hp;
        }
        else
        {
            if (darkBlueFSM != null)
            {
                parentHP = darkBlueFSM.hp;
            }
            else
            {
                parentHP = redDragonFSM.hp;
            }
        }

        hpBar.value = (float)parentHP/ (float)parentMaxHP;

        transform.LookAt(Camera.main.transform);
    }

}
