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
    public float SkillDuration;

    [Header("스킬 사용 변수")]
    [SerializeField]
    protected int SkillEquieIdx;
    [SerializeField]
    protected List<GameObject> SkillParticles;
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
        SkillDuration = SkillData.SkillDuration;
    }

    public void OnSkillEffect()
    {
        if(SkillCorutine == null)
            SkillCorutine = StartCoroutine(SkillEffect());
    }

    public void OffSkillEffect()
    {
        StopCoroutine(SkillCorutine);
        SkillCorutine = null;
    }

    protected void OnSkillParticle(int Idx)
    {
        if (SkillParticles.Count > Idx)
            SkillParticles[Idx].SetActive(true);
    }

    protected void OffSkillParticle(int Idx)
    {
        if (SkillParticles.Count > Idx)
            SkillParticles[Idx].SetActive(false);
    }

    protected virtual IEnumerator SkillEffect()
    {
        yield return null;
    }
}
