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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SkillEquie(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SkillEquie(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SkillEquie(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SkillEquie(3);
    }

    public void SkillEquie(int idx)
    {
        if(CurIdx < MaximamIdx)
        {
            EquieSkills.Add(NoneEquieSkills[idx]);
            SkillUis[CurIdx].FrontSKillImage.sprite = EquieSkills[CurIdx].SkillImage;
            SkillUis[CurIdx].BackSKillImage.sprite = EquieSkills[CurIdx].SkillImage;

            CurIdx++;
        }
    }
}
