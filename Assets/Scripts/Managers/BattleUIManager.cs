using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using DG.Tweening;

public enum Contents
{
    StatUpgradeContents,
    SaleOfFoodContents,
    ColleagueContents,
    ContentsLength
}

public enum SaleOfFoodContents
{
    BasicScreen,
    ChooseFoodScreen,
    FoodMakingScreen
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
    private Contents nowContents;

    private SaleOfFoodContents nowSaleOfFoodContents;
    #endregion

    #region 콘텐츠 창 관련 변수들
    [Header("콘텐츠 창 관련 변수들")]

    [SerializeField]
    [Tooltip("현재 보여지는 콘텐츠 창 오브젝트")]
    private GameObject nowContentsPanelObj;

  
    [Tooltip("콘텐츠 창 오브젝트 모음")]
    public GameObject[] contentsPanelObjs;

    [SerializeField]
    [Tooltip("요리 판매 시스템 - 요리 선택 및 제작 창 오브젝트")]
    private GameObject foodChooseAndMakePanelObj;

    [SerializeField]
    [Tooltip("요리 판매 시스템 - 요리 선택 창 오브젝트")]
    private GameObject chooseADishObj;

    [SerializeField]
    [Tooltip("요리 판매 시스템 - 요리 제작 창 오브젝트")]
    private GameObject theProductionObj;

    [SerializeField]
    [Tooltip("동료 시스템 - 동료 시스템 창 오브젝트")]
    private GameObject colleagueSystemPanelObj;
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

    #region 요리 판매 시스템 화면 텍스트, 변수, 오브젝트 모음
    [Header("재료 개수 표기 텍스트(기본 화면)")]
    [SerializeField]
    [Tooltip("기본 화면의 모든 등급 개수 표기 텍스트들 모음")]
    private Text[] materialsText_BasicScreen;

    [Header("재료 개수 표기 텍스트(요리 선택 화면)")]
    [SerializeField]
    [Tooltip("요리 선택 화면의 모든 등급 개수 표기 텍스트들 모음")]
    private Text[] materialsText_ChooseCookScreen;

    [SerializeField]
    [Tooltip("현재 제작할 요리 수량 표기 텍스트")]
    private Text nowCookingCountText;

    [SerializeField]
    [Tooltip("현재 제작할 요리 이름 표기 텍스트")]
    private Text nowCookingFoodNameText;

    private int nowFoodIndex;
    public int NowFoodIndex
    {
        get { return nowFoodIndex; }
        set { nowFoodIndex = value; }
    }

    private int cookingCount; //현재 요리 개수

    private int nowChangeContents; //바꿀 콘텐츠 창 인덱스

    [Header("현재 요리에 필요한 재료 개수들")]
    [SerializeField]
    [Tooltip("현재 요리에 필요한 재료 개수")]
    private int[] quantityOfMaterials;

    private bool[] isMeetingTheNumberOfMaterials = new bool[3];

    private bool isArrowMoving;

    [HideInInspector]
    public bool isCustomerArrival;

    [SerializeField]
    [Tooltip("손님 오브젝트")]
    private GameObject customerObj;

    [SerializeField]
    [Tooltip("제작할 음식 오브젝트")]
    private GameObject[] foodObjectsToCooking;

    [SerializeField]
    [Tooltip("미니게임 화살표 오브젝트")]
    private GameObject arrowObj;

    private Vector3 customerSpeed = new Vector3(1, 0, 0);

    Color redTextColor = new Color(1, 0, 0);

    Color greenTextColor = new Color(0, 1, 0.03f);

    [SerializeField]
    [Tooltip("각 음식들 필요 재료, 음식 이름")]
    private FoodData[] foodDatas;
    #endregion

    [SerializeField]
    [Tooltip("코인 재화 텍스트")]
    private Text coinText;

    [SerializeField]
    [Tooltip("보석 재화 텍스트")]
    private Text jemText;

    [Header("그 외")]
    private BigInteger[] goodsRequiredForUpgrade = new BigInteger[(int)UpgradeableBasicStats.UpgradeableBasicStatsNumber];

    [SerializeField]
    private GameObject player;

