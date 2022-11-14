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
    [SerializeField]
    private List<GameObject> Bullets;
    [SerializeField]
    int CurShootCnt = 0;
    [SerializeField]
    int MaxShootCnt = 0;
    [SerializeField]
    int TikShootCnt = 0;

    protected override IEnumerator SkillEffect()
    {
        yield return null;

        #region SkillEffect
        SpawnBase.transform.localScale = Vector3.zero;
        SpawnBase.SetActive(true);

        SpawnBase.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f);
        SpawnBaseSpriteRenderer.DOFade(1.0f, 0.5f);

        yield return new WaitForSeconds(0.5f);

        while (CurShootCnt <= MaxShootCnt)
        {
            if (CurShootCnt % TikShootCnt == 0)
            {
                foreach (var Enemy in Player.Instance.Range.TargetEnemy)
                {
                    Enemy.GetComponent<Enemy>().StartTakeDamage(10, false);
                }
            }

            float RanTime = Random.Range(0.5f, 0.7f);

            GameObject Bullet = Instantiate(Bullets[Random.Range(0, Bullets.Count)], this.gameObject.transform.position + new Vector3(0, Random.Range(-0.7f, 0.8f), 0), Quaternion.identity);

            Destroy(Bullet, RanTime);

            Bullet.GetComponent<SpriteRenderer>().DOFade(0.2f, RanTime);
            Bullet.transform.DOMoveX(this.transform.position.x + Random.Range(4.0f, 6.0f), RanTime);

            CurShootCnt++;

            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }

        SpawnBase.transform.DOScale(Vector3.zero, 0.5f);
        SpawnBaseSpriteRenderer.DOFade(0, 0.5f);

        yield return new WaitForSeconds(0.5f);

        SpawnBase.SetActive(false);
        #endregion

        CurShootCnt = 0;
        OffSkillEffect();
    }
}
