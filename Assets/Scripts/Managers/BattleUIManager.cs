using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public enum Contents
{
    StatUpgrade,
    SaleOfFood,
    Colleague,
    Proficiency,
    SkillEquip,
    EquipRigging,
    ContentsLength
}

public enum UpgradeableBasicStats
{
    Damage,
    MaxHp,
    Healing,
    AttackSpeed,
    FatalAttackDamage,
    FatalAttackProbability,
    UpgradeableBasicStatsNumber
}

public class BattleUIManager : Singleton<BattleUIManager>
{
    #region ���� ���� ǥ�� ���� ����
    public Contents nowContents;

    private bool isClickDown;
    #endregion

    #region ������ â ���� ������
    [Header("������ â ���� ������")]

    [SerializeField]
    [Tooltip("���� �������� ������ â ������Ʈ")]
    private GameObject nowContentsObj;

    [Tooltip("������ â ������Ʈ ����")]
    public GameObject[] contentsPanelObjs;

    [SerializeField]
    [Tooltip("���� �ý��� - ���� �ý��� â ������Ʈ")]
    private GameObject colleagueSystemPanelObj;
    
    private GameObject lastContents; //���� ���� ������ â ������Ʈ

    private int nowChangeContents; //�ٲ� ������ â �ε���
    #endregion

    #region ���� ���׷��̵�â �ؽ�Ʈ ����
    [Header("���� ���׷��̵�â�� �ؽ�Ʈ ����")]
    [SerializeField]
    [Tooltip("���� ���� ǥ�� �ؽ�Ʈ")]
    private Text[] basicStatLevelText;
    [SerializeField]
    [Tooltip("���� ���� ��ġ ǥ�� �ؽ�Ʈ")]
    private Text[] basicStatFigureText;
    [SerializeField]
    [Tooltip("���� ���׷��̵� ��� ǥ�� �ؽ�Ʈ")]
    private Text[] goodsTextRequiredForUpgrade;
    #endregion

    [SerializeField]
    [Tooltip("���� ��ȭ �ؽ�Ʈ")]
    private Text coinText;

    [SerializeField]
    [Tooltip("���� ��ȭ �ؽ�Ʈ")]
    private Text jemText;

    [SerializeField]
    [Tooltip("���õ� ��ġ �ؽ�Ʈ")]
    private Text proficiencyText;

    [Header("�� ��")]

    [SerializeField]
    [Tooltip("�丮 �Ǹ� �ý��� �Ŵ��� ������Ʈ")]
    private SaleOfFoodManager sofManager;

    [SerializeField]
    [Tooltip("����Ʈ �ý��� �Ŵ��� ������Ʈ")]
    private QuestManager questManager;

    private BigInteger[] goodsRequiredForUpgrade = new BigInteger[(int)UpgradeableBasicStats.UpgradeableBasicStatsNumber];

    [SerializeField]
    private GameObject player;

    private GameManager gmInstance;

    private float nowClickTime;

    void Start()
    {
        StartSetting();
        BasicStatSetting();
        basicStatLevelText[(int)UpgradeableBasicStats.Damage].text = $"Lv {GameManager.Instance.statsLevel[(int)UpgradeableBasicStats.Damage]}";
    }

    void Update()
    {
        TextSettings();
    }

    private void TextSettings()
    {
        coinText.text = $"{ConvertGoodsToString(gmInstance.MoneyUnit)}";
        jemText.text = $"{gmInstance.GemUnit}";
        proficiencyText.text = $"{gmInstance.CurrentProficiency}";
    }

    private void StartSetting()
    {
        gmInstance = GameManager.Instance;

        for (int nowIndex = 0; nowIndex < goodsRequiredForUpgrade.Length; nowIndex++)
        {
            string goodsRequiredForUpgradeString;

            goodsRequiredForUpgrade[nowIndex] = 2;

            basicStatLevelText[nowIndex].text = $"Lv {gmInstance.statsLevel[nowIndex]}"; //���� �ؽ�Ʈ ����

            goodsRequiredForUpgradeString = ConvertGoodsToString(goodsRequiredForUpgrade[nowIndex]);

            goodsTextRequiredForUpgrade[nowIndex].text = $"��ȭ\n{goodsRequiredForUpgradeString}";
        }
    }

    public void BasicStatSetting() // ���� ����
    {
        basicStatFigureText[0].text = $"{Player.Instance.AttackPower}";
        basicStatFigureText[1].text = $"{Player.Instance.MaxHp}";
        basicStatFigureText[2].text = $"{Player.Instance.HealingValue}";
        basicStatFigureText[3].text = $"{Player.Instance.AttackDelay}";
        basicStatFigureText[4].text = $"{Player.Instance.CriticalDamage}%";
        basicStatFigureText[5].text = $"{Player.Instance.CriticalPercent}%";
    }

    public void NowUpgradeButtonClick(int statsToUpgradeCurrently)
    {
        isClickDown = true;
        StartCoroutine(ButtonClicking(statsToUpgradeCurrently));
    }

    public void NowUpgradeButtonClickEnd(int statsToUpgradeCurrently)
    {
        StatUpgrade(statsToUpgradeCurrently);
        nowClickTime = 0;
        isClickDown = false;
    }

    IEnumerator ButtonClicking(int statsToUpgradeCurrently)
    {
        while (isClickDown)
        {
            nowClickTime += Time.deltaTime;
            if (nowClickTime >= 0.5f)
            {
                StatUpgrade(statsToUpgradeCurrently);
            }
            yield return null;
        }
    }

