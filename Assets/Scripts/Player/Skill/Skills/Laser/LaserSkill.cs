using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserSkill: Skill
{
    [SerializeField]
    Vector3 OriginalScale = Vector3.zero;
    [SerializeField]
    Vector3 OriginalPos = Vector3.zero;
    [SerializeField]
    Vector3 MovePos = Vector3.zero;

    [SerializeField]
    private GameObject SpawnBase;
    [SerializeField]
    private SpriteRenderer SpawnBaseSpriteRenderer;
    [SerializeField]
    int CurShootCnt = 0;
    [SerializeField]
    int MaxShootCnt = 0;
    [SerializeField]
    int TikShootCnt = 0;

    protected override void Awake()
    {
        base.Awake();

        OriginalScale = SpawnBase.transform.localScale;
        OriginalPos = SpawnBase.transform.localPosition;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            OnSkillEffect();
        }
    }

    protected override IEnumerator SkillEffect()
    {
        yield return null;
        Debug.Log("Laser");

        #region SkillEffect
        OnSkillParticle(0);

        SpawnBase.SetActive(true);
        SpawnBase.transform.localPosition = OriginalPos;
        SpawnBase.transform.localScale = OriginalScale;
        SpawnBaseSpriteRenderer.DOFade(1, 0f);

        yield return new WaitForSeconds(3.0f);

        SpawnBase.transform.DOLocalMove(MovePos, 0.3f);
        yield return new WaitForSeconds(0.3f);

        while (CurShootCnt <= MaxShootCnt)
        {
            if (CurShootCnt % TikShootCnt == 0)
            {
                foreach (var Enemy in Player.Instance.Range.TargetEnemy)
                {
                    Enemy.GetComponent<Enemy>().StartTakeDamage(Random.Range(5,8), false);
                }
            }
            CurShootCnt++;

            float RandomTime = Random.Range(0.05f, 0.07f);
            float RandomValue = Random.Range(-0.03f, 0.03f);

            SpawnBase.transform.DOLocalMove(MovePos + new Vector3(RandomValue, 0, 0), RandomTime);
            SpawnBase.transform.DOScale(OriginalScale + new Vector3(RandomValue, 0, 0), RandomTime);

            yield return new WaitForSeconds(RandomTime);
        }

        SpawnBaseSpriteRenderer.DOFade(0, 0.5f);

        yield return new WaitForSeconds(0.5f);

        SpawnBase.SetActive(false);
        #endregion

        CurShootCnt = 0;

        OffSkillParticle(0);
        OffSkillEffect();
    }
}
