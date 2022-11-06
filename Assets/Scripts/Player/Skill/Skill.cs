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
    protected Coroutine SkillCorutine;

    protected void Awake()
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

    public void OnSkillEffect()
    {
        SkillCorutine = StartCoroutine(SkillEffect());
    }

    public void OffSkillEffect()
    {
        StopCoroutine(SkillCorutine);
    }

    protected virtual IEnumerator SkillEffect()
    {
        yield return null;
    }
}
