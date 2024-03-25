using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IceRangeSkillUse : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;

    [SerializeField] Image toolTipImage;
    GameObject priorityImage;

    GameObject player;
    GameManager gameManager;

    float cooltime_current;
    float cooltime_max;
    [SerializeField] Image darkImage;
    [SerializeField] Text leftTimeText;
    bool canUseSkill;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        priorityImage = transform.parent.gameObject;
        cooltime_current = 10f;
        cooltime_max = 10f;
        darkImage.fillAmount = 0;
        canUseSkill = true;
        leftTimeText.text = "";
    }

    public void OnPointerClick(PointerEventData eData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;

            // ����Ŭ���� ����Ǵ� ����
            if (canUseSkill)
            {
                UseIceRange();
            }

        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;  // �ѹ� Ŭ���� �ð� ����
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // �̹����� hierarchy ��������ΰ��� ���̵���
        priorityImage = transform.parent.gameObject;
        priorityImage.transform.SetAsLastSibling();
        toolTipImage.gameObject.SetActive(true);

        gameManager.blockClick = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipImage.gameObject.SetActive(false);

        gameManager.blockClick = false;
    }

    public void UseIceRange()
    {
        if (canUseSkill && gameManager.playerMP >= 30)
        {
            gameManager.playerMP -= 30;
            player.GetComponent<PlayerMove>().InstantiateIceRange();

            darkImage.fillAmount = 1;
            StartCoroutine("CoolTime");

            cooltime_current = cooltime_max;
            leftTimeText.text = cooltime_current.ToString();
            StartCoroutine("CoolTimeCounter");

            canUseSkill = false;
        }
    }

    IEnumerator CoolTime()
    {
        while (darkImage.fillAmount > 0)
        {
            darkImage.fillAmount -= 1 * Time.smoothDeltaTime / cooltime_max;
            yield return null;
        }

        canUseSkill = true;
        yield break;
    }

    IEnumerator CoolTimeCounter()
    {
        while (cooltime_current > 0)
        {
            yield return new WaitForSeconds(1.0f);

            cooltime_current -= 1.0f;
            leftTimeText.text = "" + cooltime_current;
        }

        leftTimeText.text = "";
        yield break;
    }

}


