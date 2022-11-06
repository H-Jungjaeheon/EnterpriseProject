using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedUpSkill : Skill
{
    [SerializeField]
    private float UpgreadAttackSpeedValue = 0;
    [SerializeField]
    private float UpgreadPercent;

    protected override IEnumerator SkillEffect()
    {
        yield return null;

        #region SkillEffect
        UpgreadAttackSpeedValue = Player.Instance.AttackDelay * (UpgreadPercent / 100.0f);
        Player.Instance.AttackDelay -= UpgreadAttackSpeedValue;

        OnSkillParticle(0);

        yield return new WaitForSeconds(SkillDuration);

        OffSkillParticle(0);

        Player.Instance.AttackDelay += UpgreadAttackSpeedValue;
        #endregion

        OffSkillEffect();
    }
}
