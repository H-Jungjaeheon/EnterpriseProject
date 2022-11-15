using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum CharacterKind
{
    Basic,
    SecondaryCharacter,
    CharacterCount
}

public class ProficiencySystemManager : Singleton<ProficiencySystemManager>
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
    private Text nowChooseCharacterUnlockButtonText;

    [SerializeField]
    [Tooltip("���� ������� ĳ���� �ڹ��� ������Ʈ")]
    private GameObject[] lockObj;

    private int minCharacterIndex;

    private int maxCharacterIndex;

    private int[] nowChooseCharacterUnlockCost = new int[(int)CharacterKind.CharacterCount];

    public bool[] isNowChooseCharacterUnlock = new bool[(int)CharacterKind.CharacterCount];

    public bool[] isNowChooseCharacterEquiping = new bool[(int)CharacterKind.CharacterCount];

    public int nowChooseCharacterIndex;

    private Vector3 dragStartMousePos;

    public bool isDraging;

    Color blackTextColor = new Color(0, 0, 0);

    Color redTextColor = new Color(1, 0, 0);

    Color greenTextColor = new Color(0, 1, 0.03f);

    private void Start()
    {
        isDraging = false;

        minCharacterIndex = 0;
        maxCharacterIndex = (int)CharacterKind.CharacterCount - 1;
        
        for (int nowIndex = minCharacterIndex; nowIndex <= maxCharacterIndex; nowIndex++)
        {
            nowChooseCharacterUnlockCost[nowIndex] = 4;
        }
        isNowChooseCharacterUnlock[minCharacterIndex] = true;
    }

    private void OnEnable()
    {
        TextReSettings();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isDraging == false)
        {
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

        isDraging = true;
        slideableBgRectTransform[nowChooseCharacterIndex].DOAnchorPos(new Vector2(imageTargetPosX, 0), 0.3f);

        if (isRightDrag && nowChooseCharacterIndex == minCharacterIndex || isRightDrag == false && nowChooseCharacterIndex == maxCharacterIndex)
        {
            StartCoroutine(ImagePosInitialization(-imageTargetPosX, isRightDrag));
            yield break;
        }
        else
        {
            nowChooseCharacterIndex = isRightDrag ? nowChooseCharacterIndex - 1 : nowChooseCharacterIndex + 1;
        }

        slideableBgRectTransform[nowChooseCharacterIndex].DOAnchorPos(new Vector2(0, 0), 0.3f);
        TextReSettings();
        yield return new WaitForSeconds(0.035f);
        isDraging = false;
    }

    IEnumerator ImagePosInitialization(int targetPos, bool isRightDrag)
    {
        for (int nowIndex = 0; nowIndex <= maxCharacterIndex; nowIndex++) //��ü �̹��� ��ġ �ʱ�ȭ(���� �̵����� �̹����� ������ �� ��ġ ����)
        {
            if (isRightDrag == false && nowIndex == maxCharacterIndex || isRightDrag && nowIndex == minCharacterIndex)
            {
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, true));
            }
            else
            {
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, false));
            }
        }

        nowChooseCharacterIndex = isRightDrag ? maxCharacterIndex : minCharacterIndex;

        slideableBgRectTransform[nowChooseCharacterIndex].DOAnchorPos(new Vector2(0, 0), 0.3f);
        TextReSettings();
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

    public void ProficiencyUnlockOrEquip()
    {
        if (isNowChooseCharacterUnlock[nowChooseCharacterIndex] == false && nowChooseCharacterUnlockCost[nowChooseCharacterIndex] <= GameManager.Instance.CurrentProficiency)
        {
            isNowChooseCharacterUnlock[nowChooseCharacterIndex] = true;

            lockObj[nowChooseCharacterIndex].SetActive(false);

            TextReSettings();
        }
        else if(isNowChooseCharacterUnlock[nowChooseCharacterIndex] && isNowChooseCharacterEquiping[nowChooseCharacterIndex] == false)
        {
            for (int nowIndex = minCharacterIndex; nowIndex <= maxCharacterIndex; nowIndex++)
            {
                if (nowIndex == nowChooseCharacterIndex)
                {
                    isNowChooseCharacterEquiping[nowIndex] = true;
                }
                else
                {
                    isNowChooseCharacterEquiping[nowIndex] = false;
                }
            }

            // ���Ⱑ ĳ���� �����ϴ� �κ�
            Player.Instance.CharacterChange(nowChooseCharacterIndex);

            TextReSettings();
        }
    }

    public void TextReSettings()
    {
        nowChooseCharacterNameText.text = $"{nowChooseCharacterName[nowChooseCharacterIndex]}";

        nowChooseCharacterBuffText.text = $"{nowChooseCharacterBuff[nowChooseCharacterIndex]}";

        if (isNowChooseCharacterUnlock[nowChooseCharacterIndex] == false)
        {
            nowChooseCharacterUnlockButtonText.color = (nowChooseCharacterUnlockCost[nowChooseCharacterIndex] <= GameManager.Instance.CurrentProficiency) ? greenTextColor : redTextColor;
            nowChooseCharacterUnlockButtonText.text = $"ĳ���� �������\n{nowChooseCharacterUnlockCost[nowChooseCharacterIndex]} ���õ� �ʿ�";
        }
        else
        {
            nowChooseCharacterUnlockButtonText.color = blackTextColor;
            if (isNowChooseCharacterEquiping[nowChooseCharacterIndex] == false)
            {
                nowChooseCharacterUnlockButtonText.text = $"���� ����";
            }
            else
            {
                nowChooseCharacterUnlockButtonText.text = $"������";
            }
        }
    }
}