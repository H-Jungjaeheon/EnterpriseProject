using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum Contents
{
    StatUpgradeContents,
    SaleOfFoodContents,
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
    MaxHp
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

    [SerializeField]
    [Tooltip("콘텐츠 창 오브젝트 모음")]
    private GameObject[] contentsPanelObjs;

    [SerializeField]
    [Tooltip("요리 판매 시스템 - 요리 선택 및 제작 창 오브젝트")]
    private GameObject foodChooseAndMakePanelObj;

    [SerializeField]
    [Tooltip("요리 판매 시스템 - 요리 선택 창 오브젝트")]
    private GameObject chooseADishObj;

    [SerializeField]
    [Tooltip("요리 판매 시스템 - 요리 제작 창 오브젝트")]
    private GameObject theProductionObj;
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

    [Header("그 외")]
    [SerializeField]
    private int[] damageGoodsRequiredForUpgrade = new int[25];

    [SerializeField]
    private GameObject Player;

    void Start()
    {
        StartSetting();
        basicStatLevelText[(int)UpgradeableBasicStats.Damage].text = $"Lv {GameManager.Instance.statsLevel[(int)UpgradeableBasicStats.Damage]}";
        StartCoroutine(customerOnTheWay());
    }

    void Update()
    {
        SaleOfFoodViewMaterialsText_BasicScreen();
    }

    private void StartSetting()
    {
        cookingCount = 1;
        NowFoodIndex = 0;

        nowCookingFoodNameText.text = foodDatas[NowFoodIndex].FoodName;
        nowCookingCountText.text = $"{cookingCount} 개";

        for (int nowDataIndex = 0; nowDataIndex < quantityOfMaterials.Length; nowDataIndex++)
        {
            quantityOfMaterials[nowDataIndex] = foodDatas[NowFoodIndex].quantityOfMaterials[nowDataIndex];
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
        if (NowFoodIndex == maxFoodIndex && isChangeNextFoodType)
        {
            NowFoodIndex = 0;
        }
        else if (NowFoodIndex == 0 && isChangeNextFoodType == false)
        {
            NowFoodIndex = maxFoodIndex;
        }
        else
        {
            NowFoodIndex = isChangeNextFoodType ? NowFoodIndex + 1 : NowFoodIndex - 1;
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

    public void BasicStatUpgrade(int statsToUpgradeCurrently)
    {
        int unitMaximumArrayIndex = 24;
        var gmInstance = GameManager.Instance;
        int largestIndex = 0;
        int[] nextGoodsRequiredForUpgrade = new int[25];
        int[] statsForUpgrade = new int[25];
        var playerComponent = Player.GetComponent<Player>();

        for (int nowIndex = 0; nowIndex < gmInstance.MoneyUnit.Length; nowIndex++)
        {
            if (gmInstance.MoneyUnit[nowIndex] > 0)
            {
                largestIndex = nowIndex;
            }
        }


        for (int nowIndex = unitMaximumArrayIndex; nowIndex >= 0; nowIndex--)
        {
            if (nowIndex > largestIndex && gmInstance.MoneyUnit[nowIndex] > damageGoodsRequiredForUpgrade[nowIndex])
            {
                break;
            }
            else if(nowIndex <= largestIndex)
            {
                if (gmInstance.MoneyUnit[nowIndex] < damageGoodsRequiredForUpgrade[nowIndex])
                {
                    return;
                }
                else if (gmInstance.MoneyUnit[nowIndex] > damageGoodsRequiredForUpgrade[nowIndex])
                {
                    break;
                }
                else if (nowIndex == 0 && gmInstance.MoneyUnit[nowIndex] == damageGoodsRequiredForUpgrade[nowIndex])
                {
                    break;
                }
            }
        }

        CalculationOfGoods(gmInstance.MoneyUnit, damageGoodsRequiredForUpgrade, basicStatFigureText[statsToUpgradeCurrently], false); //재화 삭감
        gmInstance.statsLevel[statsToUpgradeCurrently]++; //레벨 증가
        basicStatLevelText[statsToUpgradeCurrently].text = $"Lv {gmInstance.statsLevel[statsToUpgradeCurrently]}"; //레벨 텍스트 수정

        for (int nowIndex = unitMaximumArrayIndex; nowIndex >= 0; nowIndex--) //비용 수정(연산)
        {
            if (damageGoodsRequiredForUpgrade[nowIndex] > 0)
            {
                nextGoodsRequiredForUpgrade[nowIndex]++;
                break;
            }
        }

        CalculationOfGoods(damageGoodsRequiredForUpgrade, nextGoodsRequiredForUpgrade, goodsTextRequiredForUpgrade[statsToUpgradeCurrently], true); //업그레이드 비용 수정
        goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text = $"강화\n{goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text}원";

        for (int nowIndex = unitMaximumArrayIndex; nowIndex >= 0; nowIndex--) //비용 수정(연산)
        {
            if (playerComponent.AttackPower[nowIndex] > 0)
            {
                statsForUpgrade[nowIndex]++;
                break;
            }
        }

        CalculationOfGoods(playerComponent.AttackPower, statsForUpgrade, basicStatFigureText[statsToUpgradeCurrently], true);
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

    public void CalculationOfGoods(int[] aCalculatedValues, int[] aPriceToAdd, Text commodityConversionText, bool isAddition) //뺄 때에는 가장 높은 단위 비교
    {
        int maxUnitIndex = 0;
        for (int nowUnitOfGoodsIndex = 0; nowUnitOfGoodsIndex < aCalculatedValues.Length; nowUnitOfGoodsIndex++)
        {
            if (isAddition)
            {
                aCalculatedValues[nowUnitOfGoodsIndex] += aPriceToAdd[nowUnitOfGoodsIndex];
            }
            else
            {
                aCalculatedValues[nowUnitOfGoodsIndex] -= aPriceToAdd[nowUnitOfGoodsIndex];
            }

            if (aCalculatedValues[nowUnitOfGoodsIndex] >= 1000)
            {
                aCalculatedValues[nowUnitOfGoodsIndex + 1] += aCalculatedValues[nowUnitOfGoodsIndex] / 1000;
                aCalculatedValues[nowUnitOfGoodsIndex] %= 1000;
            }
            else if(aCalculatedValues[nowUnitOfGoodsIndex] < 0)
            {
                aCalculatedValues[nowUnitOfGoodsIndex + 1] -= aCalculatedValues[nowUnitOfGoodsIndex] / 1000;
                aCalculatedValues[nowUnitOfGoodsIndex] %= 1000;

                if (aCalculatedValues[nowUnitOfGoodsIndex] < 0)
                {
                    aCalculatedValues[nowUnitOfGoodsIndex + 1]--;
                    aCalculatedValues[nowUnitOfGoodsIndex] = 1000 + aCalculatedValues[nowUnitOfGoodsIndex];
                }
            }

            if (aCalculatedValues[nowUnitOfGoodsIndex] > 0)
            {
                maxUnitIndex = nowUnitOfGoodsIndex;
            }
        }
        ConvertGoodsString(commodityConversionText, maxUnitIndex, aCalculatedValues);
    }

    private void ConvertGoodsString(Text commodityConversionText, int maxUnitIndex, int[] aCalculatedValues)
    {
        char aUnitOfGoods = '\0';
        if (maxUnitIndex != 0)
        {
            aUnitOfGoods = (char)(maxUnitIndex + 96);
        }

        if (maxUnitIndex != 0)
        {
            if (aCalculatedValues[maxUnitIndex] / 10 >= 10)
            {
                commodityConversionText.text = $"{aCalculatedValues[maxUnitIndex]}{aUnitOfGoods}";
            }
            else if (aCalculatedValues[maxUnitIndex] / 10 > 0)
            {
                commodityConversionText.text = $"{aCalculatedValues[maxUnitIndex]}.{0 + aCalculatedValues[maxUnitIndex - 1] / 100}{aUnitOfGoods}";
            }
            else if (aCalculatedValues[maxUnitIndex] / 10 <= 0)
            {
                int hundredUnits = aCalculatedValues[maxUnitIndex - 1] / 100;
                int tenUnits = (aCalculatedValues[maxUnitIndex - 1] % 100) / 10;
                commodityConversionText.text = $"{aCalculatedValues[maxUnitIndex]}.{hundredUnits}{tenUnits}{aUnitOfGoods}";
            }
        }
        else
        {
            commodityConversionText.text = $"{aCalculatedValues[maxUnitIndex]}{aUnitOfGoods}";
        }
    }
}
