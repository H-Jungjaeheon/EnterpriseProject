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
        [Tooltip("����Ʈ �̸�(����)")]
        public string name;
        [Tooltip("���� ��ǥ �޼� ��ġ")]
        public int nowFigure;
        [Tooltip("��ü ��ǥ �޼� ��ġ")]
        public int maxFigure;
        [Tooltip("��ǥ ������")]
        public int incremental;
        [Tooltip("���� ��ȭ ����")]
        public RewardKind rewardkind;
        [Tooltip("���� ��ȭ ���� ��ġ")]
        public int amountPaid;
    }

    [SerializeField]
    private QuestData[] datas;

    private int questIndex; //���� ����Ʈ �ε���

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
