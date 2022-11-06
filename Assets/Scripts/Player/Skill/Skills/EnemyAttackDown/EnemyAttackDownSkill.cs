using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackDownSkill : Skill
{
    [SerializeField]
    private List<GameObject> UseParticle;

    [SerializeField]
    private int UpgreadAttackValue = 0;

    protected override IEnumerator SkillEffect()
    {
        yield return null;

        #region SkillEffect
        for (int i = 0; i < EnemySpawner.Instance.SpawnEnemyList.Count; i++)
        {
            Enemy enemy = EnemySpawner.Instance.SpawnEnemyList[i];
            GameObject Particle = SkillParticles[i];
            UseParticle.Add(Particle);

            Particle.transform.SetParent(enemy.transform);
            Particle.transform.localPosition = Vector3.zero;

            Particle.SetActive(true);
        }
        #endregion

        yield return new WaitForSeconds(SkillDuration);

        foreach(var Particle in UseParticle)
        {
            Particle.SetActive(false);
            Particle.transform.SetParent(this.transform);
            Particle.transform.localPosition = Vector3.zero;

            UseParticle.Remove(Particle);
        }

        OffSkillEffect();
    }
}
