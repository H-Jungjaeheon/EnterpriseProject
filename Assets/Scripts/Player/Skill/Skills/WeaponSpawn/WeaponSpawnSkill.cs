using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeaponSpawnSkill : Skill
{
    [SerializeField]
    private GameObject SpawnBase;
    [SerializeField]
    private SpriteRenderer SpawnBaseSpriteRenderer;

    protected override IEnumerator SkillEffect()
    {
        yield return null;

        #region SkillEffect
        SpawnBase.SetActive(true);
        

        OnSkillParticle(0);

        yield return new WaitForSeconds(SkillDuration);

        OffSkillParticle(0);
        #endregion

        OffSkillEffect();
    }
}
