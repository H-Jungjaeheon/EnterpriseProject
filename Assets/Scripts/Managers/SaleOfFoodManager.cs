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

    #region 창 오브젝트 모음
    [Header("창 오브젝트 모음")]

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

    [Tooltip("최대 음식 인덱스")]
    const int MAX_FOOD_INDEX = 2;

    [Tooltip("현재 음식 종류 인덱스")]
    private int nowFoodIndex;
    
    public int NowFoodIndex
    {
        get { return nowFoodIndex; }
        set { nowFoodIndex = value; }
    }

    private int cookingCount; //현재 요리 개수

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

    [SerializeField]
    [Tooltip("화살표 컴포넌트")]
    private Arrow arrowComponent;

    [SerializeField]
    [Tooltip("냄비 오브젝트")]
    public GameObject potObj;

    [Tooltip("재료 리소스들(애니메이션에 사용)")]
    public Sprite[] foodResources;

    private Vector3 customerSpeed = new Vector3(1, 0, 0);

    [SerializeField]
    [Tooltip("각 음식들 필요 재료, 음식 이름")]
    private FoodData[] foodDatas;
    #endregion

    #region 재료 투입 연출 요소 모음
    [SerializeField]
    [Tooltip("재료 투입 애니메이션에 사용될 재료 오브젝트 컴포넌트")]
    private IngredientObj[] ingredientCom;

    WaitForSeconds inputDelay = new WaitForSeconds(0.15f);
    #endregion

    #region 결과 요소 모음
    [SerializeField]
    [Tooltip("결과 창 오브젝트")]
    private GameObject resultsObj;

    [SerializeField]
    [Tooltip("결과 창 이미지")]
    private Image resultsImage;

    [SerializeField]
    [Tooltip("결과 창 텍스트")]
    private Text resultsText;

    [SerializeField]
    [Tooltip("결과 창 스프라이트 모음")]
    private Sprite[] resultsSprite;
    #endregion

    [SerializeField]
    [Tooltip("퀘스트 시스템 매니저 컴포넌트")]
    private QuestManager questManager;

    void Start()
    {
        cookingCount = 1;
        NowFoodIndex = 0;

        nowCookingFoodNameText.text = foodDatas[NowFoodIndex].FoodName;
        nowCookingCountText.text = $"{cookingCount} 개";

        for (int nowDataIndex = 0; nowDataIndex < quantityOfMaterials.Length; nowDataIndex++)
        {
            quantityOfMaterials[nowDataIndex] = foodDatas[NowFoodIndex].quantityOfMaterials[nowDataIndex];
        }

        StartCoroutine(customerOnTheWay());
    }

    private void OnEnable()
    {
        customerObj.transform.position = new Vector2(-1, -3.15f);
    }

    void Update()
    {
        SaleOfFoodViewMaterialsText_BasicScreen();
    }

    private IEnumerator customerOnTheWay()
    {
        customerObj.transform.position = new Vector2(-3.5f, -3.15f);

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
        if (BattleUIManager.Instance.nowContents == Contents.SaleOfFood)
        {
            var battleSceneManagerIn = BattleSceneManager.Instance;
            for (int nowIndex = 0; nowIndex < 3; nowIndex++)
            {
                if (nowSaleOfFoodContents == SaleOfFoodContents.BasicScreen)
                {
                    materialsText_BasicScreen[nowIndex].text = $"{battleSceneManagerIn.quantityOfMaterials[nowIndex]} 개";
                }
                else if (nowSaleOfFoodContents == SaleOfFoodContents.ChooseFoodScreen)
                {
                    materialsText_ChooseCookScreen[nowIndex].text = $"{battleSceneManagerIn.quantityOfMaterials[nowIndex]} / {quantityOfMaterials[nowIndex] * cookingCount}";

                    materialsText_ChooseCookScreen[nowIndex].color = (battleSceneManagerIn.quantityOfMaterials[nowIndex] < quantityOfMaterials[nowIndex] * cookingCount)
                        ? materialsText_ChooseCookScreen[nowIndex].color = Color.red : materialsText_ChooseCookScreen[nowIndex].color = Color.green;

                    isMeetingTheNumberOfMaterials[nowIndex] = (battleSceneManagerIn.quantityOfMaterials[nowIndex] < quantityOfMaterials[nowIndex] * cookingCount)
                        ? isMeetingTheNumberOfMaterials[nowIndex] = true : isMeetingTheNumberOfMaterials[nowIndex] = false;
                }
            }
        }
    }

    /// <summary>
    /// 음식 종류(등급) 변경 함수
    /// </summary>
    /// <param name="isChangeNextFoodType"> 다음 인덱스의 음식으로 넘기는지 판별(false면 이전 인덱스의 음식으로 넘김) </param>
    public void ChangeTheFoodType(bool isChangeNextFoodType)
    {
        if (NowFoodIndex == MAX_FOOD_INDEX && isChangeNextFoodType == false)
        {
            NowFoodIndex = 0;
        }
        else if (NowFoodIndex == 0 && isChangeNextFoodType)
        {
            NowFoodIndex = MAX_FOOD_INDEX;
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

    /// <summary>
    /// 음식 선택 및 제작 콘텐츠 화면 활성화/비활성화
    /// </summary>
    /// <param name="isPanelOn"> 콘텐츠 화면 활성화 여부 </param>
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

    /// <summary>
    /// 화살표 움직임 정지
    /// </summary>
    public void StopArrow() => isArrowMoving = false;

    /// <summary>
    /// 요리제작 시스템 : 타이밍 맞추는 미니게임 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator ArrowMiniGameStart()
    {
        Vector2 arrowMoveSpeed = new Vector2(4f, 0);
        bool isLeft = false;
        int maxCount = 0;
        int nowGiveJewel = 0;

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

        yield return null;

        nowGiveJewel += (NowFoodIndex + 1) * 10 + //현재 음식 등급에 맞는 기본 지급 보석
            Random.Range(0, (10 * arrowComponent.multiplication) + 1) * //현재 요리 성공 등급에 맞는 지급 보석
            cookingCount; //현재 요리 개수에 따른 보상 배로 늘리기 
        
        switch (arrowComponent.multiplication)
        {
            case 1:
                maxCount = 1;
                break;
            case 2:
                maxCount = 3;
                break;
            case 3:
                maxCount = 5;
                break;
        }

        bool[] failIndex = new bool[5]; //성공도에 따라서 실패 애니메이션 재생할 음식 인덱스 배열
        bool isComplete; //중복 제거 완료 판별
        int randIndex;

        for (int nowIndex = 0; nowIndex < 5 - maxCount; nowIndex++) //중복 제거 작업
        {
            randIndex = Random.Range(0, 5); //처음 랜덤으로 뽑기
            isComplete = false;

            while (isComplete == false)
            {
                isComplete = true;

                for (int checkIndex = 0; checkIndex < 5; checkIndex++) //failIndex 배열 전체를 돌며 현재 뽑힌 값에 대한 중복 제거
                {
                    if (failIndex[randIndex] == true) //중복이라면, 다시 뽑기
                    {
                        randIndex = Random.Range(0, 5);
                        isComplete = false;
                    }
                }
            }

            failIndex[randIndex] = true; //중복이 아니라면, 현재 인덱스의 재료 애니메이션은 실패 애니메이션으로 저장
        }

        for (int nowIndex = 0; nowIndex < 5; nowIndex++) //재료 애니메이션 실행
        {
            if (failIndex[nowIndex] == false) //현재 인덱스의 이미지가 성공 애니메이션을 띄워야 하는 경우
            {
                ingredientCom[nowIndex].StartIngredientAnim(false);
            }
            else
            {
                ingredientCom[nowIndex].StartIngredientAnim(true);
            }
            yield return inputDelay;
        }

        yield return new WaitForSeconds(3);

        resultsText.text = $"보석 + {nowGiveJewel}";

        resultsObj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        resultsImage.sprite = resultsSprite[arrowComponent.multiplication - 1];

        yield return new WaitForSeconds(2.5f);

        GameManager.Instance.GemUnit += nowGiveJewel;

        theProductionObj.SetActive(false);
        resultsObj.transform.DOScale(Vector3.zero, 0);
        isCustomerArrival = false;
        foodChooseAndMakePanelObj.SetActive(false);
        chooseADishObj.SetActive(true);
        nowSaleOfFoodContents = SaleOfFoodContents.BasicScreen;
        
        BattleUIManager.Instance.nowContents = Contents.SaleOfFood;

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
        nowCookingCountText.text = $"{cookingCount} 개";
    }
}
