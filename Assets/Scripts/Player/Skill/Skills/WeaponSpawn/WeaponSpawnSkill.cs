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

        while(CurShootCnt <= MaxShootCnt)
        {
            float RanTime = Random.Range(1.0f, 2.0f);

            GameObject Bullet = Instantiate(Bullets[Random.Range(0, Bullets.Count)], this.gameObject.transform.position, Quaternion.identity);

            Destroy(Bullet, RanTime);

            Bullet.GetComponent<SpriteRenderer>().DOFade(0, RanTime);
            Bullet.transform.DOMoveX(this.transform.position.x + Random.Range(2.0f, 3.0f), 1.0f);

            CurShootCnt++;

            yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
        }

        yield return new WaitForSeconds(SkillDuration);
        #endregion

        OffSkillEffect();
    }
}
