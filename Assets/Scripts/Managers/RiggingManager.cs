using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RiggingType
{
    Shoes,
    Bowl,
    Spoon
}

[System.Serializable]
public class Rigging
{
    [Tooltip("��� ID")]
    public int id;

    [Tooltip("��� ����")]
    public RiggingType type;

    [Tooltip("��� ������� ����")]
    public bool isUnlock;

    [Tooltip("��� ������")]
    public Sprite icon;

    [Tooltip("��� �̸�")]
    public string name;

    [Tooltip("��� ����")]
    public string explanation;

    [Tooltip("��� ���")]
    public int rank;

    [Tooltip("��� ����")]
    public int level;

    [Tooltip("�ִ� ��� ����ġ")]
    public int maxExp;

    [Tooltip("���� ��� ����ġ")]
    public int nowExp;
}

public class RiggingManager : MonoBehaviour
{
    [Tooltip("��� �����͵�")]
    public Rigging[] riggingDatas;

    [Tooltip("���� ������ ����� ������ �ε���")]
    public int nowDataIndex;

    [SerializeField]
    [Tooltip("���� ������ ��� ������ ǥ�� �̹���")]
    private Image[] riggingImages;

    #region ��� ���� ��ư ��ҵ�
    [Header("��� ���� ��ư ��ҵ�")]

    [SerializeField]
    [Tooltip("��� ���� ��ư��")]
    private Button[] equipButtons;

    [SerializeField]
    [Tooltip("��� ���� ��ư �̹�����")]
    private Image[] buttonImage;

    [SerializeField]
    [Tooltip("��� ���� ��ư �� ��������Ʈ")]
    private Sprite nullSprite;
    #endregion

    #region ��� ���� ȭ�� ��ҵ� ����
    [Header("��� ���� ȭ�� ��ҵ�")]

    [SerializeField]
    [Tooltip("��� ���� ȭ�� ������Ʈ")]
    private GameObject panelObj;

    [SerializeField]
    [Tooltip("��� ������ �̹���")]
    private Image iconImage;

    [SerializeField]
    [Tooltip("��� �̸� �ؽ�Ʈ")]
    private Text nameText;

    [SerializeField]
    [Tooltip("��� ��� �ؽ�Ʈ")]
    private Text rankText;

    [SerializeField]
    [Tooltip("��� ���� �ؽ�Ʈ")]
    private Text partText;

    [SerializeField]
    [Tooltip("��� ���� �ؽ�Ʈ")]
    private Text explanationText;

    [SerializeField]
    [Tooltip("���� ��� �κ��丮 �ȳ� �̹���")]
    private Image guideImg;

    [SerializeField]
    [Tooltip("��� �κ��丮 �ȳ� ��������Ʈ ����")]
    private Sprite[] guideSprites;

    private const string shoesType = "�Ź�";

    private const string bowlType = "�׸�";

    private const string spoonType = "������";

    Rigging nowData;
    #endregion

    /// <summary>
    /// ��� ���� ȭ�� Ȱ��ȭ �� �Ź� ������ ����� ��ư ����
    /// </summary>
    private void OnEnable()
    {
        ButtonSetting(0);
    }

    /// <summary>
    /// ��� ���� ��ư�� ��� ���� ����(��ư)
    /// </summary>
    /// <param name="typeIndex"> ��� ���� �ε���(RiggingType) </param>
    public void ButtonSetting(int typeIndex)
    {
        RiggingType riggingType = (RiggingType)typeIndex; //���� ��� Ÿ��
        int buttonIndex = 0; //���� ��ư �ε���

        guideImg.sprite = guideSprites[typeIndex]; //���� ��� Ÿ�Կ� �´� �ȳ� �̹����� ����

        for (int nowIndex = equipButtons.Length - 1; nowIndex >= 0; nowIndex--) //������ �����ͺ��� 0��° �����ͱ��� �ݺ� (��ư �̺�Ʈ �ʱ�ȭ)
        {
            equipButtons[nowIndex].onClick.RemoveAllListeners();
            buttonImage[nowIndex].sprite = nullSprite;
        }

        for (int nowIndex = riggingDatas.Length - 1; nowIndex >= 0; nowIndex--) //������ �����ͺ��� 0��° �����ͱ��� �ݺ� (��ư �̺�Ʈ �߰�)
        {
            if (riggingDatas[nowIndex].type == riggingType && riggingDatas[nowIndex].isUnlock)
            {
                int dataIndex = nowIndex;

                equipButtons[buttonIndex].onClick.AddListener(() => QuestionPanelOn(dataIndex));
                buttonImage[buttonIndex].sprite = riggingDatas[nowIndex].icon;

                buttonIndex++;
            }
        }
    }

    /// <summary>
    /// ��� ���� �� ���� ȭ�� Ȱ��ȭ
    /// </summary>
    /// <param name="dataIndex"> ���� ��� ������ �ε��� </param>
    private void QuestionPanelOn(int dataIndex)
    {
        nowData = riggingDatas[dataIndex];

        nowDataIndex = dataIndex;

        iconImage.sprite = nowData.icon;
        nameText.text = nowData.name;
        rankText.text = nowData.rank.ToString();
        explanationText.text = nowData.explanation;

        switch (nowData.type)
        {
            case RiggingType.Shoes:
                partText.text = shoesType;
                break;
            case RiggingType.Bowl:
                partText.text = bowlType;
                break;
            case RiggingType.Spoon:
                partText.text = spoonType;
                break;
        }

        panelObj.SetActive(true);
    }

    /// <summary>
    /// ��� ���� �Լ�(��ư)
    /// </summary>
    public void EquipRigging()
    {
        nowData = riggingDatas[nowDataIndex];

        riggingImages[(int)nowData.type].sprite = nowData.icon;

        QuestionPanelClose();
    }

    /// <summary>
    /// ��� ���� �� ���� ȭ�� ��Ȱ��ȭ(��ư)
    /// </summary>
    public void QuestionPanelClose() => panelObj.SetActive(false);
}
