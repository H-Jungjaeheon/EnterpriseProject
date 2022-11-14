using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColleagueSpawnSkill : Skill
{
    [SerializeField]
    private GameObject Colleague;

    [SerializeField] public GameObject missile;
    [SerializeField] public GameObject target;

    [SerializeField] public float spd;
    [SerializeField] public int shot = 12;

    protected override IEnumerator SkillEffect()
    {
        yield return null;

        #region SkillEffect
        Colleague.SetActive(true);
        Colleague.GetComponent<SpriteRenderer>().DOFade(1.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(CreateMissile());

        Colleague.GetComponent<SpriteRenderer>().DOFade(0.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Colleague.SetActive(false);
        #endregion

        OffSkillEffect();
    }

    IEnumerator CreateMissile()
    {
        float CurDuration = 0.0f;
        float Time = 0.2f;

        while (CurDuration < SkillDuration)
        {
            if (Player.Instance.Range.TargetEnemy.Count > 0)
            {
                GameObject bullet = Instantiate(missile, Colleague.transform);
                bullet.GetComponent<BezierMissile>().master = Colleague.gameObject;
                bullet.GetComponent<BezierMissile>().enemy = Player.Instance.Range.TargetEnemy[Random.Range(0, Player.Instance.Range.TargetEnemy.Count - 1)];
            }

            CurDuration += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }
}

