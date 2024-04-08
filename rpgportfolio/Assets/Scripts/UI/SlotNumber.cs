using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotNumber : MonoBehaviour
{
    public int slotNumber = -1;

    void Start()
    {
        // �����Ǹ� parent ���� �ѹ��� ������
        if(transform.parent.GetComponent<SlotInfo>()!=null)
            slotNumber = transform.parent.GetComponent<SlotInfo>().slotNumber;
        else if(transform.parent.GetComponent<QuickSlotInfo>() != null)
            slotNumber = transform.parent.GetComponent<QuickSlotInfo>().slotNumber;

    }

}
