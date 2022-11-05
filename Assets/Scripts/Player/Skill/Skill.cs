using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("��ų ������/�ܺ�")]
    [SerializeField]
    private SkillData SkillData;

    [Header("��ų ������/����")]
    public string SkillName;
    public Sprite SkillImage;
    public float SkillCol;

    [Header("��ų ��� ����")]
    [SerializeField]
    protected int SkillEquieIdx;

    protected virtual void Awake()
    {
        ApplySkillData();
    }

    //������ ����
    protected void ApplySkillData()
    {
        SkillName = SkillData.SkillName;
        SkillImage = SkillData.SkillImage;
        SkillCol = SkillData.SkillCol;
    }

    protected virtual void OnSkillEffect()
    {

    }
}
