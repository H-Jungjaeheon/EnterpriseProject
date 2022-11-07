using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Update()
    {
        Vector3 dir = TargetPos.position - transform.position;

        // Ÿ�� �������� ȸ����
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle * RotationSpeed, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Attack(other.GetComponent<Enemy>());
            CameraManager.Instance.OnCameraShake(0.3f, 0.05f);

            TargetPos = null;
            PlayerBulletObjectPool.Instance.ReturnBullet(this);
        }
    }

    //���� �Լ�
    public void TargetSetting(GameObject Target)
    {
        TargetPos = Target.transform;
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

    //���� �Լ�
    private void Attack(Enemy enemy)
    {
        enemy.Hp -= BulletPower;
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