    private GameManager gmInstance;

    void Start()
    {
        StartSetting();
        basicStatLevelText[(int)UpgradeableBasicStats.Damage].text = $"Lv {GameManager.Instance.statsLevel[(int)UpgradeableBasicStats.Damage]}";
        StartCoroutine(customerOnTheWay());
    }

    void Update()
    {
        SaleOfFoodViewMaterialsText_BasicScreen();
        TextSettings();
    }

    private void TextSettings()
    {
        coinText.text = $"{ConvertGoodsToString(gmInstance.MoneyUnit)}";
        jemText.text = $"{gmInstance.GemUnit}";
    }

    private void StartSetting()
    {
        gmInstance = GameManager.Instance;

        cookingCount = 1;
        NowFoodIndex = 0;

        nowCookingFoodNameText.text = foodDatas[NowFoodIndex].FoodName;
        nowCookingCountText.text = $"{cookingCount} 개";

        for (int nowDataIndex = 0; nowDataIndex < quantityOfMaterials.Length; nowDataIndex++)
        {
            quantityOfMaterials[nowDataIndex] = foodDatas[NowFoodIndex].quantityOfMaterials[nowDataIndex];
        }

        for (int nowIndex = 0; nowIndex < goodsRequiredForUpgrade.Length; nowIndex++)
        {
            string goodsRequiredForUpgradeString;

            goodsRequiredForUpgrade[nowIndex] = 2;

            basicStatLevelText[nowIndex].text = $"Lv {gmInstance.statsLevel[nowIndex]}"; //레벨 텍스트 수정

            goodsRequiredForUpgradeString = ConvertGoodsToString(goodsRequiredForUpgrade[nowIndex]);

            goodsTextRequiredForUpgrade[nowIndex].text = $"강화\n{goodsRequiredForUpgradeString}원";
        }
    }

    private IEnumerator customerOnTheWay()
    {
        customerObj.transform.position = new Vector2(-3.5f, -3.2f);
        while (customerObj.transform.position.x < -1)
        {
            customerObj.transform.position += customerSpeed * Time.deltaTime;
            if (isCustomerArrival)
            {
                break;
            }
            yield return null;
        }
        isCustomerArrival = true;
        customerObj.transform.position = new Vector2(-1, -3.15f);
    }

    private void SaleOfFoodViewMaterialsText_BasicScreen()
    {
        if (nowContents == Contents.SaleOfFoodContents)
        {
            var battleSceneManagerIn = BattleSceneManager.Instance;
            for (int nowIndex = 0; nowIndex < 3; nowIndex++)
            {
                if (nowSaleOfFoodContents == SaleOfFoodContents.BasicScreen)
                {
                    materialsText_BasicScreen[nowIndex].text = $"{battleSceneManagerIn.quantityOfMaterials[nowIndex]} 개";
                }
                else if(nowSaleOfFoodContents == SaleOfFoodContents.ChooseFoodScreen)
                {
                    materialsText_ChooseCookScreen[nowIndex].text = $"{battleSceneManagerIn.quantityOfMaterials[nowIndex]} / {quantityOfMaterials[nowIndex] * cookingCount}";
                    
                    materialsText_ChooseCookScreen[nowIndex].color = (battleSceneManagerIn.quantityOfMaterials[nowIndex] < quantityOfMaterials[nowIndex] * cookingCount)
                        ? materialsText_ChooseCookScreen[nowIndex].color = redTextColor : materialsText_ChooseCookScreen[nowIndex].color = greenTextColor;

                    isMeetingTheNumberOfMaterials[nowIndex] = (battleSceneManagerIn.quantityOfMaterials[nowIndex] < quantityOfMaterials[nowIndex] * cookingCount)
                        ? isMeetingTheNumberOfMaterials[nowIndex] = true : isMeetingTheNumberOfMaterials[nowIndex] = false;
                }
            }
        }
    }

