using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardKind
{
    Gold,
    Gem,
    Proficiency
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

    private int questIndex; //현재 퀘스트 인덱스

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
