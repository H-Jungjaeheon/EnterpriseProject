using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public enum ColleagueKind
{
    FirstColleague,
    SecondColleague,
    ColleagueCount
}

public class ColleagueSystemManager : Singleton<ColleagueSystemManager>
{
    [System.Serializable]
    public class ColleagueData
    {
        [Tooltip("��� ���� �� �ʿ��� ����")]
        public int needGem;

        [Tooltip("��� ���� �Ǻ�")]
        public bool isUnlock;

        [Tooltip("���׷��̵� �� �ʿ��� ��")]
        public BigInteger needGold;

        [Tooltip("���� ����")]
        public int level;

        [Tooltip("���� ��ư ������Ʈ")]
        public GameObject buttonObj;

        [Tooltip("���� ǥ�� �ؽ�Ʈ")]
        public Text priceText;

        [Tooltip("���� ������")]
        public Sprite icon;

        [Tooltip("���� ������")]
        public PartnerData partnerData;
    }

    public ColleagueData[] colleagueDatas;

    [SerializeField]
    private Partner Partner;

    [SerializeField]
    [Tooltip("���� �������� ���� ������")]
    private SpriteRenderer nowColleagueIcon;

    Color redTextColor = new Color(1f, 0f, 0f);

    Color greenTextColor = new Color(0f, 1f, 0.03f);

    void Start()
    {
        StartSettings();
    }

    private void OnEnable()
    {
        OnEnableSetting();
    }

    /// <summary>
    /// ���� �ý��� â Ȱ��ȭ �� ����
    /// </summary>
    private void OnEnableSetting()
    {
        if (gameObject.activeSelf)
        {
            TextColorChange();
        }
        EquipButtonSetActive();
    }

    /// <summary>
    /// �ʱ� ����
    /// </summary>
    private void StartSettings()
    {
        for (int nowIndex = 0; nowIndex < (int)ColleagueKind.ColleagueCount; nowIndex++)
        {
            colleagueDatas[nowIndex].needGold = 10;
        }
    }

    /// <summary>
    /// ���� ���� ����(��ư)
    /// </summary>
    /// <param name="nowEquipColleagueIndex"> ���� ������ ���� �ε��� </param>
    public void EquipColleague(int nowEquipColleagueIndex)
    {
        if (nowColleagueIcon.sprite != colleagueDatas[nowEquipColleagueIndex].icon)
        {
            Partner.PartnerData = this.colleagueDatas[nowEquipColleagueIndex].partnerData;
            Partner.Setting();
            Partner.gameObject.SetActive(true);

            Partner.NowIdx = nowEquipColleagueIndex;

            nowColleagueIcon.sprite = colleagueDatas[nowEquipColleagueIndex].icon;
        }
    }

    /// <summary>
    /// ���� ���׷��̵� or ��� ���� �Լ�(��ư)
    /// </summary>
    /// <param name="nowColleagueIndex"> ���� ���׷��̵� or ��� ������ ���� �ε��� </param>
    public void ColleagueUpgradeOrUnlock(int nowColleagueIndex)
    {
        if (colleagueDatas[nowColleagueIndex].isUnlock == false && GameManager.Instance.GemUnit >= colleagueDatas[nowColleagueIndex].needGem) //�ش� �ε��� ���� �� ��������� ���
        {
            GameManager.Instance.GemUnit -= colleagueDatas[nowColleagueIndex].needGem; //�ش� �ε��� ���� ��� ���� ��� ����

            colleagueDatas[nowColleagueIndex].isUnlock = true; //�ش� �ε��� ���� ��� ����
                
            EquipButtonSetActive();
        }
        else if (colleagueDatas[nowColleagueIndex].isUnlock && GameManager.Instance.MoneyUnit >= colleagueDatas[nowColleagueIndex].needGold) //�ش� �ε��� ���� ��������� ���
        {
            GameManager.Instance.MoneyUnit -= colleagueDatas[nowColleagueIndex].needGold; //�ش� �ε��� ���� ���׷��̵� ��� ����

            colleagueDatas[nowColleagueIndex].level++; //�ش� �ε��� ���� ���� ����

            colleagueDatas[nowColleagueIndex].needGold += colleagueDatas[nowColleagueIndex].needGold / 2; //�ش� �ε��� ���� ���׷��̵� ��� ����
        }
        else
        {
            return;
        }

        colleagueDatas[nowColleagueIndex].priceText.text = $"���׷��̵�\n{colleagueDatas[nowColleagueIndex].needGold} ���"; //�ش� �ε��� ���� ��ư �ؽ�Ʈ ����
    }

    /// <summary>
    /// �ؽ�Ʈ �� ����(�ؽ�Ʈ ���� ����)
    /// </summary>
    public void TextColorChange()
    {
        for (int nowIndex = 0; nowIndex < (int)ColleagueKind.ColleagueCount; nowIndex++)
        {
            if (colleagueDatas[nowIndex].isUnlock) //��� ���� �Ϸ� ��
            {
                colleagueDatas[nowIndex].priceText.color = (colleagueDatas[nowIndex].needGold <= GameManager.Instance.MoneyUnit) ? greenTextColor : redTextColor;
                colleagueDatas[nowIndex].priceText.text = $"���׷��̵�\n{colleagueDatas[nowIndex].needGold} ���"; //�ش� �ε��� ���� ��ư �ؽ�Ʈ ����
            }
            else //��� ���� �� �Ϸ� ��
            {
                colleagueDatas[nowIndex].priceText.color = (colleagueDatas[nowIndex].needGem <= GameManager.Instance.GemUnit) ? greenTextColor : redTextColor;
                colleagueDatas[nowIndex].priceText.text = $"����\n{colleagueDatas[nowIndex].needGem} ��"; //�ش� �ε��� ���� ��ư �ؽ�Ʈ ����
            }
        }
    }

    /// <summary>
    /// ���� ��ư Ȱ��ȭ
    /// </summary>
    private void EquipButtonSetActive()
    {
        for (int nowIndex = 0; nowIndex < (int)ColleagueKind.ColleagueCount; nowIndex++)
        {
            if (colleagueDatas[nowIndex].isUnlock && colleagueDatas[nowIndex].buttonObj.activeSelf == false)
            {
                colleagueDatas[nowIndex].buttonObj.SetActive(true);
            }
        }
    }
}
