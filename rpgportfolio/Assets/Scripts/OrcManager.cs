using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcManager : MonoBehaviour
{
    [SerializeField] int orcHp;

    private Animator orcAnimator;
    bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        orcHp = 30;
        orcAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(orcHp <= 0 && !isDead)
        {
            isDead = true;
            orcAnimator.SetTrigger("Die");
        }
    }

    public void TakeDamage(int damage)
    {
        orcHp = Mathf.Max(orcHp-damage, 0);
        print(orcHp);
    }
}
