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

    [SerializeField]
    [Tooltip("���� ����Ʈ �ε��� ǥ�� �ؽ�Ʈ")]
    private Text indexText;

    [SerializeField]
    [Tooltip("���� ����Ʈ ���൵ ǥ�� �ؽ�Ʈ")]
    private Text progressText;

    private QuestKind questKind; //���� ����Ʈ ����

    [SerializeField]
    private int questIndex = 1; //���� ����Ʈ �ε���

    private void Start()
    {
        for (int nowIndex = 0; nowIndex < datas.Length; nowIndex++)//���� �� �ҷ����� �� �� ����Ʈ ��ǥ�� �ִ�ġ �缼��
        {
            datas[nowIndex].maxFigure += datas[nowIndex].incremental * (questIndex / 8);
        }

        InformationFix();
    }

    /// <summary>
    /// ���� ���� �� ���� �Լ�
    /// </summary>
    public void InformationFix()
    {
        TextReSetting();
    }

    /// <summary>
    /// �ؽ�Ʈ �缼�� �Լ�
    /// </summary>
    private void TextReSetting()
    {
        indexText.text = $"����Ʈ {questIndex}";
        progressText.text = $"{datas[(int)questKind].name} ({datas[(int)questKind].nowFigure}/{datas[(int)questKind].maxFigure})";
    }

    /// <summary>
    /// ����Ʈ ���� ���� �� Ŭ���� ���� �Լ�
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
