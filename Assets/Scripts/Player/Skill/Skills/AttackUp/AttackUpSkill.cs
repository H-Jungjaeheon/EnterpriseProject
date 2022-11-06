using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpSkill : Skill
{
    [SerializeField]
    private int UpgreadAttackValue = 0;
    [SerializeField]
    private float UpgreadPercent;

    protected override IEnumerator SkillEffect()
    {
        yield return null;

        #region SkillEffect
        UpgreadAttackValue = (int)((float)Player.Instance.AttackPower * (UpgreadPercent / 100.0f));
        Player.Instance.AttackPower += UpgreadAttackValue;

        OnSkillParticle(0);

        yield return new WaitForSeconds(SkillDuration);

        OffSkillParticle(0);

        Player.Instance.AttackPower -= UpgreadAttackValue;
        #endregion

        OffSkillEffect();
    }
}
