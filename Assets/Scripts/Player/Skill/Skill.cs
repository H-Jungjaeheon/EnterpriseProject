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
    public float SkillCurCol;
    public bool WaitCol = false;

    public float SkillDuration;

    [Header("스킬 사용 변수")]
    public int SkillEquieIdx;
    [SerializeField]
    protected List<GameObject> SkillParticles;
    protected Coroutine SkillCorutine;

    protected virtual void Awake()
    {
        ApplySkillData();
    }

    private void Update()
    {
        if (SkillCurCol < SkillCol)
        {
            SkillCurCol += Time.deltaTime;
        }

        else
        {
            SkillCurCol = SkillCol;
            WaitCol = false;
        }
    }

    //데이터 삽입
    protected void ApplySkillData()
    {
        SkillName = SkillData.SkillName;
        SkillImage = SkillData.SkillImage;
        SkillCol = SkillData.SkillCol;
        SkillCurCol = SkillData.SkillCol;
        SkillDuration = SkillData.SkillDuration;
    }

    public void OnSkillEffect()
    {
        if (SkillCorutine == null && WaitCol == false)
        {
            SkillCurCol = 0.0f;
            WaitCol = true;

            SkillCorutine = StartCoroutine(SkillEffect());
        }
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
