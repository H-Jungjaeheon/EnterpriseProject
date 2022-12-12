using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RewardKind
{
    Gold,
    Gem,
    Proficiency
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

    [SerializeField]
    private QuestData[] datas;

    [SerializeField]
    [Tooltip("현재 퀘스트 인덱스 표시 텍스트")]
    private Text indexText;

    [SerializeField]
    [Tooltip("현재 퀘스트 진행도 표시 텍스트")]
    private Text progressText;

    private QuestKind questKind; //현재 퀘스트 종류

    [SerializeField]
    private int questIndex = 1; //현재 퀘스트 인덱스

    private void Start()
    {
        for (int nowIndex = 0; nowIndex < datas.Length; nowIndex++)//저장 후 불러오기 시 각 퀘스트 목표량 최대치 재세팅
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
    /// 퀘스트 조건 충족 시 클리어 세팅 함수
    /// </summary>
    public void QuestClear()
    {
        if (datas[(int)questKind].nowFigure >= datas[(int)questKind].maxFigure)
        {
            datas[(int)questKind].nowFigure -= datas[(int)questKind].maxFigure;
            datas[(int)questKind].maxFigure += datas[(int)questKind].incremental;

            switch (datas[(int)questKind].rewardkind)
            {
                case RewardKind.Gold:
                    break;
                case RewardKind.Gem:
                    GameManager.Instance.GemUnit += datas[(int)questKind].amountPaid;
                    break;
                case RewardKind.Proficiency:
                    break;
            }

            questKind = (questKind == QuestKind.CriticalProbabilityStatLevel) ? QuestKind.DamageStatLevel : questKind + 1;

            questIndex++;

            InformationFix();
        }
    }
}
