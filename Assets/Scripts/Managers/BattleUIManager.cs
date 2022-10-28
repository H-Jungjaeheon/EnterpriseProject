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
    #region ���� ���� ǥ�� ���� ����
    private Contents nowContents;

    private SaleOfFoodContents nowSaleOfFoodContents;
    #endregion

    #region ������ â ���� ������
    [Header("������ â ���� ������")]

    [SerializeField]
    [Tooltip("���� �������� ������ â ������Ʈ")]
    private GameObject nowContentsPanelObj;

  
    [Tooltip("������ â ������Ʈ ����")]
    public GameObject[] contentsPanelObjs;

    [SerializeField]
    [Tooltip("�丮 �Ǹ� �ý��� - �丮 ���� �� ���� â ������Ʈ")]
    private GameObject foodChooseAndMakePanelObj;

    [SerializeField]
    [Tooltip("�丮 �Ǹ� �ý��� - �丮 ���� â ������Ʈ")]
    private GameObject chooseADishObj;

    [SerializeField]
    [Tooltip("�丮 �Ǹ� �ý��� - �丮 ���� â ������Ʈ")]
    private GameObject theProductionObj;

    [SerializeField]
    [Tooltip("���� �ý��� - ���� �ý��� â ������Ʈ")]
    private GameObject colleagueSystemPanelObj;
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

    [SerializeField]
    [Tooltip("���� ��ȭ �ؽ�Ʈ")]
    private Text coinText;

    [SerializeField]
    [Tooltip("���� ��ȭ �ؽ�Ʈ")]
    private Text jemText;

    [Header("�� ��")]
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
        nowCookingCountText.text = $"{cookingCount} ��";

        for (int nowDataIndex = 0; nowDataIndex < quantityOfMaterials.Length; nowDataIndex++)
        {
            quantityOfMaterials[nowDataIndex] = foodDatas[NowFoodIndex].quantityOfMaterials[nowDataIndex];
        }

        for (int nowIndex = 0; nowIndex < goodsRequiredForUpgrade.Length; nowIndex++)
        {
            string goodsRequiredForUpgradeString;

            goodsRequiredForUpgrade[nowIndex] = 2;

            basicStatLevelText[nowIndex].text = $"Lv {gmInstance.statsLevel[nowIndex]}"; //���� �ؽ�Ʈ ����

            goodsRequiredForUpgradeString = ConvertGoodsToString(goodsRequiredForUpgrade[nowIndex]);

            goodsTextRequiredForUpgrade[nowIndex].text = $"��ȭ\n{goodsRequiredForUpgradeString}��";
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
        nowCookingCountText.text = $"{cookingCount} ��";
    }

    public void BasicStatUpgrade(int statsToUpgradeCurrently) //���� ���׷��̵� �Լ�
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

        gmInstance.statsLevel[statsToUpgradeCurrently]++; //���� ����

        basicStatLevelText[statsToUpgradeCurrently].text = $"Lv {gmInstance.statsLevel[statsToUpgradeCurrently]}"; //���� �ؽ�Ʈ ����

        goodsRequiredForUpgrade[statsToUpgradeCurrently] += goodsRequiredForUpgrade[statsToUpgradeCurrently] / 2; //��ȭ ��� ����(�ӽ� ����)

        goodsRequiredForUpgradeString = ConvertGoodsToString(goodsRequiredForUpgrade[statsToUpgradeCurrently]);

        goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text = $"��ȭ\n{goodsRequiredForUpgradeString}��";

        //if (statsToUpgradeCurrently == (int)UpgradeableBasicStats.Damage)
        //{
        //    playerComponent.AttackPower++; //���ݷ� ����(�ӽ� ����)
        //    basicStatFigureText[statsToUpgradeCurrently].text = $"{playerComponent.AttackPower}";
        //}
        //else
        //{
        //    basicStatFigureText[statsToUpgradeCurrently].text = $"{gmInstance.statsLevel[statsToUpgradeCurrently]}";
        //}
        switch (statsToUpgradeCurrently)
        {
            case (int)UpgradeableBasicStats.Damage:
                playerComponent.AttackPower++; //���ݷ� ����(�ӽ� ����)
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

    public string ConvertGoodsToString(BigInteger theValueOfAGood) //������ũ�� ��ȭ ���� ǥ��
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
