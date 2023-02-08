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

public class ProficiencySystemManager : Singleton<ProficiencySystemManager>
{
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

    [Tooltip("�� ��Ų �ý��� ĳ���� �����͵�")]
    public CharacterData[] characterDatas;

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
    [Tooltip("���׷��̵�/������� ��ư ��ȭ �̹���")]
    private Image goodsImage;

    [SerializeField]
    [Tooltip("�ؽ�Ʈ �� ����")]
    private Color[] textColors;

    [Tooltip("���� ĳ���� ��Ų")]
    public CharacterKind nowKind;

    [Tooltip("�巡�� �� ���콺 ����Ʈ ���� ����")]
    private Vector3 dragStartMousePos;

    [Tooltip("�巡�� ������ �Ǻ�")]
    private bool isDraging; 

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
        int imageTargetPosX;

        imageTargetPosX = isRightDrag ? 1300 : -1300; //���������� �巡�� �ߴٸ� ���� �̹��� �̵� ��ǥ X�� 1300���� ����(�������� �巡�� �� -1300)

        isDraging = true;
        characterDatas[(int)nowKind].bgAnchorPos.DOAnchorPos(new Vector2(imageTargetPosX, 0), 0.3f);

        if (isRightDrag && nowKind == CharacterKind.Basic || isRightDrag == false && nowKind == CharacterKind.FourthCharacter) //
        {
            StartCoroutine(ImagePosInitialization(-imageTargetPosX, isRightDrag));
            yield break;
        }
        else
        {
            nowKind = isRightDrag ? nowKind - 1 : nowKind + 1;
        }

        characterDatas[(int)nowKind].bgAnchorPos.DOAnchorPos(new Vector2(0, 0), 0.3f);

        TextReSettings();

        yield return new WaitForSeconds(0.035f);
        
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
        for (int nowIndex = 0; nowIndex <= (int)CharacterKind.FourthCharacter; nowIndex++) //��ü �̹��� ��ġ �ʱ�ȭ(���� �̵����� �̹����� ������ �� ��ġ ����)
        {
            if (isRightDrag == false && nowIndex == (int)CharacterKind.FourthCharacter || isRightDrag && nowIndex == (int)CharacterKind.Basic)
            {
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, true));
            }
            else
            {
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, false));
            }
        }

        nowKind = isRightDrag ? CharacterKind.FourthCharacter : CharacterKind.Basic;

        characterDatas[(int)nowKind].bgAnchorPos.DOAnchorPos(new Vector2(0, 0), 0.3f);

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
            yield return new WaitForSeconds(0.35f);
            isDraging = false;
        }
        characterDatas[moveImageIndex].bgAnchorPos.DOAnchorPos(targetPos, 0);
    }

    /// <summary>
    /// ĳ���� ���� or ������� ��ư Ŭ�� ��
    /// </summary>
    public void ProficiencyUnlockOrEquip()
    {
        if (characterDatas[(int)nowKind].isUnlock == false && characterDatas[(int)nowKind].unlockCost <= GameManager.Instance.GemUnit)
        {
            GameManager.Instance.GemUnit -= characterDatas[(int)nowKind].unlockCost;

            characterDatas[(int)nowKind].isUnlock = true;

            characterDatas[(int)nowKind].lockObj.SetActive(false);

            TextReSettings();
        }
        else if(characterDatas[(int)nowKind].isUnlock && characterDatas[(int)nowKind].isEquiping == false)
        {
            for (int nowIndex = (int)CharacterKind.Basic; nowIndex <= (int)CharacterKind.FourthCharacter; nowIndex++) //�ٸ� �ε��� ĳ���͵��� ���� ����, ���� �ε��� ĳ���͸� �������� ����
            {
                characterDatas[(int)nowKind].isEquiping = (nowIndex == (int)nowKind) ? true : false;
            }

            Player.Instance.CharacterChange((int)nowKind);

            TextReSettings();
        }
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
            guideText.color = (characterDatas[(int)nowKind].unlockCost <= GameManager.Instance.GemUnit) ? textColors[(int)TextColorKind.Green] : textColors[(int)TextColorKind.Red];

            guideText.transform.localPosition = new Vector3(0, 185, 0);

            guideText.text = "ĳ���� �������";
            priceText.text = $"{characterDatas[(int)nowKind].unlockCost}";

            goodsImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(-70 + ((priceText.text.Length - 1) * -20), 15, 0); //�ʿ� ��ȭ ������ ���� �̹��� ��ġ ����

            goodsImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            guideText.color = textColors[(int)TextColorKind.Black];

            goodsImage.color = new Color(1, 1, 1, 0);
            
            guideText.transform.localPosition = new Vector3(0, 140, 0);
            
            priceText.text = "";

            guideText.text = characterDatas[(int)nowKind].isEquiping == false ? guideText.text = "���� ����" : guideText.text = "������";
        }
    }
}