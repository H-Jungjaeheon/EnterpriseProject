using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBullet : MonoBehaviour
{
    [Header("스크립터블 변수 관련")]
    [SerializeField]
    private BulletData[] BulletData;

    [Header("총알 컴포넌트")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Header("총알 관련 변수")]
    [SerializeField]
    private BulletType BulletType;
    [SerializeField]
    private Sprite BulletImg;
    [SerializeField]
    private string BulletName;
    [SerializeField]
    private int BulletPower;
    [SerializeField]
    private float BulletSpeed;

    [Header("공격 관련 변수")]
    [SerializeField]
    private Transform TargetPos;
    [SerializeField]
    private float RotationSpeed;
    [SerializeField]
    private AnimationCurve curve;

    [Header("타겟 관련 변수")]
    [SerializeField]
    GameObject Target;
    [SerializeField]
    bool IsTarget = false;
    [SerializeField]
    bool TargetActive = false;

    [Header("데미지 관련 변수")]
    [SerializeField]
    bool IsCritical;
    [SerializeField]
    int CriticalDamage;

    private void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        IsTarget = true;
        TargetActive = true;

        this.gameObject.GetComponent<SpriteRenderer>().DOFade(1, 0);
    }

    void Update()
    {
        Vector3 dir = Vector3.one;

        if (TargetActive == true && Target)
        {
            dir = TargetPos.position - transform.position;
        }

        // 타겟 방향으로 회전함
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle * RotationSpeed, Vector3.forward);

        TargetActive = Target.activeSelf;

        if ((TargetActive == false && IsTarget == true) || (!Target && IsTarget == true))
        {
            IsTarget = false;

            StartCoroutine(NoneTarget());
        }
    }

    IEnumerator NoneTarget()
    {
        TargetPos = null;

        this.gameObject.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);

        PlayerBulletObjectPool.Instance.ReturnBullet(this);

        yield break;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(BulletType == BulletType.Player && other.CompareTag("Enemy"))
        {
            //Debug.Log("Attack");
            other.GetComponent<Enemy>().StartTakeDamage(BulletPower, IsCritical);

            IsTarget = false;

            TargetPos = null;
            PlayerBulletObjectPool.Instance.ReturnBullet(this);
        }

        else if(BulletType == BulletType.Enemy && other.CompareTag("Player"))
        {
            Player.Instance.Hp -= BulletPower;

            Destroy(this.gameObject);
        }
    }

    //세팅 함수
    public void TargetSetting(GameObject Target, BulletType BulletType, int Damage = 0)
    {
        TargetPos = Target.transform;
        this.Target = Target;

        if (BulletType == BulletType.Player)
        {
            BasicSetting();
            ResetBulletDamage();
        }

        else if((BulletType == BulletType.Enemy))
        {
            EnemyBasicSetting(Damage);
        }

        StartCoroutine(BulletMove());
    }

    void ResetBulletDamage()
    {
        IsCritical = false;
        CriticalDamage = 0;

        //치명타
        float RanCriticalPercent = Random.Range(0.0f, 100.1f);

        if (RanCriticalPercent <= Player.Instance.CriticalPercent)
        {
            IsCritical = true;
            CriticalDamage = (int)((float)Player.Instance.AttackPower * ((Player.Instance.CriticalDamage / 100.0f) - 1.0f));
        }

        this.BulletPower = Player.Instance.AttackPower + CriticalDamage;
    }

    private void BasicSetting()
    {
        this.BulletType = Player.Instance.BulletData[Player.Instance.SelectNumber].BulletType;
        this.BulletImg = Player.Instance.BulletData[Player.Instance.SelectNumber].BulletImg;
        this.BulletName = Player.Instance.BulletData[Player.Instance.SelectNumber].BulletName;

        this.BulletSpeed = Player.Instance.BulletData[Player.Instance.SelectNumber].BulletSpeed;

        spriteRenderer.sprite = BulletImg;
    }

    public void EnemyBasicSetting(int Damage)
    {
        this.BulletType = BulletData[0].BulletType;
        this.BulletImg = BulletData[0].BulletImg;
        this.BulletName = BulletData[0].BulletName;
        this.BulletPower = Damage;

        this.BulletSpeed = BulletData[0].BulletSpeed;

        spriteRenderer.sprite = BulletImg;
    }

    private IEnumerator BulletMove()
    {
        float duration = BulletSpeed;
        float time = 0.0f;
        float hoverHeight = 10.0f;

        Vector3 start = transform.position;
        Vector3 end = TargetPos.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            float linearT = time / duration;
            float heightT = curve.Evaluate(linearT);

            float height = Mathf.Lerp(0.0f, hoverHeight, heightT);

            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0.0f, height);

            yield return null;
        }

        yield break;
    }
}
