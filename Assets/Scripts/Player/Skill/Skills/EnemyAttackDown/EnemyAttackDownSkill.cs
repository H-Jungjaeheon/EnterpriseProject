using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackDownSkill : Skill
{
    [SerializeField]
    private int UpgreadAttackValue = 0;

    protected override IEnumerator SkillEffect()
    {
        yield return null;

        #region SkillEffect
        UpgreadAttackValue = (int)((float)Player.Instance.AttackPower * (70.0f / 100.0f));
        Player.Instance.AttackPower += UpgreadAttackValue;

        OnSkillParticle(0);

        yield return new WaitForSeconds(SkillDuration);

        OffSkillParticle(0);

        Player.Instance.AttackPower -= UpgreadAttackValue;
        #endregion

        OffSkillEffect();
    }
}