    public void ChangeTheFoodType(bool isChangeNextFoodType)
    {
        int maxFoodIndex = 2;
        if (NowFoodIndex == maxFoodIndex && isChangeNextFoodType == false)
        {
            NowFoodIndex = 0;
        }
        else if (NowFoodIndex == 0 && isChangeNextFoodType)
        {
            NowFoodIndex = maxFoodIndex;
        }
        else
        {
            NowFoodIndex = isChangeNextFoodType ? NowFoodIndex - 1 : NowFoodIndex + 1;
        }
        ChangeFoodAnim(isChangeNextFoodType);
        nowCookingFoodNameText.text = foodDatas[NowFoodIndex].FoodName;
        for (int nowDataIndex = 0; nowDataIndex < quantityOfMaterials.Length; nowDataIndex++)
        {
            quantityOfMaterials[nowDataIndex] = foodDatas[NowFoodIndex].quantityOfMaterials[nowDataIndex];
        }
    }

    private void ChangeFoodAnim(bool isChangeNextFoodType)
    {
        for (int nowIndex = 0; nowIndex < foodObjectsToCooking.Length; nowIndex++)
        {
            foodObjectsToCooking[nowIndex].GetComponent<FoodObj>().FoodMovingAnimStart(isChangeNextFoodType);
        }
    }

    public void FoodChooseAndMakePanelOnOrOff(bool isPanelOn)
    {
        if (isPanelOn && isCustomerArrival == false)
        {
            return;
        }
        nowSaleOfFoodContents = isPanelOn ? SaleOfFoodContents.ChooseFoodScreen : SaleOfFoodContents.BasicScreen;
        foodChooseAndMakePanelObj.SetActive(isPanelOn);
    }

    public void CookingPanelOn()
    {
        for (int nowIndex = 0; nowIndex < isMeetingTheNumberOfMaterials.Length; nowIndex++)
        {
            if (isMeetingTheNumberOfMaterials[nowIndex])
            {
                return;
            }
        }

        for (int nowIndex = 0; nowIndex < isMeetingTheNumberOfMaterials.Length; nowIndex++)
        {
            BattleSceneManager.Instance.quantityOfMaterials[nowIndex] -= quantityOfMaterials[nowIndex] * cookingCount;
        }

        nowSaleOfFoodContents = SaleOfFoodContents.FoodMakingScreen;
        chooseADishObj.SetActive(false);
        theProductionObj.SetActive(true);
        StartCoroutine(ArrowMiniGameStart());
    }

    public void StopArrow() => isArrowMoving = false;

    IEnumerator ArrowMiniGameStart()
    {
        Vector2 arrowMoveSpeed = new Vector2(4f, 0);
        bool isLeft = false;
        isArrowMoving = true;

        while (isArrowMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            arrowObj.transform.position += isLeft ? (Vector3)arrowMoveSpeed * -Time.deltaTime : (Vector3)arrowMoveSpeed * Time.deltaTime;
            if (isLeft && arrowObj.transform.position.x <= -1.5f)
            {
                isLeft = false;
            }
            else if (isLeft == false && arrowObj.transform.position.x >= 1.5f)
            {
                isLeft = true;
            }
            yield return null;
        }
        yield return new WaitForSeconds(5);
        theProductionObj.SetActive(false);
        isCustomerArrival = false;
        foodChooseAndMakePanelObj.SetActive(false);
        chooseADishObj.SetActive(true);
        nowSaleOfFoodContents = SaleOfFoodContents.BasicScreen;
        nowContents = Contents.SaleOfFoodContents;
        StartCoroutine(customerOnTheWay());
    }

    public void AdjustTheNumberOfFoods(bool isPlus)
    {
        if (isPlus == false && cookingCount > 1)
        {
            cookingCount--;
        }
        else if(isPlus && cookingCount < 99)
        {
            cookingCount++;
        }
        nowCookingCountText.text = $"{cookingCount} 개";
    }

