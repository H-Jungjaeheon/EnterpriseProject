using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum CharacterKind
{
    Basic,
    SecondaryCharacter,
    CharacterCount
}

public enum TextColorKind
{
    Black,
    Red,
    Green
}

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
    private Text nameText;

    [SerializeField]
    [Tooltip("현재 캐릭터 버프 내용 텍스트")]
    private Text buffText;

    [SerializeField]
    [Tooltip("현재 캐릭터 장착/잠금해제 버튼 안내 텍스트")]
    private Text guideText;

    [SerializeField]
    [Tooltip("현재 캐릭터 장착/잠금해제 버튼 가격 텍스트")]
    private Text priceText;

    [SerializeField]
    [Tooltip("현재 잠금해제 캐릭터 자물쇠 오브젝트")]
    private GameObject[] lockObj;

    [SerializeField]
    [Tooltip("업그레이드/잠금해제 버튼 재화 이미지")]
    private Image goodsImage;

    [SerializeField]
    [Tooltip("텍스트 색 모음")]
    private Color[] textColors;

    private int minIndex; //최소 페이지 인덱스

    private int maxIndex; //최대 페이지 인덱스

    public int nowIndex; //현재 페이지 인덱스

    private int[] unlockCost = new int[(int)CharacterKind.CharacterCount]; //현재 페이지의 캐릭터 잠금 해제 가격

    public bool[] isUnlock = new bool[(int)CharacterKind.CharacterCount]; //현재 페이지의 캐릭터 잠금 해제 유무 판별

    public bool[] isEquiping = new bool[(int)CharacterKind.CharacterCount]; //현재 페이지의 캐릭터 장착 유무 판별

    private Vector3 dragStartMousePos; //드래그 시 마우스 포인트 시작 지점

    public bool isDraging; //드래그 중인지 판별


    private void Start()
    {
        isDraging = false;

        minIndex = 0;
        maxIndex = (int)CharacterKind.CharacterCount - 1;
        
        for (int nowIndex = minIndex; nowIndex <= maxIndex; nowIndex++)
        {
            unlockCost[nowIndex] = 10;
        }
        isUnlock[minIndex] = true;
    }

    private void OnEnable()
    {
        TextReSettings();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isDraging == false)
        {
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

        isDraging = true;
        slideableBgRectTransform[nowIndex].DOAnchorPos(new Vector2(imageTargetPosX, 0), 0.3f);

        if (isRightDrag && nowIndex == minIndex || isRightDrag == false && nowIndex == maxIndex)
        {
            StartCoroutine(ImagePosInitialization(-imageTargetPosX, isRightDrag));
            yield break;
        }
        else
        {
            nowIndex = isRightDrag ? nowIndex - 1 : nowIndex + 1;
        }

        slideableBgRectTransform[nowIndex].DOAnchorPos(new Vector2(0, 0), 0.3f);
        TextReSettings();
        yield return new WaitForSeconds(0.035f);
        isDraging = false;
    }

    IEnumerator ImagePosInitialization(int targetPos, bool isRightDrag)
    {
        for (int nowIndex = 0; nowIndex <= maxIndex; nowIndex++) //전체 이미지 위치 초기화(현재 이동중인 이미지는 딜레이 후 위치 변경)
        {
            if (isRightDrag == false && nowIndex == maxIndex || isRightDrag && nowIndex == minIndex)
            {
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, true));
            }
            else
            {
                StartCoroutine(ImagerelocationDelay(new Vector2(targetPos, 0), nowIndex, false));
            }
        }

        nowIndex = isRightDrag ? maxIndex : minIndex;

        slideableBgRectTransform[nowIndex].DOAnchorPos(new Vector2(0, 0), 0.3f);
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

    /// <summary>
    /// 캐릭터 장착 or 잠금해제 버튼 클릭 시
    /// </summary>
    public void ProficiencyUnlockOrEquip()
    {
        if (isUnlock[nowIndex] == false && unlockCost[nowIndex] <= GameManager.Instance.CurrentProficiency)
        {
            isUnlock[nowIndex] = true;

            lockObj[nowIndex].SetActive(false);

            TextReSettings();
        }
        else if(isUnlock[nowIndex] && isEquiping[nowIndex] == false)
        {
            for (int nowIndex = minIndex; nowIndex <= maxIndex; nowIndex++) //다른 인덱스 캐릭터들은 장착 해제, 현재 인덱스 캐릭터만 장착으로 갱신
            {
                isEquiping[nowIndex] = nowIndex == this.nowIndex ? true : false;
            }

            Player.Instance.CharacterChange(nowIndex);

            TextReSettings();
        }
    }

    /// <summary>
    /// 텍스트들 갱신
    /// </summary>
    public void TextReSettings()
    {
        nameText.text = $"{nowChooseCharacterName[nowIndex]}";

        buffText.text = $"{nowChooseCharacterBuff[nowIndex]}";

        if (isUnlock[nowIndex] == false)
        {
            guideText.color = (unlockCost[nowIndex] <= GameManager.Instance.CurrentProficiency) ? textColors[(int)TextColorKind.Green] : textColors[(int)TextColorKind.Red];

            guideText.transform.localPosition = new Vector3(0, 185, 0);

            guideText.text = "캐릭터 잠금해제";
            priceText.text = $"{unlockCost[nowIndex]}";

            goodsImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(-70 + ((priceText.text.Length - 1) * -20), 15, 0); //필요 재화 단위에 따라서 이미지 위치 변경

            goodsImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            guideText.color = textColors[(int)TextColorKind.Black];

            goodsImage.color = new Color(1, 1, 1, 0);
            
            guideText.transform.localPosition = new Vector3(0, 140, 0);
            
            priceText.text = "";

            guideText.text = isEquiping[nowIndex] == false ? guideText.text = "장착 가능" : guideText.text = "장착중";
        }
    }
}