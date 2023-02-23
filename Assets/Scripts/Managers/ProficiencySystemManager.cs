using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum CharacterKind
{
    Basic,
    SecondaryCharacter,
    ThirdCharacter,
    FourthCharacter
}

public enum TextColorKind
{
    Black,
    Red,
    Green
}

[System.Serializable]
public class CharacterData
{
    [Tooltip("캐릭터 이름")]
    public string name;

    [Tooltip("캐릭터 버프 내용")]
    public string buffContents;

    [Tooltip("캐릭터 배경 포지션")]
    public RectTransform bgAnchorPos;

    [Tooltip("캐릭터 잠금 표시 오브젝트")]
    public GameObject lockObj;

    [Tooltip("캐릭터 잠금 해제 가격")]
    public int unlockCost;

    [Tooltip("캐릭터 잠금 해제 유무 판별")]
    public bool isUnlock;

    [Tooltip("캐릭터 장착 유무 판별")]
    public bool isEquiping;
}

public class ProficiencySystemManager : Singleton<ProficiencySystemManager>
{
    [Tooltip("각 스킨 시스템 캐릭터 데이터들")]
    public CharacterData[] characterDatas;

    [SerializeField]
    [Tooltip("현재 캐릭터 이름 텍스트")]
    Text nameText;

    [SerializeField]
    [Tooltip("현재 캐릭터 버프 내용 텍스트")]
    Text buffText;

    [SerializeField]
    [Tooltip("현재 캐릭터 장착/잠금해제 버튼 안내 텍스트")]
    Text guideText;

    [SerializeField]
    [Tooltip("현재 캐릭터 장착/잠금해제 버튼 가격 텍스트")]
    Text priceText;

    [SerializeField]
    [Tooltip("업그레이드/잠금해제 버튼 재화 이미지")]
    Image goodsImage;

    [Tooltip("goodsImage의 RectTransform 컴포넌트")]
    RectTransform goodsImageRt;

    [Tooltip("현재 캐릭터 스킨")]
    public CharacterKind nowKind;

    [Tooltip("드래그 시 마우스 포인트 시작 지점")]
    Vector3 dragStartMousePos;

    [Tooltip("드래그 중인지 판별")]
    bool isDraging;

    [Tooltip("텍스트 포지션 세팅용 Vector3")]
    Vector3 textPos = Vector3.zero;

    [Tooltip("스킨 시스템에 쓰일 공통 딜레이")]
    WaitForSeconds delay = new WaitForSeconds(0.5f);

    #region 인스턴스 모음
    [Tooltip("GameManager 싱글톤 인스턴스")]
    GameManager gameManager;

    [Tooltip("Player 싱글톤 인스턴스")]
    Player player;
    #endregion

    #region 다회 사용 문자열 모음
    [Tooltip("GuideText : 스킨 잠금 해제 안내 문자열")]
    const string SKIN_UNLOCK = "스킨 잠금해제";

    [Tooltip("GuideText : 스킨 장착 가능 안내 문자열")]
    const string SKIN_EQUIPABLE = "장착 가능";

    [Tooltip("GuideText : 스킨 잠금 해제 안내 문자열")]
    const string SKIN_EQUIPING = "장착중";

    [Tooltip("빈 문자열")]
    const string NULL = "";
    #endregion

    private void Start()
    {
        gameManager = GameManager.Instance;
        player = Player.Instance;

        goodsImageRt = goodsImage.GetComponent<RectTransform>();
    }

    void OnEnable()
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

    /// <summary>
    /// 드래그 시작 시 실행 함수
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 드래그 시 이미지 위치 변경(애니메이션) 함수
    /// </summary>
    /// <param name="isRightDrag"> 현재 드래그 방향이 오른쪽인지 판별 </param>
    /// <returns></returns>
    IEnumerator Draging(bool isRightDrag)
    {
        Vector2 bgPos = Vector2.zero;
        int imageTargetPosX = isRightDrag ? 1300 : -1300; //오른쪽으로 드래그 했다면 현재 이미지 이동 목표 X값 1300으로 설정(왼쪽으로 드래그 시 -1300)

        bgPos.x = imageTargetPosX;

        isDraging = true;

        characterDatas[(int)nowKind].bgAnchorPos.DOAnchorPos(bgPos, 0.3f);

        if (isRightDrag && nowKind == CharacterKind.Basic || isRightDrag == false && nowKind == CharacterKind.FourthCharacter)
        {
            StartCoroutine(ImagePosInitialization(-imageTargetPosX, isRightDrag));

            yield break;
        }
        else
        {
            nowKind = isRightDrag ? nowKind - 1 : nowKind + 1;
        }

        characterDatas[(int)nowKind].bgAnchorPos.DOAnchorPos(Vector2.zero, 0.3f);

        TextReSettings();

        yield return delay;
        
        isDraging = false;
    }