    private void StatUpgrade(int statsToUpgradeCurrently) //���� ���׷��̵� �Լ�
    {
        if (isClickDown == false || gmInstance.MoneyUnit < goodsRequiredForUpgrade[statsToUpgradeCurrently])
        {
            return;
        }
        else
        {
            gmInstance.MoneyUnit -= goodsRequiredForUpgrade[statsToUpgradeCurrently];
            questManager.datas[statsToUpgradeCurrently].nowFigure++;
            questManager.InformationFix();
        }

        var playerComponent = player.GetComponent<Player>();
        string goodsRequiredForUpgradeString;

        gmInstance.statsLevel[statsToUpgradeCurrently]++; //���� ����

        basicStatLevelText[statsToUpgradeCurrently].text = $"Lv {gmInstance.statsLevel[statsToUpgradeCurrently]}"; //���� �ؽ�Ʈ ����

        goodsRequiredForUpgrade[statsToUpgradeCurrently] += goodsRequiredForUpgrade[statsToUpgradeCurrently] / 2; //��ȭ ��� ����(�ӽ� ����)

        goodsRequiredForUpgradeString = ConvertGoodsToString(goodsRequiredForUpgrade[statsToUpgradeCurrently]);

        goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text = $"��ȭ\n{goodsRequiredForUpgradeString}";

        switch (statsToUpgradeCurrently)
        {
            case (int)UpgradeableBasicStats.Damage:
                playerComponent.AttackPower += 10; //���ݷ� ����(�ӽ� ����)
                basicStatFigureText[statsToUpgradeCurrently].text = $"{playerComponent.AttackPower}";
                break;

            case (int)UpgradeableBasicStats.MaxHp:
                playerComponent.MaxHp += 10;
                basicStatFigureText[statsToUpgradeCurrently].text = $"{playerComponent.MaxHp}";
                break;

            case (int)UpgradeableBasicStats.Healing:
                playerComponent.HealingValue += 10;
                basicStatFigureText[statsToUpgradeCurrently].text = $"{playerComponent.HealingValue}";
                break;

            case (int)UpgradeableBasicStats.AttackSpeed:
                playerComponent.AttackDelay -= 0.01f;
                basicStatFigureText[statsToUpgradeCurrently].text = $"{playerComponent.AttackDelay}";
                break;

            case (int)UpgradeableBasicStats.FatalAttackDamage:
                playerComponent.CriticalDamage += 5;
                basicStatFigureText[statsToUpgradeCurrently].text = $"{playerComponent.CriticalDamage}%";
                break;

            case (int)UpgradeableBasicStats.FatalAttackProbability:
                playerComponent.CriticalPercent += 0.5f;
                basicStatFigureText[statsToUpgradeCurrently].text = $"{playerComponent.CriticalPercent}%";
                break;
        }
    }

    /// <summary>
    /// ������ â ���� (�˾��Ǵ� ��������)
    /// </summary>
    /// <param name="popUpObj"></param>
    public void AnotherContentsPopUp(GameObject popUpObj)
    {
        lastContents = nowContentsObj;
        nowContentsObj = popUpObj;

        nowContentsObj.SetActive(true);

        nowContents = (Contents)nowChangeContents;

        if (sofManager.isCustomerArrival == false)
        {
            sofManager.isCustomerArrival = true;
        }
    }

    /// <summary>
    /// ������ â �ݱ� (�˾��Ǵ� ��������)
    /// </summary>
    /// <param name="PopUpObj"></param>
    public void PopUpClose(GameObject PopUpObj)
    {
        PopUpObj.SetActive(false);

        if (lastContents == contentsPanelObjs[(int)Contents.StatUpgrade])
        {
            nowChangeContents = (int)Contents.StatUpgrade;
        }
        else if (lastContents == contentsPanelObjs[(int)Contents.SaleOfFood])
        {
            nowChangeContents = (int)Contents.SaleOfFood;
        }
        else
        {
            nowChangeContents = (int)Contents.SkillEquip;
        }

        nowContentsObj = lastContents;
        nowContents = (Contents)nowChangeContents;
    }

    /// <summary>
    /// ���� ������ �ε��� ����(��ư)
    /// </summary>
    /// <param name="ChangeIndex"> ������ ������ �ε��� </param>
    public void NowContentsChange(int ChangeIndex)
    {
        nowChangeContents = ChangeIndex;
    }

    /// <summary>
    /// ��ũ�� �信�� ���̴� ������ â ����
    /// </summary>
    /// <param name="PopUpObj"></param>
    public void AnotherContentsChangeScrollView(GameObject PopUpObj)
    {
        bool isSameContents = PopUpObj.activeSelf;

        nowContentsObj.SetActive(false);
        nowContentsObj = isSameContents ? contentsPanelObjs[(int)Contents.StatUpgrade] : PopUpObj;

        nowContents = isSameContents ? Contents.StatUpgrade : (Contents)nowChangeContents;
        nowContentsObj.SetActive(true);

        if (sofManager.isCustomerArrival == false)
        {
            sofManager.isCustomerArrival = true;
        }
    }

    /// <summary>
    /// ��ȭ ���� ǥ�� �Լ� (�� ������ string���� ����)
    /// </summary>
    /// <param name="theValueOfAGood"></param>
    /// <returns></returns>
    public string ConvertGoodsToString(BigInteger theValueOfAGood)
    {
        int nowAUnitOfGoodsIndex = 0;
        string translatedString;

        while (theValueOfAGood > 1000)
        {
            theValueOfAGood /= 1000;
            nowAUnitOfGoodsIndex++;
        }

        translatedString = (nowAUnitOfGoodsIndex == 0) ? $"{theValueOfAGood}" : $"{theValueOfAGood}{(char)(96 + nowAUnitOfGoodsIndex)}";

        return translatedString;
    }
}
