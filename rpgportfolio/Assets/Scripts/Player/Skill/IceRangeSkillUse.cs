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

            // 더블클릭시 실행되는 내용
            if (canUseSkill)
            {
                UseIceRange();
            }

        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;  // 한번 클릭시 시간 저장
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 이미지가 hierarchy 가장밑으로가서 보이도록
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

            darkImage.fillAmount = 1;   //스킬 버튼을 가림
            StartCoroutine("CoolTime");

            cooltime_current = cooltime_max;
            leftTimeText.text = cooltime_current.ToString();
            StartCoroutine("CoolTimeCounter");

            canUseSkill = false;    //스킬을 사용할 수 없는 상태로 바꿈
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


