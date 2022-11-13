using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partner : MonoBehaviour
{
    public static Partner Instance;
    public static PartnerData PartnerData;

    [Header("�ܰ� ����")]
    public Sprite PartnerImg;
    public string PartnerName;

    [Header("���� ����")]
    public int AttackPower;
    public float AttackDelay;

    [Header("�⺻ ���� ������ ����")]
    public float AttackBuff;
    public float HpBuff;

    [Header("���� ����")]
    [SerializeField]
    public PlayerRange Range;
    [SerializeField]
    private bool IsAttack = false;
    private Coroutine AttackCorutine;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        OnAttack();
    }

    #region Attack
    void OnAttack()
    {
        if (IsAttack == false && Range.TargetEnemy.Count > 0)
        {
            Debug.Log("asdads");
            StartAttack();
        }

        else if (IsAttack == true && Range.TargetEnemy.Count <= 0)
        {
            StopAttack();
        }
    }

    void StartAttack()
    {
        IsAttack = true;
        AttackCorutine = StartCoroutine(Attack());
    }

    void StopAttack()
    {
        IsAttack = false;
        StopCoroutine(AttackCorutine);
    }

    IEnumerator Attack()
    {
        yield return null;

        while (Range.TargetEnemy.Count > 0)
        {
            //Debug.Log("Attack");
            yield return new WaitForSeconds(AttackDelay);
            ParterBulletObjectPool.Instance.GetBullet(Range.TargetEnemy[0]);
        }
    }
    #endregion

    public void Setting()
    {
        this.PartnerImg = PartnerData.PartnerImg;
        this.PartnerName = PartnerData.PartnerName;

        this.AttackPower = PartnerData.AttackPower;
        this.AttackDelay = PartnerData.AttackDelay;

        this.AttackBuff = PartnerData.AttackBuff;
        this.HpBuff = PartnerData.HpBuff;

        this.GetComponent<SpriteRenderer>().sprite = PartnerImg;
    }
}
