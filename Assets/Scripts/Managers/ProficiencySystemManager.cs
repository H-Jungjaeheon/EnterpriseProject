using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum CharacterKind
{
    Basic,
    SecondaryCharacter,
    ThirdCharacter,
    FourthCharacter
}

public enum TextColorKind
{
    Black,
    Red,
    Green
}

[System.Serializable]
public class CharacterData
{
    [Tooltip("ĳ���� �̸�")]
    public string name;

    [Tooltip("ĳ���� ���� ����")]
    public string buffContents;

    [Tooltip("ĳ���� ��� ������")]
    public RectTransform bgAnchorPos;

    [Tooltip("ĳ���� ��� ǥ�� ������Ʈ")]
    public GameObject lockObj;

    [Tooltip("ĳ���� ��� ���� ����")]
    public int unlockCost;

    [Tooltip("ĳ���� ��� ���� ���� �Ǻ�")]
    public bool isUnlock;

    [Tooltip("ĳ���� ���� ���� �Ǻ�")]
    public bool isEquiping;
}

public class ProficiencySystemManager : Singleton<ProficiencySystemManager>
{
    [Tooltip("�� ��Ų �ý��� ĳ���� �����͵�")]
    public CharacterData[] characterDatas;

    [SerializeField]
    [Tooltip("���� ĳ���� �̸� �ؽ�Ʈ")]
    Text nameText;

    [SerializeField]
    [Tooltip("���� ĳ���� ���� ���� �ؽ�Ʈ")]
    Text buffText;

    [SerializeField]
    [Tooltip("���� ĳ���� ����/������� ��ư �ȳ� �ؽ�Ʈ")]
    Text guideText;

    [SerializeField]
    [Tooltip("���� ĳ���� ����/������� ��ư ���� �ؽ�Ʈ")]
    Text priceText;

    [SerializeField]
    [Tooltip("���׷��̵�/������� ��ư ��ȭ �̹���")]
    Image goodsImage;

    [Tooltip("goodsImage�� RectTransform ������Ʈ")]
    RectTransform goodsImageRt;

    [Tooltip("���� ĳ���� ��Ų")]
    public CharacterKind nowKind;

    [Tooltip("�巡�� �� ���콺 ����Ʈ ���� ����")]
    Vector3 dragStartMousePos;

    [Tooltip("�巡�� ������ �Ǻ�")]
    bool isDraging;

    [Tooltip("�ؽ�Ʈ ������ ���ÿ� Vector3")]
    Vector3 textPos = Vector3.zero;

    [Tooltip("��Ų �ý��ۿ� ���� ���� ������")]
    WaitForSeconds delay = new WaitForSeconds(0.5f);

    #region �ν��Ͻ� ����
    [Tooltip("GameManager �̱��� �ν��Ͻ�")]
    GameManager gameManager;

    [Tooltip("Player �̱��� �ν��Ͻ�")]
    Player player;
    #endregion

    #region ��ȸ ��� ���ڿ� ����
    [Tooltip("GuideText : ��Ų ��� ���� �ȳ� ���ڿ�")]
    const string SKIN_UNLOCK = "��Ų �������";

    [Tooltip("GuideText : ��Ų ���� ���� �ȳ� ���ڿ�")]
    const string SKIN_EQUIPABLE = "���� ����";

    [Tooltip("GuideText : ��Ų ��� ���� �ȳ� ���ڿ�")]
    const string SKIN_EQUIPING = "������";

    [Tooltip("�� ���ڿ�")]
    const string NULL = "";
    #endregion

    private void Start()
    {
        gameManager = GameManager.Instance;
        player = Player.Instance;

        goodsImageRt = goodsImage.GetComponent<RectTransform>();
    }

    void OnEnable()
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

    /// <summary>
    /// �巡�� ���� �� ���� �Լ�
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// �巡�� �� �̹��� ��ġ ����(�ִϸ��̼�) �Լ�
    /// </summary>
    /// <param name="isRightDrag"> ���� �巡�� ������ ���������� �Ǻ� </param>
    /// <returns></returns>
    IEnumerator Draging(bool isRightDrag)
    {
        Vector2 bgPos = Vector2.zero;
        int imageTargetPosX = isRightDrag ? 1300 : -1300; //���������� �巡�� �ߴٸ� ���� �̹��� �̵� ��ǥ X�� 1300���� ����(�������� �巡�� �� -1300)

        bgPos.x = imageTargetPosX;

        isDraging = true;

        characterDatas[(int)nowKind].bgAnchorPos.DOAnchorPos(bgPos, 0.3f);

        if (isRightDrag && nowKind == CharacterKind.Basic || isRightDrag == false && nowKind == CharacterKind.FourthCharacter)
        {
            StartCoroutine(ImagePosInitialization(-imageTargetPosX, isRightDrag));

            yield break;
        }
        else
        {
            nowKind = isRightDrag ? nowKind - 1 : nowKind + 1;
        }

        characterDatas[(int)nowKind].bgAnchorPos.DOAnchorPos(Vector2.zero, 0.3f);

        TextReSettings();

        yield return delay;
        
        isDraging = false;
    }

