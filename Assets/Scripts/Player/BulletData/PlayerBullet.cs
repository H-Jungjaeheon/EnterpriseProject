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

    // Start is called before the first frame update
    void Start()
    {
        BasicSetting();
        StartCoroutine(BulletMove());
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

        if (TargetActive == true)
        {
            dir = TargetPos.position - transform.position;
        }

        // 타겟 방향으로 회전함
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle * RotationSpeed, Vector3.forward);

        TargetActive = Target.activeSelf;

        if (TargetActive == false && IsTarget == true)
        {
            Debug.Log("Des");
            IsTarget = false;

            StartCoroutine(NoneTarget());
        }
    }

    IEnumerator NoneTarget()
    {
        yield return new WaitForSeconds(1.0f);

        TargetPos = null;

        this.gameObject.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);

        PlayerBulletObjectPool.Instance.ReturnBullet(this);

        yield break;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Attack");
            other.GetComponent<Enemy>().StartTakeDamage(BulletPower, IsCritical);

            IsTarget = false;

            TargetPos = null;
            PlayerBulletObjectPool.Instance.ReturnBullet(this);
        }
    }

    //세팅 함수
    public void TargetSetting(GameObject Target)
    {
        TargetPos = Target.transform;
        this.Target = Target;

        ResetBulletDamage();

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
        int Select = 0;

        this.BulletType = BulletData[Select].BulletType;
        this.BulletImg = BulletData[Select].BulletImg;
        this.BulletName = BulletData[Select].BulletName;

        this.BulletSpeed = BulletData[Select].BulletSpeed;

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
