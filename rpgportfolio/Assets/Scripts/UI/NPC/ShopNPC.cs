using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopNPC : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Outline outlineScript;
    public Image shopImage;
    GameManager gameManager;
    GameObject player;           // 플레이어   
    AudioSource audioSource;
    [SerializeField] AudioClip npcTalkAC;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        outlineScript = GetComponent<Outline>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = transform.GetComponent<AudioSource>();
    }


    void Update()
    {        
        if(gameManager.merchantNPCturn)
        {
            // 플레이어 바라보기
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 윤곽선 생성
        outlineScript.enabled = !outlineScript.enabled;
        // npc클릭할때 공격x
        gameManager.blockClick = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outlineScript.enabled = !outlineScript.enabled;
        //gameManager.blockClick = false;
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        // 상점창 켜지도록
        shopImage.gameObject.SetActive(true);
        gameManager.merchantNPCturn = true;

        gameManager.blockClick = true;
        gameManager.canScreenRotate = false;
        audioSource.PlayOneShot(npcTalkAC,0.6f);
    }
}