    /// <summary>
    /// �̹��� ���ġ : �� �̹��� �ε����� ������ ó�� or ó������ �� �ε����� �Ѿ ��
    /// </summary>
    /// <param name="targetPos"> ���ġ �̹��� ��ǥ ������ </param>
    /// <param name="isRightDrag"> ���� �巡�� ������ ���������� �Ǻ� </param>
    /// <returns></returns>
    IEnumerator ImagePosInitialization(int targetPos, bool isRightDrag)
    {
        Vector2 curTargetPos = Vector2.zero;
        curTargetPos.x = targetPos;

        for (int nowIndex = 0; nowIndex <= (int)CharacterKind.FourthCharacter; nowIndex++) //��ü �̹��� ��ġ �ʱ�ȭ(���� �̵����� �̹����� ������ �� ��ġ ����)
        {
            if (isRightDrag == false && nowIndex == (int)CharacterKind.FourthCharacter || isRightDrag && nowIndex == (int)CharacterKind.Basic)
            {
                StartCoroutine(ImagerelocationDelay(curTargetPos, nowIndex, true));
            }
            else
            {
                StartCoroutine(ImagerelocationDelay(curTargetPos, nowIndex, false));
            }
        }

        nowKind = isRightDrag ? CharacterKind.FourthCharacter : CharacterKind.Basic;

        characterDatas[(int)nowKind].bgAnchorPos.DOAnchorPos(Vector2.zero, 0.3f);

        TextReSettings();

        yield return null;
    }

    /// <summary>
    /// �̹��� ���ġ : ���ġ ���� ������ �ڷ�ƾ
    /// </summary>
    /// <param name="targetPos"> �̹����� ���ġ ������ </param>
    /// <param name="moveImageIndex"> ���ġ�� �̹����� �ε��� </param>
    /// <param name="isGiveDelay"> ���ġ ������ ����(���� �̵����� �̹����� ������ �ֱ�) </param>
    /// <returns></returns>
    IEnumerator ImagerelocationDelay(Vector2 targetPos, int moveImageIndex, bool isGiveDelay)
    {
        if (isGiveDelay)
        {
            yield return delay;

            isDraging = false;
        }

        characterDatas[moveImageIndex].bgAnchorPos.DOAnchorPos(targetPos, 0);
    }

    /// <summary>
    /// ĳ���� ���� or ������� ��ư Ŭ�� ��
    /// </summary>
    public void ProficiencyUnlockOrEquip()
    {
        if (characterDatas[(int)nowKind].isUnlock == false && characterDatas[(int)nowKind].unlockCost <= gameManager.GemUnit)
        {
            gameManager.GemUnit -= characterDatas[(int)nowKind].unlockCost;

            characterDatas[(int)nowKind].isUnlock = true;

            characterDatas[(int)nowKind].lockObj.SetActive(false);
        }
        else if(characterDatas[(int)nowKind].isUnlock && characterDatas[(int)nowKind].isEquiping == false)
        {
            for (int nowIndex = (int)CharacterKind.Basic; nowIndex <= (int)CharacterKind.FourthCharacter; nowIndex++) //�ٸ� �ε��� ĳ���͵��� ���� ����, ���� �ε��� ĳ���͸� �������� ����
            {
                characterDatas[nowIndex].isEquiping = (nowIndex == (int)nowKind) ? true : false;
            }

            player.CharacterChange((int)nowKind);
        }

        TextReSettings();
    }

    /// <summary>
    /// �ؽ�Ʈ ���� �Լ�
    /// </summary>
    public void TextReSettings()
    {
        nameText.text = $"{characterDatas[(int)nowKind].name}";

        buffText.text = $"{characterDatas[(int)nowKind].buffContents}";

        if (characterDatas[(int)nowKind].isUnlock == false)
        {
            guideText.color = (characterDatas[(int)nowKind].unlockCost <= gameManager.GemUnit) ? Color.green : Color.red;
            goodsImage.color = Color.white;

            textPos.y = 185f;
            guideText.transform.localPosition = textPos;

            guideText.text = SKIN_UNLOCK;
            priceText.text = $"{characterDatas[(int)nowKind].unlockCost}";

            goodsImageRt.anchoredPosition = new Vector3(-70 + ((priceText.text.Length - 1) * -20), 15, 0); //�ʿ� ��ȭ ������ ���� �̹��� ��ġ ����
        }
        else
        {
            guideText.color = Color.black;
            goodsImage.color = Color.clear;

            textPos.y = 140f;
            guideText.transform.localPosition = textPos;
            
            priceText.text = NULL;
            guideText.text = characterDatas[(int)nowKind].isEquiping == false ? guideText.text = SKIN_EQUIPABLE : guideText.text = SKIN_EQUIPING;
        }
    }
}