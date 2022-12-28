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
    #region 현재 상태 표기 관련 변수
    public Contents nowContents;

    private bool isClickDown;
    #endregion

    #region 콘텐츠 창 관련 변수들
    [Header("콘텐츠 창 관련 변수들")]

    [SerializeField]
    [Tooltip("현재 보여지는 콘텐츠 창 오브젝트")]
    private GameObject nowContentsObj;

    [Tooltip("콘텐츠 창 오브젝트 모음")]
    public GameObject[] contentsPanelObjs;

    [SerializeField]
    [Tooltip("동료 시스템 - 동료 시스템 창 오브젝트")]
    private GameObject colleagueSystemPanelObj;
    
    private GameObject lastContents; //전에 띄우던 콘텐츠 창 오브젝트

    private int nowChangeContents; //바꿀 콘텐츠 창 인덱스
    #endregion

    #region 스탯 업그레이드창 텍스트 모음
    [Header("스탯 업그레이드창의 텍스트 모음")]
    [SerializeField]
    [Tooltip("스탯 레벨 표기 텍스트")]
    private Text[] basicStatLevelText;
    [SerializeField]
    [Tooltip("현재 스탯 수치 표기 텍스트")]
    private Text[] basicStatFigureText;
    [SerializeField]
    [Tooltip("스탯 업그레이드 비용 표기 텍스트")]
    private Text[] goodsTextRequiredForUpgrade;
    #endregion

    [SerializeField]
    [Tooltip("코인 재화 텍스트")]
    private Text coinText;

    [SerializeField]
    [Tooltip("보석 재화 텍스트")]
    private Text jemText;

    [SerializeField]
    [Tooltip("숙련도 수치 텍스트")]
    private Text proficiencyText;

    [Header("그 외")]

    [SerializeField]
    [Tooltip("요리 판매 시스템 매니저 컴포넌트")]
    private SaleOfFoodManager sofManager;

    [SerializeField]
    [Tooltip("퀘스트 시스템 매니저 컴포넌트")]
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

            basicStatLevelText[nowIndex].text = $"Lv {gmInstance.statsLevel[nowIndex]}"; //레벨 텍스트 수정

            goodsRequiredForUpgradeString = ConvertGoodsToString(goodsRequiredForUpgrade[nowIndex]);

            goodsTextRequiredForUpgrade[nowIndex].text = $"강화\n{goodsRequiredForUpgradeString}";
        }
    }

    public void BasicStatSetting() // 스탯 세팅
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

    private void StatUpgrade(int statsToUpgradeCurrently) //스탯 업그레이드 함수
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

        gmInstance.statsLevel[statsToUpgradeCurrently]++; //레벨 증가

        basicStatLevelText[statsToUpgradeCurrently].text = $"Lv {gmInstance.statsLevel[statsToUpgradeCurrently]}"; //레벨 텍스트 수정

        goodsRequiredForUpgrade[statsToUpgradeCurrently] += goodsRequiredForUpgrade[statsToUpgradeCurrently] / 2; //강화 비용 수정(임시 연산)

        goodsRequiredForUpgradeString = ConvertGoodsToString(goodsRequiredForUpgrade[statsToUpgradeCurrently]);

        goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text = $"강화\n{goodsRequiredForUpgradeString}";

        switch (statsToUpgradeCurrently)
        {
            case (int)UpgradeableBasicStats.Damage:
                playerComponent.AttackPower += 10; //공격력 증가(임시 연산)
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
    /// 콘텐츠 창 변경 (팝업되는 콘텐츠들)
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
    /// 콘텐츠 창 닫기 (팝업되는 콘텐츠들)
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
    /// 현재 콘텐츠 인덱스 변경(버튼)
    /// </summary>
    /// <param name="ChangeIndex"> 변경할 콘텐츠 인덱스 </param>
    public void NowContentsChange(int ChangeIndex)
    {
        nowChangeContents = ChangeIndex;
    }

    /// <summary>
    /// 스크롤 뷰에서 보이는 콘텐츠 창 변경
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
    /// 재화 단위 표시 함수 (값 넣으면 string으로 리턴)
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
