using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class SkillUI
{
    public string SkillName;
    public Image FrontSKillImage;
    public Image BackSKillImage;
}

public class SkillManager : MonoBehaviour
{
    [Header("스킬 변수")]
    [SerializeField]
    private List<Skill> EquieSkills;
    [SerializeField]
    private int MaximamIdx = 4;
    [SerializeField]
    private int CurIdx = 0;
    [SerializeField]
    protected List<Skill> NoneEquieSkills;
    [SerializeField]
    private List<SkillUI> SkillUis;

    [SerializeField]
    Image AutoSkillBtn = null;
    [SerializeField]
    List<Sprite> SkillOnOffImg;
    [SerializeField]
    bool IsAutoSkill = false;

    [HideInInspector]
    public bool isSkillEquiping;

    [SerializeField]
    private SkillEquipManager semInstance;

    private void Start()
    {
        SkillEquie(0);
        SkillEquie(1);
        SkillEquie(2);
        SkillEquie(3);
    }

    private void Update()
    {
        for (int i = 0; i < EquieSkills.Count; i++)
        {
            SkillUis[EquieSkills[i].SkillEquieIdx].FrontSKillImage.fillAmount = EquieSkills[i].SkillCurCol / EquieSkills[i].SkillCol;

            if (IsAutoSkill == true && EquieSkills[i].WaitCol == false && Player.Instance.Range.TargetEnemy.Count > 0)
                EquieSkills[i].OnSkillEffect();
        }
    }

    public void SkillEquie(int idx)
    {
        if (CurIdx < MaximamIdx)
        {
            EquieSkills.Add(NoneEquieSkills[idx]);

            //임의
            EquieSkills[CurIdx].SkillEquieIdx = CurIdx;

            SkillUis[CurIdx].FrontSKillImage.sprite = EquieSkills[CurIdx].SkillImage;
            SkillUis[CurIdx].BackSKillImage.sprite = EquieSkills[CurIdx].SkillImage;

            CurIdx++;
        }
    }

    public void UseSkill(int idx)
    {
        if (isSkillEquiping)
        {
            isSkillEquiping = false;
            semInstance.changeObj.SetActive(false);

            //semInstance.nowSkillIndex
            //현재 선택한 스킬 인덱스로 스킬 바꾸기 
        }
        else
        {
            EquieSkills[idx].OnSkillEffect();
            SkillUis[idx].FrontSKillImage.fillAmount = 0.0f;
        }
    }

    public void AutoSkillOnOff()
    {
        if (IsAutoSkill == true)
        {
            IsAutoSkill = false;
            AutoSkillBtn.sprite = SkillOnOffImg[1];
        }

        else
        {
            IsAutoSkill = true;
            AutoSkillBtn.sprite = SkillOnOffImg[0];

        }
    }
}
