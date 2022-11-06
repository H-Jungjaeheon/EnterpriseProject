using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("스킬 데이터/외부")]
    [SerializeField]
    private SkillData SkillData;

    [Header("스킬 데이터/내부")]
    public string SkillName;
    public Sprite SkillImage;
    public float SkillCol;

    [Header("스킬 사용 변수")]
    [SerializeField]
    protected int SkillEquieIdx;
    protected Coroutine SkillCorutine;

    protected void Awake()
    {
        ApplySkillData();
    }

    //데이터 삽입
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
