using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcManager : MonoBehaviour
{
    [SerializeField] int orcHp;
    // Start is called before the first frame update
    void Start()
    {
        orcHp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        orcHp = Mathf.Max(orcHp-damage, 0);
        print(orcHp);
    }
}
