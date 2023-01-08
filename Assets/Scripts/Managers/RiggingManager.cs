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
    [Tooltip("장비 ID")]
    public int id;

    [Tooltip("장비 종류")]
    public RiggingType type;

    [Tooltip("장비 잠금해제 여부")]
    public bool isUnlock;

    [Tooltip("장비 아이콘")]
    public Sprite icon;

    [Tooltip("장비 이름")]
    public string name;

    [Tooltip("장비 설명")]
    public string explanation;

    [Tooltip("장비 등급")]
    public int rank;

    [Tooltip("장비 레벨")]
    public int level;

    [Tooltip("최대 장비 경험치")]
    public int maxExp;

    [Tooltip("현재 장비 경험치")]
    public int nowExp;
}

public class RiggingManager : MonoBehaviour
{
    [Tooltip("장비 데이터들")]
    public Rigging[] riggingDatas;

    [Tooltip("현재 장착한 장비의 데이터 인덱스")]
    public int nowDataIndex;

    [SerializeField]
    [Tooltip("현재 장착한 장비 아이콘 표시 이미지")]
    private Image[] riggingImages;

    #region 장비 장착 버튼 요소들
    [Header("장비 장착 버튼 요소들")]

    [SerializeField]
    [Tooltip("장비 장착 버튼들")]
    private Button[] equipButtons;

    [SerializeField]
    [Tooltip("장비 장착 버튼 이미지들")]
    private Image[] buttonImage;

    [SerializeField]
    [Tooltip("장비 장착 버튼 빈 스프라이트")]
    private Sprite nullSprite;
    #endregion

    #region 장비 장착 화면 요소들 모음
    [Header("장비 장착 화면 요소들")]

    [SerializeField]
    [Tooltip("장비 장착 화면 오브젝트")]
    private GameObject panelObj;

    [SerializeField]
    [Tooltip("장비 아이콘 이미지")]
    private Image iconImage;

    [SerializeField]
    [Tooltip("장비 이름 텍스트")]
    private Text nameText;

    [SerializeField]
    [Tooltip("장비 등급 텍스트")]
    private Text rankText;

    [SerializeField]
    [Tooltip("장비 부위 텍스트")]
    private Text partText;

    [SerializeField]
    [Tooltip("장비 설명 텍스트")]
    private Text explanationText;

    [SerializeField]
    [Tooltip("현재 장비 인벤토리 안내 이미지")]
    private Image guideImg;

    [SerializeField]
    [Tooltip("장비 인벤토리 안내 스프라이트 모음")]
    private Sprite[] guideSprites;

    private const string shoesType = "신발";

    private const string bowlType = "그릇";

    private const string spoonType = "숟가락";

    Rigging nowData;
    #endregion

    /// <summary>
    /// 장비 장착 화면 활성화 시 신발 종류의 장비들로 버튼 세팅
    /// </summary>
    private void OnEnable()
    {
        ButtonSetting(0);
    }

    /// <summary>
    /// 장비 장착 버튼들 장비 종류 변경(버튼)
    /// </summary>
    /// <param name="typeIndex"> 장비 종류 인덱스(RiggingType) </param>
    public void ButtonSetting(int typeIndex)
    {
        RiggingType riggingType = (RiggingType)typeIndex; //현재 장비 타입
        int buttonIndex = 0; //현재 버튼 인덱스

        guideImg.sprite = guideSprites[typeIndex]; //현재 장비 타입에 맞는 안내 이미지로 변경

        for (int nowIndex = equipButtons.Length - 1; nowIndex >= 0; nowIndex--) //마지막 데이터부터 0번째 데이터까지 반복 (버튼 이벤트 초기화)
        {
            equipButtons[nowIndex].onClick.RemoveAllListeners();
            buttonImage[nowIndex].sprite = nullSprite;
        }

        for (int nowIndex = riggingDatas.Length - 1; nowIndex >= 0; nowIndex--) //마지막 데이터부터 0번째 데이터까지 반복 (버튼 이벤트 추가)
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
    /// 장비 설명 및 장착 화면 활성화
    /// </summary>
    /// <param name="dataIndex"> 현재 장비 데이터 인덱스 </param>
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
    /// 장비 장착 함수(버튼)
    /// </summary>
    public void EquipRigging()
    {
        nowData = riggingDatas[nowDataIndex];

        riggingImages[(int)nowData.type].sprite = nowData.icon;

        QuestionPanelClose();
    }

    /// <summary>
    /// 장비 설명 및 장착 화면 비활성화(버튼)
    /// </summary>
    public void QuestionPanelClose() => panelObj.SetActive(false);
}