    /// <summary>
    /// 이미지 재배치 : 각 이미지 인덱스가 끝에서 처음 or 처음에서 끝 인덱스로 넘어갈 때
    /// </summary>
    /// <param name="targetPos"> 재배치 이미지 목표 포지션 </param>
    /// <param name="isRightDrag"> 현재 드래그 방향이 오른쪽인지 판별 </param>
    /// <returns></returns>
    IEnumerator ImagePosInitialization(int targetPos, bool isRightDrag)
    {
        Vector2 curTargetPos = Vector2.zero;
        curTargetPos.x = targetPos;

        for (int nowIndex = 0; nowIndex <= (int)CharacterKind.FourthCharacter; nowIndex++) //전체 이미지 위치 초기화(현재 이동중인 이미지는 딜레이 후 위치 변경)
        {
            if (isRightDrag == false && nowIndex == (int)CharacterKind.FourthCharacter || isRightDrag && nowIndex == (int)CharacterKind.Basic)
            {
                StartCoroutine(ImagerelocationDelay(curTargetPos, nowIndex, true));
            }
            else
            {
                StartCoroutine(ImagerelocationDelay(curTargetPos, nowIndex, false));
            }
        }

        nowKind = isRightDrag ? CharacterKind.FourthCharacter : CharacterKind.Basic;

        characterDatas[(int)nowKind].bgAnchorPos.DOAnchorPos(Vector2.zero, 0.3f);

        TextReSettings();

        yield return null;
    }

    /// <summary>
    /// 이미지 재배치 : 재배치 시의 딜레이 코루틴
    /// </summary>
    /// <param name="targetPos"> 이미지의 재배치 포지션 </param>
    /// <param name="moveImageIndex"> 재배치할 이미지의 인덱스 </param>
    /// <param name="isGiveDelay"> 재배치 딜레이 유무(현재 이동중인 이미지는 딜레이 주기) </param>
    /// <returns></returns>
    IEnumerator ImagerelocationDelay(Vector2 targetPos, int moveImageIndex, bool isGiveDelay)
    {
        if (isGiveDelay)
        {
            yield return delay;

            isDraging = false;
        }

        characterDatas[moveImageIndex].bgAnchorPos.DOAnchorPos(targetPos, 0);
    }

    /// <summary>
    /// 캐릭터 장착 or 잠금해제 버튼 클릭 시
    /// </summary>
    public void ProficiencyUnlockOrEquip()
    {
        if (characterDatas[(int)nowKind].isUnlock == false && characterDatas[(int)nowKind].unlockCost <= gameManager.GemUnit)
        {
            gameManager.GemUnit -= characterDatas[(int)nowKind].unlockCost;

            characterDatas[(int)nowKind].isUnlock = true;

            characterDatas[(int)nowKind].lockObj.SetActive(false);
        }
        else if(characterDatas[(int)nowKind].isUnlock && characterDatas[(int)nowKind].isEquiping == false)
        {
            for (int nowIndex = (int)CharacterKind.Basic; nowIndex <= (int)CharacterKind.FourthCharacter; nowIndex++) //다른 인덱스 캐릭터들은 장착 해제, 현재 인덱스 캐릭터만 장착으로 갱신
            {
                characterDatas[nowIndex].isEquiping = (nowIndex == (int)nowKind) ? true : false;
            }

            player.CharacterChange((int)nowKind);
        }

        TextReSettings();
    }

    /// <summary>
    /// 텍스트 갱신 함수
    /// </summary>
    public void TextReSettings()
    {
        nameText.text = $"{characterDatas[(int)nowKind].name}";

        buffText.text = $"{characterDatas[(int)nowKind].buffContents}";

        if (characterDatas[(int)nowKind].isUnlock == false)
        {
            guideText.color = (characterDatas[(int)nowKind].unlockCost <= gameManager.GemUnit) ? Color.green : Color.red;
            goodsImage.color = Color.white;

            textPos.y = 185f;
            guideText.transform.localPosition = textPos;

            guideText.text = SKIN_UNLOCK;
            priceText.text = $"{characterDatas[(int)nowKind].unlockCost}";

            goodsImageRt.anchoredPosition = new Vector3(-70 + ((priceText.text.Length - 1) * -20), 15, 0); //필요 재화 단위에 따라서 이미지 위치 변경
        }
        else
        {
            guideText.color = Color.black;
            goodsImage.color = Color.clear;

            textPos.y = 140f;
            guideText.transform.localPosition = textPos;
            
            priceText.text = NULL;
            guideText.text = characterDatas[(int)nowKind].isEquiping == false ? guideText.text = SKIN_EQUIPABLE : guideText.text = SKIN_EQUIPING;
        }
    }
}