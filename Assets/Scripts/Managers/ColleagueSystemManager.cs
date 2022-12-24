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
        [Tooltip("잠금 해제 시 필요한 보석")]
        public int needGem;

        [Tooltip("잠금 해제 판별")]
        public bool isUnlock;

        [Tooltip("업그레이드 시 필요한 돈")]
        public BigInteger needGold;

        [Tooltip("동료 레벨")]
        public int level;

        [Tooltip("장착 버튼 오브젝트")]
        public GameObject buttonObj;

        [Tooltip("가격 표시 텍스트")]
        public Text priceText;

        [Tooltip("동료 아이콘")]
        public Sprite icon;

        [Tooltip("동료 데이터")]
        public PartnerData partnerData;
    }

    public ColleagueData[] colleagueDatas;

    [SerializeField]
    private Partner Partner;

    [SerializeField]
    [Tooltip("현재 장착중인 동료 아이콘")]
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
    /// 동료 시스템 창 활성화 시 세팅
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
    /// 초기 세팅
    /// </summary>
    private void StartSettings()
    {
        for (int nowIndex = 0; nowIndex < (int)ColleagueKind.ColleagueCount; nowIndex++)
        {
            colleagueDatas[nowIndex].needGold = 10;
        }
    }

    /// <summary>
    /// 동료 장착 실행(버튼)
    /// </summary>
    /// <param name="nowEquipColleagueIndex"> 현재 장착할 동료 인덱스 </param>
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
    /// 동료 업그레이드 or 잠금 해제 함수(버튼)
    /// </summary>
    /// <param name="nowColleagueIndex"> 현재 업그레이드 or 잠금 해제할 동료 인덱스 </param>
    public void ColleagueUpgradeOrUnlock(int nowColleagueIndex)
    {
        if (colleagueDatas[nowColleagueIndex].isUnlock == false && GameManager.Instance.GemUnit >= colleagueDatas[nowColleagueIndex].needGem) //해당 인덱스 동료 비 잠금해제의 경우
        {
            GameManager.Instance.GemUnit -= colleagueDatas[nowColleagueIndex].needGem; //해당 인덱스 동료 잠금 해제 비용 차감

            colleagueDatas[nowColleagueIndex].isUnlock = true; //해당 인덱스 동료 잠금 해제
                
            EquipButtonSetActive();
        }
        else if (colleagueDatas[nowColleagueIndex].isUnlock && GameManager.Instance.MoneyUnit >= colleagueDatas[nowColleagueIndex].needGold) //해당 인덱스 동료 잠금해제의 경우
        {
            GameManager.Instance.MoneyUnit -= colleagueDatas[nowColleagueIndex].needGold; //해당 인덱스 동료 업그레이드 비용 차감

            colleagueDatas[nowColleagueIndex].level++; //해당 인덱스 동료 레벨 증가

            colleagueDatas[nowColleagueIndex].needGold += colleagueDatas[nowColleagueIndex].needGold / 2; //해당 인덱스 동료 업그레이드 비용 증가
        }
        else
        {
            return;
        }

        colleagueDatas[nowColleagueIndex].priceText.text = $"업그레이드\n{colleagueDatas[nowColleagueIndex].needGold} 골드"; //해당 인덱스 동료 버튼 텍스트 수정
    }

    /// <summary>
    /// 텍스트 색 변경(텍스트 내용 변경)
    /// </summary>
    public void TextColorChange()
    {
        for (int nowIndex = 0; nowIndex < (int)ColleagueKind.ColleagueCount; nowIndex++)
        {
            if (colleagueDatas[nowIndex].isUnlock) //잠금 해제 완료 시
            {
                colleagueDatas[nowIndex].priceText.color = (colleagueDatas[nowIndex].needGold <= GameManager.Instance.MoneyUnit) ? greenTextColor : redTextColor;
                colleagueDatas[nowIndex].priceText.text = $"업그레이드\n{colleagueDatas[nowIndex].needGold} 골드"; //해당 인덱스 동료 버튼 텍스트 수정
            }
            else //잠금 해제 비 완료 시
            {
                colleagueDatas[nowIndex].priceText.color = (colleagueDatas[nowIndex].needGem <= GameManager.Instance.GemUnit) ? greenTextColor : redTextColor;
                colleagueDatas[nowIndex].priceText.text = $"구매\n{colleagueDatas[nowIndex].needGem} 젬"; //해당 인덱스 동료 버튼 텍스트 수정
            }
        }
    }

    /// <summary>
    /// 장착 버튼 활성화
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