    public void BasicStatUpgrade(int statsToUpgradeCurrently) //스탯 업그레이드 함수
    {
        var playerComponent = player.GetComponent<Player>();
        string goodsRequiredForUpgradeString;

        if (gmInstance.MoneyUnit < goodsRequiredForUpgrade[statsToUpgradeCurrently])
        {
            return;
        }
        else
        {
            gmInstance.MoneyUnit -= goodsRequiredForUpgrade[statsToUpgradeCurrently];
        }

        gmInstance.statsLevel[statsToUpgradeCurrently]++; //레벨 증가

        basicStatLevelText[statsToUpgradeCurrently].text = $"Lv {gmInstance.statsLevel[statsToUpgradeCurrently]}"; //레벨 텍스트 수정

        goodsRequiredForUpgrade[statsToUpgradeCurrently] += goodsRequiredForUpgrade[statsToUpgradeCurrently] / 2; //강화 비용 수정(임시 연산)

        goodsRequiredForUpgradeString = ConvertGoodsToString(goodsRequiredForUpgrade[statsToUpgradeCurrently]);

        goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text = $"강화\n{goodsRequiredForUpgradeString}원";

        //if (statsToUpgradeCurrently == (int)UpgradeableBasicStats.Damage)
        //{
        //    playerComponent.AttackPower++; //공격력 증가(임시 연산)
        //    basicStatFigureText[statsToUpgradeCurrently].text = $"{playerComponent.AttackPower}";
        //}
        //else
        //{
        //    basicStatFigureText[statsToUpgradeCurrently].text = $"{gmInstance.statsLevel[statsToUpgradeCurrently]}";
        //}
        switch (statsToUpgradeCurrently)
        {
            case (int)UpgradeableBasicStats.Damage:
                playerComponent.AttackPower++; //공격력 증가(임시 연산)
                basicStatFigureText[statsToUpgradeCurrently].text = $"{playerComponent.AttackPower}";
                break;
            case (int)UpgradeableBasicStats.MaxHp:
                basicStatFigureText[statsToUpgradeCurrently].text = $"{gmInstance.statsLevel[statsToUpgradeCurrently]}";
                break;
            case (int)UpgradeableBasicStats.Healing:
                basicStatFigureText[statsToUpgradeCurrently].text = $"{gmInstance.statsLevel[statsToUpgradeCurrently]}";
                break;
            case (int)UpgradeableBasicStats.AttackSpeed:
                basicStatFigureText[statsToUpgradeCurrently].text = $"{gmInstance.statsLevel[statsToUpgradeCurrently]}";
                break;
            case (int)UpgradeableBasicStats.FatalAttackDamage:
                basicStatFigureText[statsToUpgradeCurrently].text = $"{gmInstance.statsLevel[statsToUpgradeCurrently]}%";
                break;
            case (int)UpgradeableBasicStats.FatalAttackProbability:
                basicStatFigureText[statsToUpgradeCurrently].text = $"{gmInstance.statsLevel[statsToUpgradeCurrently]}%";
                break;
        }
    }

    public void AnotherContentsPopUp(GameObject PopUpObj)
    {
        //팝업 다트윈 애니메이션 넣기
        GameObject lastViewContents = PopUpObj;
        if (PopUpObj.activeSelf)
        {
            nowContentsPanelObj = contentsPanelObjs[(int)Contents.StatUpgradeContents];
            nowContents = Contents.StatUpgradeContents;
        }
        else
        {
            nowContents = (Contents)nowChangeContents;
            nowContentsPanelObj = PopUpObj;
        }
        lastViewContents.SetActive(false);
        nowContentsPanelObj.SetActive(true);
        if (isCustomerArrival == false)
        {
            isCustomerArrival = true;
        }
    }

    public void NowContentsChange(int ChangeIndex)
    {
        nowChangeContents = ChangeIndex;
    }

    public void AnotherContentsChangeScrollView(GameObject PopUpObj)
    {
        GameObject lastViewContents = PopUpObj;
        if (PopUpObj.activeSelf)
        {
            nowContentsPanelObj = contentsPanelObjs[(int)Contents.StatUpgradeContents];
            nowContents = Contents.StatUpgradeContents;
        }
        else
        {
            nowContentsPanelObj = PopUpObj;
            nowContents = (Contents)nowChangeContents;
        }
        lastViewContents.SetActive(false);
        nowContentsPanelObj.SetActive(true);
        if (isCustomerArrival == false)
        {
            isCustomerArrival = true;
        }
    }

    public string ConvertGoodsToString(BigInteger theValueOfAGood) //리메이크한 재화 단위 표시
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
