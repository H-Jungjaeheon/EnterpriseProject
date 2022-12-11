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

public enum TextColorKind
{
    Black,
    Red,
    Green
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
    private Text nameText;

    [SerializeField]
    [Tooltip("���� ĳ���� ���� ���� �ؽ�Ʈ")]
    private Text buffText;

    [SerializeField]
    [Tooltip("���� ĳ���� ����/������� ��ư �ȳ� �ؽ�Ʈ")]
    private Text guideText;

    [SerializeField]
    [Tooltip("���� ĳ���� ����/������� ��ư ���� �ؽ�Ʈ")]
    private Text priceText;

    [SerializeField]
    [Tooltip("���� ������� ĳ���� �ڹ��� ������Ʈ")]
    private GameObject[] lockObj;

    [SerializeField]
    [Tooltip("���׷��̵�/������� ��ư ��ȭ �̹���")]
    private Image goodsImage;

    [SerializeField]
    [Tooltip("�ؽ�Ʈ �� ����")]
    private Color[] textColors;

    private int minIndex; //�ּ� ������ �ε���

    private int maxIndex; //�ִ� ������ �ε���

    public int nowIndex; //���� ������ �ε���

    private int[] unlockCost = new int[(int)CharacterKind.CharacterCount]; //���� �������� ĳ���� ��� ���� ����

    public bool[] isUnlock = new bool[(int)CharacterKind.CharacterCount]; //���� �������� ĳ���� ��� ���� ���� �Ǻ�

    public bool[] isEquiping = new bool[(int)CharacterKind.CharacterCount]; //���� �������� ĳ���� ���� ���� �Ǻ�

    private Vector3 dragStartMousePos; //�巡�� �� ���콺 ����Ʈ ���� ����

    public bool isDraging; //�巡�� ������ �Ǻ�


    private void Start()
    {
        isDraging = false;

        minIndex = 0;
        maxIndex = (int)CharacterKind.CharacterCount - 1;
        
        for (int nowIndex = minIndex; nowIndex <= maxIndex; nowIndex++)
        {
            unlockCost[nowIndex] = 10;
        }
        isUnlock[minIndex] = true;
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
        slideableBgRectTransform[nowIndex].DOAnchorPos(new Vector2(imageTargetPosX, 0), 0.3f);

        if (isRightDrag && nowIndex == minIndex || isRightDrag == false && nowIndex == maxIndex)
        {
            StartCoroutine(ImagePosInitialization(-imageTargetPosX, isRightDrag));
            yield break;
        }
        else
        {
            nowIndex = isRightDrag ? nowIndex - 1 : nowIndex + 1;
        }

        slideableBgRectTransform[nowIndex].DOAnchorPos(new Vector2(0, 0), 0.3f);
        TextReSettings();
        yield return new WaitForSeconds(0.035f);
        isDraging = false;
    }

    IEnumerator ImagePosInitialization(int targetPos, bool isRightDrag)
    {
        for (int nowIndex = 0; nowIndex <= maxIndex; nowIndex++) //��ü �̹��� ��ġ �ʱ�ȭ(���� �̵����� �̹����� ������ �� ��ġ ����)
        {
            if (isRightDrag == false && nowIndex == maxIndex || isRightDrag && nowIndex == minIndex)
            {
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, true));
            }
            else
            {
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, false));
            }
        }

        nowIndex = isRightDrag ? maxIndex : minIndex;

        slideableBgRectTransform[nowIndex].DOAnchorPos(new Vector2(0, 0), 0.3f);
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

    /// <summary>
    /// ĳ���� ���� or ������� ��ư Ŭ�� ��
    /// </summary>
    public void ProficiencyUnlockOrEquip()
    {
        if (isUnlock[nowIndex] == false && unlockCost[nowIndex] <= GameManager.Instance.CurrentProficiency)
        {
            isUnlock[nowIndex] = true;

            lockObj[nowIndex].SetActive(false);

            TextReSettings();
        }
        else if(isUnlock[nowIndex] && isEquiping[nowIndex] == false)
        {
            for (int nowIndex = minIndex; nowIndex <= maxIndex; nowIndex++) //�ٸ� �ε��� ĳ���͵��� ���� ����, ���� �ε��� ĳ���͸� �������� ����
            {
                isEquiping[nowIndex] = nowIndex == this.nowIndex ? true : false;
            }

            Player.Instance.CharacterChange(nowIndex);

            TextReSettings();
        }
    }

    /// <summary>
    /// �ؽ�Ʈ�� ����
    /// </summary>
    public void TextReSettings()
    {
        nameText.text = $"{nowChooseCharacterName[nowIndex]}";

        buffText.text = $"{nowChooseCharacterBuff[nowIndex]}";

        if (isUnlock[nowIndex] == false)
        {
            guideText.color = (unlockCost[nowIndex] <= GameManager.Instance.CurrentProficiency) ? textColors[(int)TextColorKind.Green] : textColors[(int)TextColorKind.Red];

            guideText.transform.localPosition = new Vector3(0, 185, 0);

            guideText.text = "ĳ���� �������";
            priceText.text = $"{unlockCost[nowIndex]}";

            goodsImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(-70 + ((priceText.text.Length - 1) * -20), 15, 0); //�ʿ� ��ȭ ������ ���� �̹��� ��ġ ����

            goodsImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            guideText.color = textColors[(int)TextColorKind.Black];

            goodsImage.color = new Color(1, 1, 1, 0);
            
            guideText.transform.localPosition = new Vector3(0, 140, 0);
            
            priceText.text = "";

            guideText.text = isEquiping[nowIndex] == false ? guideText.text = "���� ����" : guideText.text = "������";
        }
    }
}