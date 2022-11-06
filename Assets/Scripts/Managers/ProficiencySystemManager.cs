using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProficiencySystemManager : Singleton<ProficiencySystemManager>
{
    [SerializeField]
    [Tooltip("현재 선택한 캐릭터 이름")]
    private string[] nowChooseCharacterName;

    [SerializeField]
    [Tooltip("현재 선택한 캐릭터 버프 내용")]
    private string[] nowChooseCharacterBuff;

    [SerializeField]
    [Tooltip("슬라이드할 배경(캐릭터) 렉트 트랜스폼")]
    private RectTransform[] slideableBgRectTransform;

    [SerializeField]
    [Tooltip("현재 캐릭터 이름 텍스트")]
    private Text nowChooseCharacterNameText;

    [SerializeField]
    [Tooltip("현재 캐릭터 버프 내용 텍스트")]
    private Text nowChooseCharacterBuffText;

    [SerializeField]
    [Tooltip("현재 캐릭터 업그레이드/잠금해제 버튼 텍스트")]
    private Text nowChooseCharacterUpgradeOrUnlockButtonText;

    private int minCharacterIndex;

    private int maxCharacterIndex;

    private int[] nowChooseCharacterUnlockCost;

    private int[] nowChooseCharacterUpgradeCost;

    public bool[] isNowChooseCharacterUnlock;

    private int[] nowChooseCharacterLevel;

    public int nowChooseCharacterIndex;

    private Vector3 dragStartMousePos;

    private bool isDraging;

    Color redTextColor = new Color(1, 0, 0);

    Color greenTextColor = new Color(0, 1, 0.03f);

    protected override void Awake()
    {
        base.Awake();
        minCharacterIndex = 0;
        maxCharacterIndex = nowChooseCharacterName.Length - 1;

        nowChooseCharacterUnlockCost = new int[nowChooseCharacterName.Length];
        nowChooseCharacterUpgradeCost = new int[nowChooseCharacterName.Length];
        isNowChooseCharacterUnlock = new bool[nowChooseCharacterName.Length];
        nowChooseCharacterLevel = new int[nowChooseCharacterName.Length];

        for (int nowIndex = minCharacterIndex; nowIndex <= maxCharacterIndex; nowIndex++)
        {
            nowChooseCharacterLevel[nowIndex] = 1;
            nowChooseCharacterUpgradeCost[nowIndex] = 4;
            nowChooseCharacterUnlockCost[nowIndex] = 4;
        }

        isNowChooseCharacterUnlock[minCharacterIndex] = true;
        
        TextReSettings();
    }

    private void OnEnable()
    {
        TextReSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isDraging == false)
        {
            isDraging = true;
            StartCoroutine(DragStart());
        }
    }

    IEnumerator DragStart()
    {
        Vector3 nowMousePos;

        dragStartMousePos = Input.mousePosition;

        while (true)
        {
            nowMousePos = Input.mousePosition;

            if (dragStartMousePos.x + 100 < nowMousePos.x)
            {
                StartCoroutine(Draging(true));
                break;
            }
            else if (dragStartMousePos.x - 100 > nowMousePos.x)
            {
                StartCoroutine(Draging(false));
                break;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDraging = false;
                break;
            }
            yield return null;
        }
    }

    IEnumerator Draging(bool isRightDrag)
    {
        int imageTargetPosX;

        imageTargetPosX = isRightDrag ? 1300 : -1300;

        slideableBgRectTransform[nowChooseCharacterIndex].DOAnchorPos(new Vector2(imageTargetPosX, 0), 0.3f);

        if (isRightDrag && nowChooseCharacterIndex == minCharacterIndex || isRightDrag == false && nowChooseCharacterIndex == maxCharacterIndex)
        {
            StartCoroutine(ImagePosInitialization(-imageTargetPosX, isRightDrag));
            yield break;
        }
        else
        {
            nowChooseCharacterIndex = isRightDrag ? nowChooseCharacterIndex - 1 : nowChooseCharacterIndex + 1;
        }

        slideableBgRectTransform[nowChooseCharacterIndex].DOAnchorPos(new Vector2(0, 0), 0.3f);
        TextReSettings();
        yield return new WaitForSeconds(0.035f);
        isDraging = false;
    }

    IEnumerator ImagePosInitialization(int targetPos, bool isRightDrag)
    {
        for (int nowIndex = 0; nowIndex <= maxCharacterIndex; nowIndex++) //전체 이미지 위치 초기화(현재 이동중인 이미지는 딜레이 후 위치 변경)
        {
            if (isRightDrag == false && nowIndex == maxCharacterIndex || isRightDrag && nowIndex == minCharacterIndex)
            {
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, true));
            }
            else
            {
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, false));
            }
        }

        nowChooseCharacterIndex = isRightDrag ? maxCharacterIndex : minCharacterIndex;

        slideableBgRectTransform[nowChooseCharacterIndex].DOAnchorPos(new Vector2(0, 0), 0.3f);
        TextReSettings();
        yield return null;
    }

    IEnumerator ImagerelocationDelay(Vector2 targetPos, int moveImageIndex, bool isGiveDelay)
    {
        if (isGiveDelay)
        {
            yield return new WaitForSeconds(0.35f);
            isDraging = false;
        }
        slideableBgRectTransform[moveImageIndex].DOAnchorPos(targetPos, 0);
    }

    public void ProficiencyUpgradeOrUnlock()
    {
        if (isNowChooseCharacterUnlock[nowChooseCharacterIndex] == false) //&& nowChooseCharacterUnlockCost[nowChooseCharacterIndex] <= 현재 숙련도
        {
            //GameManager.Instance.GemUnit -= gemRequiredForColleaguenlock[nowColleagueIndex]; //숙련도 차감 

            isNowChooseCharacterUnlock[nowChooseCharacterIndex] = true;

            TextReSettings();
        }
        else if (isNowChooseCharacterUnlock[nowChooseCharacterIndex] && GameManager.Instance.MoneyUnit >= nowChooseCharacterUpgradeCost[nowChooseCharacterIndex])
        {
            GameManager.Instance.MoneyUnit -= nowChooseCharacterUpgradeCost[nowChooseCharacterIndex];

            nowChooseCharacterLevel[nowChooseCharacterIndex]++; //해당 인덱스 동료 레벨 증가

            nowChooseCharacterUpgradeCost[nowChooseCharacterIndex] += nowChooseCharacterUpgradeCost[nowChooseCharacterIndex] / 2;

            TextReSettings();
        }
    }

    public void TextReSettings()
    {
        nowChooseCharacterNameText.text = $"{nowChooseCharacterName[nowChooseCharacterIndex]}";

        nowChooseCharacterBuffText.text = $"{nowChooseCharacterBuff[nowChooseCharacterIndex]}";

        if (isNowChooseCharacterUnlock[nowChooseCharacterIndex] == false)
        {
            //숙련도 변수와 비교
            //nowChooseCharacterUpgradeOrUnlockButtonText.color = (nowChooseCharacterUnlockCost[nowChooseCharacterIndex] <= GameManager.Instance.MoneyUnit) ? greenTextColor : redTextColor;
            nowChooseCharacterUpgradeOrUnlockButtonText.text = $"캐릭터 잠금해제\n{nowChooseCharacterUnlockCost[nowChooseCharacterIndex]} 숙련도 필요";
        }
        else
        {
            nowChooseCharacterUpgradeOrUnlockButtonText.color = (nowChooseCharacterUpgradeCost[nowChooseCharacterIndex] <= GameManager.Instance.MoneyUnit) ? greenTextColor : redTextColor;
            nowChooseCharacterUpgradeOrUnlockButtonText.text = $"캐릭터 업그레이드\n{nowChooseCharacterUpgradeCost[nowChooseCharacterIndex]} 골드";
        }
    }
}
