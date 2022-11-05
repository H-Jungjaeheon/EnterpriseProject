using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProficiencySystemManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("���� ������ ĳ���� �̸�")]
    private string[] nowChooseCharacterName;

    [SerializeField]
    [Tooltip("���� ������ ĳ���� ���� ����")]
    private string[] nowChooseCharacterBuff;

    [SerializeField]
    [Tooltip("�����̵��� ���(ĳ����) ��Ʈ Ʈ������")]
    private RectTransform[] slideableBgRectTransform;

    [SerializeField]
    [Tooltip("���� ĳ���� �̸� �ؽ�Ʈ")]
    private Text nowChooseCharacterNameText;

    [SerializeField]
    [Tooltip("���� ĳ���� ���� ���� �ؽ�Ʈ")]
    private Text nowChooseCharacterBuffText;
    [SerializeField]

    [Tooltip("���� ĳ���� ���׷��̵�/������� ��ư �ؽ�Ʈ")]
    private Text nowChooseCharacterUpgradeOrUnlockButtonText;

    private int minCharacterIndex;

    private int maxCharacterIndex;

    private int[] nowChooseCharacterUnlockCost;

    private int[] nowChooseCharacterUpgradeCost;

    private bool[] isNowChooseCharacterUnlock;

    private int[] nowChooseCharacterLevel;

    public int nowChooseCharacterIndex;

    private Vector3 dragStartMousePos;

    private bool isDraging;

    private void Start()
    {
        minCharacterIndex = 0;
        maxCharacterIndex = nowChooseCharacterName.Length - 1;

        nowChooseCharacterUnlockCost = new int[nowChooseCharacterName.Length];
        nowChooseCharacterUpgradeCost = new int[nowChooseCharacterName.Length];
        isNowChooseCharacterUnlock = new bool[nowChooseCharacterName.Length];
        nowChooseCharacterLevel = new int[nowChooseCharacterName.Length];
    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isDraging == false)
        {
            isDraging = true;
            StartCoroutine(DragStart());
        }
    }

    IEnumerator DragStart()
    {
        Vector3 nowMousePos;

        dragStartMousePos = Input.mousePosition;

        while (true)
        {
            nowMousePos = Input.mousePosition;

            if (dragStartMousePos.x + 100 < nowMousePos.x)
            {
                StartCoroutine(Draging(true));
                break;
            }
            else if (dragStartMousePos.x - 100 > nowMousePos.x)
            {
                StartCoroutine(Draging(false));
                break;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDraging = false;
                break;
            }
            yield return null;
        }
    }

    IEnumerator Draging(bool isRightDrag)
    {
        int imageTargetPosX;

        imageTargetPosX = isRightDrag ? 1300 : -1300;

        slideableBgRectTransform[nowChooseCharacterIndex].DOAnchorPos(new Vector2(imageTargetPosX, 0), 0.35f);

        if (isRightDrag)
        {
            if (nowChooseCharacterIndex - 1 < minCharacterIndex)
            {
                StartCoroutine(ImagePosInitialization(-imageTargetPosX, isRightDrag));
            }
            else
            {
                nowChooseCharacterIndex--;
            }
        }
        else
        {
            if (nowChooseCharacterIndex + 1 > maxCharacterIndex)
            {
                StartCoroutine(ImagePosInitialization(-imageTargetPosX, isRightDrag));
            }
            else
            {
                nowChooseCharacterIndex++;
            }
        }
        slideableBgRectTransform[nowChooseCharacterIndex].DOAnchorPos(new Vector2(0, 0), 0.35f);
        yield return new WaitForSeconds(0.4f);
        isDraging = false;
    }

    IEnumerator ImagePosInitialization(int targetPos, bool isRightDrag)
    {
        WaitForSeconds imagerelocationDelay = new WaitForSeconds(0.4f);
        bool isTheEndIndex = false;

        for (int nowIndex = 0; nowIndex <= maxCharacterIndex; nowIndex++)
        {
            if (nowIndex == maxCharacterIndex)
            {
                isTheEndIndex = true;
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, true));
            }
            else
            {
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, false));
            }
        }

        if (isRightDrag)
        {
            nowChooseCharacterIndex = maxCharacterIndex;
        }
        else
        {
            nowChooseCharacterIndex = minCharacterIndex;
        }

        slideableBgRectTransform[nowChooseCharacterIndex].DOAnchorPos(new Vector2(0, 0), 0.35f);
        if (isTheEndIndex == false)
        {
            yield return imagerelocationDelay;
            isDraging = false;
        }

        yield return null;
    }  

    IEnumerator ImagerelocationDelay(Vector2 targetPos, int moveImageIndex, bool isGiveDelay)
    {
        if (isGiveDelay)
        {
            yield return new WaitForSeconds(0.35f);
            isDraging = false;
        }
        slideableBgRectTransform[moveImageIndex].DOAnchorPos(targetPos, 0);
    }
}
