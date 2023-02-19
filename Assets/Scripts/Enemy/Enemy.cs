using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public enum BehaviorType
{
    Move, Attack, Stance
}

public class Enemy : MonoBehaviour
{
    [Header("Enemy 부모 변수")]
    public EnemyData[] Data;
    public SpriteRenderer EnemySprite;
    public Transform TargetPos;
    public BehaviorType CurBehaviorType;
   
    public List<GameObject> TextObjs;
    public GameObject TextObj;
    public TextMeshProUGUI Text;
    private Coroutine TakeDamageCorutine;
    private float UpYPos = 0.4f;
    private float DurationUpPos = 0.5f;

    [Header("데이터 받아올 변수")]
    [SerializeField]
    private EnemyType EnemyType;
    [SerializeField]
    private string EnemyName;
    [SerializeField]
    private int hp;
    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;

            if (hp <= 0)
            {
                Die();
            }
        }
    }

    [SerializeField]
    private int AttackPower;
    [SerializeField]
    private float AttackDistance;
    [SerializeField]
    private float AttackDelay;
    [SerializeField]
    private bool IsAttack;
    private Coroutine AttackCorutine;
    [SerializeField]
    private float MoveSpeed;

    [Header("파티클 소환")]
    [SerializeField]
    Material ParticleMaterial;
    [SerializeField]
    GameObject ParticlePrefab;

    [Header("코인")]
    [SerializeField]
    private GameObject CoinObj;
    [SerializeField]
    private int CoinValue;

    private void Awake()
    {
        EnemySprite = this.GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        Move();
    }

    #region Attack
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

    protected virtual IEnumerator Attack()
    {
        yield return null;

        while (TargetPos.gameObject.activeSelf != false)
        {
            //Debug.Log("Attack");
            yield return new WaitForSeconds(AttackDelay);
            Player.Instance.Hp -= AttackPower;

        }
    }
    #endregion

    #region TakeDamage
    public void StartTakeDamage(int Damage, bool IsCritical)
    {
        TakeDamageCorutine = StartCoroutine(TakeDamage(Damage, IsCritical));
    }

    protected IEnumerator TakeDamage(int Damage, bool IsCritical)
    {
        yield return null;

        GameObject Obj = Instantiate(TextObj, this.transform.position, Quaternion.identity);
        Destroy(Obj, DurationUpPos);

        Text = Obj.GetComponentInChildren<TextMeshProUGUI>();
        Text.text = Damage.ToString();

        if (IsCritical == true)
        {
            Text.color = Color.red;
        }

        TextObjs.Add(Obj);

        Hp -= Damage;

        Obj.transform.DOMoveY(Obj.transform.position.y + UpYPos, DurationUpPos);
        Text.DOFade(0.3f, DurationUpPos);

        yield return new WaitForSeconds(DurationUpPos);
        TextObjs.Remove(Obj);

        Text.color = Color.white;
        StopCoroutine(TakeDamageCorutine);
    }
    #endregion TakeDamage

    protected virtual void Move()
    {
        if (Vector2.Distance(TargetPos.position, this.transform.position) > AttackDistance)
        {
            SwichBehaviorType(BehaviorType.Move);
            this.gameObject.transform.position -= new Vector3(MoveSpeed * Time.deltaTime, 0, 0);
        }

        else
            SwichBehaviorType(BehaviorType.Attack);
    }

    void SwichBehaviorType(BehaviorType Type)
    {
        //Animator.SetTrigger(Type.ToString()); //  애니메이션 변경

        if (CurBehaviorType != Type)
        {
            CurBehaviorType = Type;

            switch (Type)
            {
                case BehaviorType.Attack:
                    
                    StartAttack();
                    break;

                case BehaviorType.Move:
                    StopAttack();
                    break;

                case BehaviorType.Stance:
                    break;
            }
        }
    }

    //데이터 값 세팅
    public void BasicSetting(int Select)
    {
        TargetPos = FindObjectOfType<Player>().transform;

        this.EnemyType = Data[Select].EnemyType;
        this.EnemySprite.sprite = Data[Select].EnemyImg;
        this.EnemyName = Data[Select].EnemyName;

        this.Hp = Data[Select].Hp;
        this.AttackPower = Data[Select].AttackPower;
        this.AttackDistance = Data[Select].AttackDistance;
        this.AttackDelay = Data[Select].AttackDelay;
        this.MoveSpeed = Data[Select].MoveSpeed;
    }

    void Die()
    {
        Player.Instance.Range.TargetEnemy.Remove(this.gameObject);
        EnemySpawner.ReturnEnemy(EnemyType, this);

        for (int i = 0; i < 3; i++)
        {
            Coin coin = Instantiate(CoinObj, this.transform.position, Quaternion.Euler(0,0,Random.Range(0.0f, 360.0f))).GetComponent<Coin>();

            coin.CoinValue = this.CoinValue / 3;
        }

        GameManager.Instance.GemUnit += 10;

        int cnt = TextObjs.Count;
        for (int i = 0; i < cnt; i++)
        {
            TextObjs.RemoveAt(0);
        }

        for (int i = 0; i < 3; i++)
        {
            BattleSceneManager.Instance.quantityOfMaterials[i]++;
        }

        ParticleSystemRenderer Particle = Instantiate(ParticlePrefab, this.transform.position, Quaternion.identity).GetComponent<ParticleSystemRenderer>();
        Particle.material = ParticleMaterial;
    }
}

