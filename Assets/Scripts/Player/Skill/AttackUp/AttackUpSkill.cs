using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpSkill : Skill
{
    protected override IEnumerator SkillEffect()
    {
        return base.SkillEffect();



        OffSkillEffect();
    }
}
