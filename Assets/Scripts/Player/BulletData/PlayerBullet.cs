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
    private AnimationCurve curve;


    private void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        BasicSetting();
        StartCoroutine(IEFlight());
    }

    // Update is called once per frame
    void Update()
    {
        BulletMove(); 
    }

    //���� �Լ�
    public void TargetSetting(GameObject Target)
    {
        TargetPos = Target.transform;
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
    private void BulletMove()
    {

    }

    private IEnumerator IEFlight()
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
    }
}
