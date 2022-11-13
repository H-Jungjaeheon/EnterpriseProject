using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBullet : MonoBehaviour
{
    [Header("��ũ���ͺ� ���� ����")]
    [SerializeField]
    private BulletData[] BulletData;

    [Header("�Ѿ� ������Ʈ")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Header("�Ѿ� ���� ����")]
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

    [Header("���� ���� ����")]
    [SerializeField]
    private Transform TargetPos;
    [SerializeField]
    private float RotationSpeed;
    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    GameObject Target;
    [SerializeField]
    bool IsTarget = false;
    [SerializeField]
    bool TargetActive = false;

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

        // Ÿ�� �������� ȸ����
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
            other.GetComponent<Enemy>().StartTakeDamage(BulletPower, false);

            IsTarget = false;

            TargetPos = null;
            PlayerBulletObjectPool.Instance.ReturnBullet(this);
        }
    }

    //���� �Լ�
    public void TargetSetting(GameObject Target)
    {
        TargetPos = Target.transform;
        this.Target = Target;

        StartCoroutine(BulletMove());
    }

    private void BasicSetting()
    {
        int Select = 0;

        this.BulletType = BulletData[Select].BulletType;
        this.BulletImg = BulletData[Select].BulletImg;
        this.BulletName = BulletData[Select].BulletName;

        this.BulletPower = BulletData[Select].BulletPower;
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
