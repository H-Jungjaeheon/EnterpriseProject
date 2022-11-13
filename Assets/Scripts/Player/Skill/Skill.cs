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
    public float SkillCurCol;
    public bool WaitCol = false;

    public float SkillDuration;

    [Header("��ų ��� ����")]
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

    //������ ����
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
