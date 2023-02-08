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

    Dictionary<int, Skill> EquieSkillsDic = new Dictionary<int, Skill>();
    Dictionary<string, Skill> CanEquoeSkills = new Dictionary<string, Skill>();

    public Sprite NonePlace;

    [HideInInspector]
    public bool isSkillEquiping;

    [SerializeField]
    private SkillEquipManager semInstance;

    private void Start()
    {
        foreach (var Obj in NoneEquieSkills)
        {
            CanEquoeSkills.Add(Obj.SkillName, Obj);
        }
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

    public void SkillEquie(int idx, string SkillName)
    {
        if (EquieSkillsDic.ContainsKey(idx) == false)
        {
            if(EquieSkillsDic.ContainsValue(CanEquoeSkills[SkillName]) == true)
            {
                Debug.Log("Yes");

                SkillUis[CanEquoeSkills[SkillName].SkillEquieIdx].FrontSKillImage.sprite = NonePlace;
                SkillUis[CanEquoeSkills[SkillName].SkillEquieIdx].BackSKillImage.sprite = NonePlace;

                EquieSkills.Remove(CanEquoeSkills[SkillName]);
                EquieSkillsDic.Remove(CanEquoeSkills[SkillName].SkillEquieIdx);
            }

            EquieSkills.Add(CanEquoeSkills[SkillName]);
            EquieSkillsDic.Add(idx, CanEquoeSkills[SkillName]);

            EquieSkillsDic[idx].SkillEquieIdx = idx;

            SkillUis[idx].FrontSKillImage.sprite = EquieSkillsDic[idx].SkillImage;
            SkillUis[idx].BackSKillImage.sprite = EquieSkillsDic[idx].SkillImage;
        }

        else
        {
            if (EquieSkillsDic.ContainsValue(CanEquoeSkills[SkillName]) == true)
            {
                Debug.Log("Yes");

                SkillUis[CanEquoeSkills[SkillName].SkillEquieIdx].FrontSKillImage.sprite = NonePlace;
                SkillUis[CanEquoeSkills[SkillName].SkillEquieIdx].BackSKillImage.sprite = NonePlace;

                EquieSkills.Remove(CanEquoeSkills[SkillName]);
                EquieSkillsDic.Remove(CanEquoeSkills[SkillName].SkillEquieIdx);
            }

            EquieSkills.Remove(EquieSkillsDic[idx]);
            EquieSkillsDic.Remove(idx);

            EquieSkills.Add(CanEquoeSkills[SkillName]);
            EquieSkillsDic.Add(idx, CanEquoeSkills[SkillName]);

            EquieSkillsDic[idx].SkillEquieIdx = idx;

            SkillUis[idx].FrontSKillImage.sprite = EquieSkillsDic[idx].SkillImage;
            SkillUis[idx].BackSKillImage.sprite = EquieSkillsDic[idx].SkillImage;
        }
    }

    public void SkillUnEquie(int idx, string SkillName)
    {
    }

    public void UseSkill(int idx)
    {
        if (isSkillEquiping)
        {
            isSkillEquiping = false;
            semInstance.changeObj.SetActive(false);

            //semInstance.nowSkillIndex
            //현재 선택한 스킬 인덱스로 스킬 바꾸기 

            SkillEquie(idx, semInstance.CurSkillData.SkillName);
        }
        else
        {
            if (EquieSkillsDic.ContainsKey(idx))
            {
                EquieSkillsDic[idx].OnSkillEffect();
                SkillUis[idx].FrontSKillImage.fillAmount = 0.0f;
            }
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
