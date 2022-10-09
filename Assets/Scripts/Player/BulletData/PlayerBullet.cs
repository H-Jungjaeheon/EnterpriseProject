using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //세팅 함수
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

    //공격 함수
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
