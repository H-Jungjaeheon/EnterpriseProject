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
    #region ���� ���� ǥ�� ���� ����
    private Contents nowContents;

    private SaleOfFoodContents nowSaleOfFoodContents;
    #endregion

    #region ������ â ���� ������
    [Header("������ â ���� ������")]
    [SerializeField]
    [Tooltip("���� �������� ������ â ������Ʈ")]
    private GameObject nowContentsPanelObj;

    [SerializeField]
    [Tooltip("������ â ������Ʈ ����")]
    private GameObject[] contentsPanelObjs;

    [SerializeField]
    [Tooltip("�丮 �Ǹ� �ý��� - �丮 ���� �� ���� â ������Ʈ")]
    private GameObject foodChooseAndMakePanelObj;

    [SerializeField]
    [Tooltip("�丮 �Ǹ� �ý��� - �丮 ���� â ������Ʈ")]
    private GameObject chooseADishObj;

    [SerializeField]
    [Tooltip("�丮 �Ǹ� �ý��� - �丮 ���� â ������Ʈ")]
    private GameObject theProductionObj;
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

    #region �丮 �Ǹ� �ý��� ȭ�� �ؽ�Ʈ, ����, ������Ʈ ����
    [Header("��� ���� ǥ�� �ؽ�Ʈ(�⺻ ȭ��)")]
    [SerializeField]
    [Tooltip("�⺻ ȭ���� ��� ��� ���� ǥ�� �ؽ�Ʈ�� ����")]
    private Text[] materialsText_BasicScreen;

    [Header("��� ���� ǥ�� �ؽ�Ʈ(�丮 ���� ȭ��)")]
    [SerializeField]
    [Tooltip("�丮 ���� ȭ���� ��� ��� ���� ǥ�� �ؽ�Ʈ�� ����")]
    private Text[] materialsText_ChooseCookScreen;

    [SerializeField]
    [Tooltip("���� ������ �丮 ���� ǥ�� �ؽ�Ʈ")]
    private Text nowCookingCountText;

    [SerializeField]
    [Tooltip("���� ������ �丮 �̸� ǥ�� �ؽ�Ʈ")]
    private Text nowCookingFoodNameText;

    private int nowFoodIndex;
    public int NowFoodIndex
    {
        get { return nowFoodIndex; }
        set { nowFoodIndex = value; }
    }

    private int cookingCount; //���� �丮 ����

    private int nowChangeContents; //�ٲ� ������ â �ε���

    [Header("���� �丮�� �ʿ��� ��� ������")]
    [SerializeField]
    [Tooltip("���� �丮�� �ʿ��� ��� ����")]
    private int[] quantityOfMaterials;

    private bool[] isMeetingTheNumberOfMaterials = new bool[3];

    private bool isArrowMoving;

    [HideInInspector]
    public bool isCustomerArrival;

    [SerializeField]
    [Tooltip("�մ� ������Ʈ")]
    private GameObject customerObj;

    [SerializeField]
    [Tooltip("������ ���� ������Ʈ")]
    private GameObject[] foodObjectsToCooking;

    [SerializeField]
    [Tooltip("�̴ϰ��� ȭ��ǥ ������Ʈ")]
    private GameObject arrowObj;

    private Vector3 customerSpeed = new Vector3(1, 0, 0);

    Color redTextColor = new Color(1, 0, 0);

    Color greenTextColor = new Color(0, 1, 0.03f);

    [SerializeField]
    [Tooltip("�� ���ĵ� �ʿ� ���, ���� �̸�")]
    private FoodData[] foodDatas;
    #endregion

    [Header("�� ��")]
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
        nowCookingCountText.text = $"{cookingCount} ��";

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
                    materialsText_BasicScreen[nowIndex].text = $"{battleSceneManagerIn.quantityOfMaterials[nowIndex]} ��";
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
        nowCookingCountText.text = $"{cookingCount} ��";
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

        CalculationOfGoods(gmInstance.MoneyUnit, damageGoodsRequiredForUpgrade, basicStatFigureText[statsToUpgradeCurrently], false); //��ȭ �谨
        gmInstance.statsLevel[statsToUpgradeCurrently]++; //���� ����
        basicStatLevelText[statsToUpgradeCurrently].text = $"Lv {gmInstance.statsLevel[statsToUpgradeCurrently]}"; //���� �ؽ�Ʈ ����

        for (int nowIndex = unitMaximumArrayIndex; nowIndex >= 0; nowIndex--) //��� ����(����)
        {
            if (damageGoodsRequiredForUpgrade[nowIndex] > 0)
            {
                nextGoodsRequiredForUpgrade[nowIndex]++;
                break;
            }
        }

        CalculationOfGoods(damageGoodsRequiredForUpgrade, nextGoodsRequiredForUpgrade, goodsTextRequiredForUpgrade[statsToUpgradeCurrently], true); //���׷��̵� ��� ����
        goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text = $"��ȭ\n{goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text}��";

        for (int nowIndex = unitMaximumArrayIndex; nowIndex >= 0; nowIndex--) //��� ����(����)
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
        //�˾� ��Ʈ�� �ִϸ��̼� �ֱ�
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

    public void CalculationOfGoods(int[] aCalculatedValues, int[] aPriceToAdd, Text commodityConversionText, bool isAddition) //�� ������ ���� ���� ���� ��
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
