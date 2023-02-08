using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RewardKind
{
    Gold,
    Gem
}

public enum BGKind
{
    BasicBg,
    ClearBg
}

public enum QuestKind
{
    DamageStatLevel,
    HpStatLevel,
    HealingStatLevel,
    AttackSpeedStatLevel,
    CriticalDamageStatLevel,
    CriticalProbabilityStatLevel
}

public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestData
    {
        [Tooltip("퀘스트 이름(내용)")]
        public string name;
        [Tooltip("현재 목표 달성 수치")]
        public int nowFigure;
        [Tooltip("전체 목표 달성 수치")]
        public int maxFigure;
        [Tooltip("목표 증가량")]
        public int incremental;
        [Tooltip("보상 재화 종류")]
        public RewardKind rewardkind;
        [Tooltip("보상 재화 지급 수치")]
        public int amountPaid;
    }

    public QuestData[] datas;

    [Tooltip("GameManager 싱글톤 인스턴스")]
    private GameManager gm;

    [SerializeField]
    [Tooltip("현재 퀘스트 인덱스 표시 텍스트")]
    private Text indexText;

    [SerializeField]
    [Tooltip("현재 퀘스트 진행도 표시 텍스트")]
    private Text progressText;

    [SerializeField]
    [Tooltip("퀘스트 클리어 표시 UI 오브젝트")]
    private GameObject clearUiObj;

    [SerializeField]
    [Tooltip("퀘스트 배경 UI 이미지")]
    private Image bgImage;

    [SerializeField]
    [Tooltip("퀘스트 배경 UI 리소스")]
    private Sprite[] bgResources;

    [HideInInspector]
    public QuestKind questKind; //현재 퀘스트 종류

    private int questIndex = 1; //현재 퀘스트 인덱스

    private void Start()
    {
        gm = GameManager.Instance;

        for (int nowIndex = 0; nowIndex < datas.Length; nowIndex++) //저장 후 불러오기 시 각 퀘스트 목표량 최대치 재세팅
        {
            datas[nowIndex].maxFigure += datas[nowIndex].incremental * (questIndex / 8);
        }

        InformationFix();
    }

    /// <summary>
    /// 정보 변경 시 갱신 함수
    /// </summary>
    public void InformationFix()
    {
        TextReSetting();
        ClearInspection();
    }

    /// <summary>
    /// 현재 퀘스트 클리어 판별 함수 (클리어 UI 표시)
    /// </summary>
    private void ClearInspection()
    {
        clearUiObj.SetActive(datas[(int)questKind].nowFigure >= datas[(int)questKind].maxFigure); //현재 퀘스트 목표치를 달성했다면, 클리어 표시 UI 활성화(달성하지 못했을 시 비활성화)

        bgImage.sprite = datas[(int)questKind].nowFigure >= datas[(int)questKind].maxFigure ? bgResources[(int)BGKind.ClearBg] : bgResources[(int)BGKind.BasicBg]; //현재 퀘스트 목표치를 달성했다면, 클리어 표시 배경으로 변경(달성하지 못했을 시 일반 배경으로 변경)
    }

    /// <summary>
    /// 텍스트 재세팅 함수
    /// </summary>
    private void TextReSetting()
    {
        indexText.text = $"퀘스트 {questIndex}";
        progressText.text = $"{datas[(int)questKind].name} ({datas[(int)questKind].nowFigure}/{datas[(int)questKind].maxFigure})";
    }

    /// <summary>
    /// 퀘스트 조건 충족 시 클리어 세팅 함수(버튼 연결)
    /// </summary>
    public void QuestClear()
    {
        if (datas[(int)questKind].nowFigure >= datas[(int)questKind].maxFigure) //퀘스트 조건을 달성 시 실행
        {
            datas[(int)questKind].maxFigure += datas[(int)questKind].incremental; //현재 클리어한 퀘스트의 클리어 조건 확장

            switch (datas[(int)questKind].rewardkind) //각 퀘스트 마다 설정된 재화 지급
            {
                case RewardKind.Gold:


                    break;
                case RewardKind.Gem:
                    gm.GemUnit += datas[(int)questKind].amountPaid;

                    break;
            }

            questKind = (questKind == QuestKind.CriticalProbabilityStatLevel) ? QuestKind.DamageStatLevel : questKind + 1; //현재 클리어한 퀘스트가 마지막 퀘스트 종류 인덱스면 퀘스트 종류 인덱스 0번으로 가기 or 다음 퀘스트 종류 인덱스로 가기

            questIndex++; //현재 퀘스트 번호 + 1 (n번째 퀘스트)

            InformationFix();
        }
    }
}
