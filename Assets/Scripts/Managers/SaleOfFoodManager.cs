using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum SaleOfFoodContents
{
    BasicScreen,
    ChooseFoodScreen,
    FoodMakingScreen
}

public class SaleOfFoodManager : MonoBehaviour
{
    private SaleOfFoodContents nowSaleOfFoodContents;

    [SerializeField]
    [Tooltip("�丮 �Ǹ� �ý��� - �丮 ���� �� ���� â ������Ʈ")]
    private GameObject foodChooseAndMakePanelObj;

    [SerializeField]
    [Tooltip("�丮 �Ǹ� �ý��� - �丮 ���� â ������Ʈ")]
    private GameObject chooseADishObj;

    [SerializeField]
    [Tooltip("�丮 �Ǹ� �ý��� - �丮 ���� â ������Ʈ")]
    private GameObject theProductionObj;

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

    [SerializeField]
    [Tooltip("ȭ��ǥ ������Ʈ")]
    private Arrow arrowComponent;

    private Vector3 customerSpeed = new Vector3(1, 0, 0);

    Color redTextColor = new Color(1, 0, 0);

    Color greenTextColor = new Color(0, 1, 0.03f);

    [SerializeField]
    [Tooltip("�� ���ĵ� �ʿ� ���, ���� �̸�")]
    private FoodData[] foodDatas;
    #endregion

    #region ��� ���� ���� ��� ����
    [SerializeField]
    [Tooltip("��� ���ҽ���")]
    private Sprite[] ingredientSprite;

    [SerializeField]
    [Tooltip("��� �̹���")]
    private Image ingredientImage;

    [SerializeField]
    [Tooltip("���� ������Ʈ")]
    private GameObject potObj;

    Color ingredientColor = new Color(1, 1, 1, 0);
    #endregion

    void Start()
    {
        cookingCount = 1;
        NowFoodIndex = 0;

        nowCookingFoodNameText.text = foodDatas[NowFoodIndex].FoodName;
        nowCookingCountText.text = $"{cookingCount} ��";

        for (int nowDataIndex = 0; nowDataIndex < quantityOfMaterials.Length; nowDataIndex++)
        {
            quantityOfMaterials[nowDataIndex] = foodDatas[NowFoodIndex].quantityOfMaterials[nowDataIndex];
        }

        StartCoroutine(customerOnTheWay());
    }

    // Update is called once per frame
    void Update()
    {
        SaleOfFoodViewMaterialsText_BasicScreen();
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
        if (BattleUIManager.Instance.nowContents == Contents.SaleOfFoodContents)
        {
            var battleSceneManagerIn = BattleSceneManager.Instance;
            for (int nowIndex = 0; nowIndex < 3; nowIndex++)
            {
                if (nowSaleOfFoodContents == SaleOfFoodContents.BasicScreen)
                {
                    materialsText_BasicScreen[nowIndex].text = $"{battleSceneManagerIn.quantityOfMaterials[nowIndex]} ��";
                }
                else if (nowSaleOfFoodContents == SaleOfFoodContents.ChooseFoodScreen)
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
        int basicScore = 2;
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

        ingredientImage.sprite = ingredientSprite[Random.Range(0, 4)];

        for (int nowIndex = 0; nowIndex < 2; nowIndex++)
        {
            ingredientImage.transform.DOLocalMoveY(900, 0);
            float nowAlpha = 0;
            while (nowAlpha < 1)
            {
                nowAlpha += Time.deltaTime;
                ingredientColor.a = nowAlpha;
                ingredientImage.color = ingredientColor;
                yield return null;
            }

            ingredientImage.transform.DOLocalMoveY(420, 0.5f).SetEase(Ease.InBack);

            while (ingredientImage.transform.localPosition.y > 420)
            {
                yield return null;
            }

            potObj.transform.DOScale(new Vector3(1.08f, 1.08f, 1), 0.2f);
            yield return new WaitForSeconds(0.2f);
            potObj.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
            yield return new WaitForSeconds(0.7f);

        }

        yield return new WaitForSeconds(1);

        GameManager.Instance.CurrentProficiency += (basicScore * arrowComponent.multiplication);

        theProductionObj.SetActive(false);
        isCustomerArrival = false;
        foodChooseAndMakePanelObj.SetActive(false);
        chooseADishObj.SetActive(true);
        nowSaleOfFoodContents = SaleOfFoodContents.BasicScreen;
        BattleUIManager.Instance.nowContents = Contents.SaleOfFoodContents;
        StartCoroutine(customerOnTheWay());
    }

    public void AdjustTheNumberOfFoods(bool isPlus)
    {
        if (isPlus == false && cookingCount > 1)
        {
            cookingCount--;
        }
        else if (isPlus && cookingCount < 99)
        {
            cookingCount++;
        }
        nowCookingCountText.text = $"{cookingCount} ��";
    }
}
