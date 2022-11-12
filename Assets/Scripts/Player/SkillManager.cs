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
    [Header("��ų ����")]
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

    public void SkillEquie(int idx)
    {
        if (CurIdx < MaximamIdx)
        {
            EquieSkills.Add(NoneEquieSkills[idx]);
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
            //���� ������ ��ų �ε����� ��ų �ٲٱ� 
        }
        else
        {
            EquieSkills[idx].OnSkillEffect();
        }
    }
}
