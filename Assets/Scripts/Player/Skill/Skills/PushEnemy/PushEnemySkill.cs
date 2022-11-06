using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PushEnemySkill : Skill
{
    [SerializeField]
    private float KnockBackForce = 0;

    [SerializeField]
    private GameObject Hand;
    private SpriteRenderer HandSpriteRenderer;
    [SerializeField]
    private Vector2 OriginalPos;

    [SerializeField]
    private float AddBackPosValue;
    [SerializeField]
    private Vector2 BackMovePos;

    [SerializeField]
    private float AddFrontPosValue;
    [SerializeField]
    private Vector2 FrontMovePos;

    private void Awake()
    {
        HandSpriteRenderer = Hand.GetComponent<SpriteRenderer>();

        OriginalPos = this.transform.position;
        FrontMovePos = this.transform.position + new Vector3(AddFrontPosValue, 0, 0);
        BackMovePos = this.transform.position - new Vector3(AddBackPosValue, 0, 0);
    }

    protected override IEnumerator SkillEffect()
    {
        yield return null;

        OriginalPos = this.transform.position;
        FrontMovePos = this.transform.position + new Vector3(AddFrontPosValue, 0, 0);
        BackMovePos = this.transform.position - new Vector3(AddBackPosValue, 0, 0);

        #region SkillEffect

        Hand.SetActive(true);
        HandSpriteRenderer.DOFade(0.0f, 0.0f);
        HandSpriteRenderer.DOFade(1.0f, 1.0f);

        //�ڷ� ��¡
        Hand.transform.DOMove(BackMovePos, 1.0f);
        yield return new WaitForSeconds(1.5f);

        //���� ȿ��
        foreach(var enemy in EnemySpawner.Instance.SpawnEnemyList)
        {
            enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 3.5f, ForceMode2D.Impulse);
            enemy.GetComponent<Rigidbody2D>().drag = 1.5f;
        }

        //������ ����
        Hand.transform.DOMove(FrontMovePos, 0.1f);
        yield return new WaitForSeconds(0.5f);

        //õõ�� �����
        HandSpriteRenderer.DOFade(0.0f, 1.5f);
        yield return new WaitForSeconds(1.5f);

        Hand.SetActive(false);
        Hand.transform.position = OriginalPos;

        yield return new WaitForSeconds(SkillDuration);


        //Player.Instance.AttackPower -= UpgreadAttackValue;
        #endregion

        OffSkillEffect();
    }
}
