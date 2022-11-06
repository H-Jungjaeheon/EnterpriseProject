using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PushEnemySkill : Skill
{
    [SerializeField]
    private float KnockBackForce = 0;

    [SerializeField]
    private GameObject Hand;
    private SpriteRenderer HandSpriteRenderer;

    private void Awake()
    {
        HandSpriteRenderer = Hand.GetComponent<SpriteRenderer>();
    }

    protected override IEnumerator SkillEffect()
    {
        yield return null;

        #region SkillEffect
        Hand.SetActive(true);
        

        OnSkillParticle(0);

        yield return new WaitForSeconds(SkillDuration);

        OffSkillParticle(0);

        Player.Instance.AttackPower -= UpgreadAttackValue;
        #endregion

        OffSkillEffect();
    }
}
