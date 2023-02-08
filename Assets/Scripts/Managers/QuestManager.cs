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

    public QuestData[] datas;

    [Tooltip("GameManager �̱��� �ν��Ͻ�")]
    private GameManager gm;

    [SerializeField]
    [Tooltip("���� ����Ʈ �ε��� ǥ�� �ؽ�Ʈ")]
    private Text indexText;

    [SerializeField]
    [Tooltip("���� ����Ʈ ���൵ ǥ�� �ؽ�Ʈ")]
    private Text progressText;

    [SerializeField]
    [Tooltip("����Ʈ Ŭ���� ǥ�� UI ������Ʈ")]
    private GameObject clearUiObj;

    [SerializeField]
    [Tooltip("����Ʈ ��� UI �̹���")]
    private Image bgImage;

    [SerializeField]
    [Tooltip("����Ʈ ��� UI ���ҽ�")]
    private Sprite[] bgResources;

    [HideInInspector]
    public QuestKind questKind; //���� ����Ʈ ����

    private int questIndex = 1; //���� ����Ʈ �ε���

    private void Start()
    {
        gm = GameManager.Instance;

        for (int nowIndex = 0; nowIndex < datas.Length; nowIndex++) //���� �� �ҷ����� �� �� ����Ʈ ��ǥ�� �ִ�ġ �缼��
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
        ClearInspection();
    }

    /// <summary>
    /// ���� ����Ʈ Ŭ���� �Ǻ� �Լ� (Ŭ���� UI ǥ��)
    /// </summary>
    private void ClearInspection()
    {
        clearUiObj.SetActive(datas[(int)questKind].nowFigure >= datas[(int)questKind].maxFigure); //���� ����Ʈ ��ǥġ�� �޼��ߴٸ�, Ŭ���� ǥ�� UI Ȱ��ȭ(�޼����� ������ �� ��Ȱ��ȭ)

        bgImage.sprite = datas[(int)questKind].nowFigure >= datas[(int)questKind].maxFigure ? bgResources[(int)BGKind.ClearBg] : bgResources[(int)BGKind.BasicBg]; //���� ����Ʈ ��ǥġ�� �޼��ߴٸ�, Ŭ���� ǥ�� ������� ����(�޼����� ������ �� �Ϲ� ������� ����)
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
    /// ����Ʈ ���� ���� �� Ŭ���� ���� �Լ�(��ư ����)
    /// </summary>
    public void QuestClear()
    {
        if (datas[(int)questKind].nowFigure >= datas[(int)questKind].maxFigure) //����Ʈ ������ �޼� �� ����
        {
            datas[(int)questKind].maxFigure += datas[(int)questKind].incremental; //���� Ŭ������ ����Ʈ�� Ŭ���� ���� Ȯ��

            switch (datas[(int)questKind].rewardkind) //�� ����Ʈ ���� ������ ��ȭ ����
            {
                case RewardKind.Gold:


                    break;
                case RewardKind.Gem:
                    gm.GemUnit += datas[(int)questKind].amountPaid;

                    break;
            }

            questKind = (questKind == QuestKind.CriticalProbabilityStatLevel) ? QuestKind.DamageStatLevel : questKind + 1; //���� Ŭ������ ����Ʈ�� ������ ����Ʈ ���� �ε����� ����Ʈ ���� �ε��� 0������ ���� or ���� ����Ʈ ���� �ε����� ����

            questIndex++; //���� ����Ʈ ��ȣ + 1 (n��° ����Ʈ)

            InformationFix();
        }
    }
}
