using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneNPC : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Outline outlineScript;
    public Image shopImage;
    GameManager gameManager;
    GameObject player;           // �÷��̾�  
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
        if (gameManager.sceneNPCturn)
        {
            // �÷��̾� �ٶ󺸱�
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ������ ����
        outlineScript.enabled = !outlineScript.enabled;
        // npcŬ���Ҷ� ����x
        gameManager.blockClick = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outlineScript.enabled = !outlineScript.enabled;
        //gameManager.blockClick = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // scene �̵�â ��������
        shopImage.gameObject.SetActive(true);
        gameManager.sceneNPCturn = true;

        gameManager.blockClick = true;
        gameManager.canScreenRotate = false;
        audioSource.PlayOneShot(npcTalkAC);
    }
}
