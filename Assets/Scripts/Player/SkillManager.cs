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
    [SerializeReference]
    protected List<Skill> NoneEquieSkills;
    [SerializeField]
    private List<SkillUI> SkillUis;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EquieSkills[0].OnSkillEffect();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            EquieSkills[1].OnSkillEffect();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            EquieSkills[2].OnSkillEffect();
        if (Input.GetKeyDown(KeyCode.Alpha4))
            EquieSkills[3].OnSkillEffect();
    }

    public void SkillEquie(int idx)
    {

    }
}
