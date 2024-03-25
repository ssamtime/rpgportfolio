using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NumberKeyUse : MonoBehaviour
{
    [SerializeField] Image skillImage1;
    [SerializeField] Image skillImage2;
    [SerializeField] Image skillImage3;
    [SerializeField] Image skillImage4;

    Transform fireBallObjectTransform;
    Transform iceRangeObjectTransform;
    Transform potionObjectTransform;

    void Start()
    {        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (skillImage1.transform.childCount == 2)
            {
                Use(skillImage1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (skillImage2.transform.childCount == 2)
            {
                Use(skillImage2);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (skillImage3.transform.childCount == 2)
            {
                Use(skillImage3);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (skillImage4.transform.childCount == 2)
            {
                Use(skillImage4);
            }
        }
    }

    void Use(Image image)
    {
        if(fireBallObjectTransform = image.transform.Find("FireBallSkill"))
        {
            fireBallObjectTransform.GetComponent<FireBallSkillUse>().UseFireBall();
        }
        else if(iceRangeObjectTransform = image.transform.Find("IceRangeSkill"))
        {
            iceRangeObjectTransform.GetComponent <IceRangeSkillUse>().UseIceRange();
        }
        else if( (potionObjectTransform = image.transform.Find("Image_RedPotion(Clone)")) ||
            (potionObjectTransform = image.transform.Find("Image_BluePotion(Clone)"))||
            (potionObjectTransform = image.transform.Find("Image_Elixir(Clone)"))
            )
        {
            potionObjectTransform.GetComponent<PotionUse>().UsePotion();
        }        
            
    }
}
