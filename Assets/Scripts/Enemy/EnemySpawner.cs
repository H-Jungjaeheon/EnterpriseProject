using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataForm
{
    public EnemyType Type;
    public int EnemyValue;
    public int EnemyForm;
}

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("EnemySpawner����")]
    [SerializeField]
    private GameObject[] PoolingEnemyPrefabs;
    [SerializeField]
    private Queue<Enemy> PoolingShortQueue = new Queue<Enemy>();
    private Queue<Enemy> PoolingLongQueue = new Queue<Enemy>();
    private Queue<Enemy> PoolingAirQueue = new Queue<Enemy>();
    [SerializeField]
    private float MaxTime, MinTime;
    [SerializeField]
    private float MaxY, MinY;

    [SerializeField]
    private Coroutine EnemySpawnCorutine;

    [Header("StageInfo����")]
    [SerializeField]
    private StageInfo StageData;

    // ���� ���� �迭ȭ(���� : short1, short2, short3, long1...)
    [SerializeField]
    private DataForm[] EnemyData;


    private void Awake()
    {
        Instance = this;
        Initialize(8);
    }

    private void Start()
    {
        Instance.StartEnemySpawn();
        ReciveData(9);
    }

    #region Pool�Լ�
    //�ʱ�ȭ Ÿ�Ժ��� ��ȯ
    private void Initialize(int initCount)
    {
        for (int i = 0; i < PoolingEnemyPrefabs.Length; i++)
        {
            for (int j = 0; j < initCount; j++)
            {
               if(i == 0)
                    PoolingShortQueue.Enqueue(CreateNewEnemy(i));

               else if(i == 1)
                    PoolingLongQueue.Enqueue(CreateNewEnemy(i));

               else
                    PoolingAirQueue.Enqueue(CreateNewEnemy(i));
            }
        }
    }

    //�ʱ�ȭ�� �� �̸� ��ȯ
    private Enemy CreateNewEnemy(int Type)
    {
        var newEnemy = Instantiate(PoolingEnemyPrefabs[Type]).GetComponent<Enemy>();
        newEnemy.gameObject.SetActive(false);
        newEnemy.transform.SetParent(transform);

        return newEnemy;
    }

    //�� Ȱ��ȭ
    private Enemy GetEnemy(EnemyType type, int form)
    {
        Enemy enemy = null;

        if (type == EnemyType.ShortDis)
        {
            //pool�� ���� ���� �� ��ȯ
            if (Instance.PoolingShortQueue.Count > 0)
            {
                enemy = Instance.PoolingShortQueue.Dequeue();
                enemy.transform.SetParent(null);
                enemy.gameObject.SetActive(true);
            }
        }

        else if (type == EnemyType.LongDis)
        {
            //pool�� ���� ���� �� ��ȯ
            if (Instance.PoolingLongQueue.Count > 0)
            {
                enemy = Instance.PoolingLongQueue.Dequeue();
                enemy.transform.SetParent(null);
                enemy.gameObject.SetActive(true);
            }
        }

        else if(type == EnemyType.Air)
        {
            //pool�� ���� ���� �� ��ȯ
            if (Instance.PoolingAirQueue.Count > 0)
            {
                enemy = Instance.PoolingAirQueue.Dequeue();
                enemy.transform.SetParent(null);
                enemy.gameObject.SetActive(true);
            }
        }

        //���� �� ���� ����
        else
        {
            enemy = Instance.CreateNewEnemy((int)type);
            enemy.gameObject.SetActive(true);
            enemy.transform.SetParent(null);
        }

        enemy.GetComponent<Enemy>().BasicSetting(form);

        return enemy;
    }

    //�� ������Ʈ Ǯ�� ����
    public static void ReturnEnemy(EnemyType type ,Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(Instance.transform);

        //Ÿ�Ժ� ����(�����丵 �ʿ�)
        if(type == EnemyType.ShortDis)
            Instance.PoolingShortQueue.Enqueue(enemy);

        else if (type == EnemyType.LongDis)
            Instance.PoolingLongQueue.Enqueue(enemy);

        if (type == EnemyType.Air)
            Instance.PoolingAirQueue.Enqueue(enemy);
    }
    #endregion

    #region Spawn�Լ�
    public void StartEnemySpawn()
    {
        EnemySpawnCorutine = StartCoroutine(EnemySpawn());
    }

    public void StopEnemySpawn()
    {
        StopCoroutine(EnemySpawnCorutine);
    }

    private IEnumerator EnemySpawn()
    {
        yield return null;

        for (int i = 0; i < EnemyData.Length; i++)
        {
            if(EnemyData[i].EnemyValue > 0)
            {
                for (int j = 0; j < EnemyData[i].EnemyValue; j++)
                {
                    Enemy enemy = Instance.GetEnemy(EnemyData[i].Type, EnemyData[i].EnemyForm);
                    enemy.transform.position = new Vector2(this.transform.position.x, Random.Range(MinY, MaxY));

                    yield return new WaitForSeconds(Random.Range(MinTime, MaxTime));
                }
            }
        }
    }
    #endregion

    #region ������ ���� �Լ�
    void ReciveData(int StageID)
    {
        StageData = this.GetComponent<ReciveStageInfo>().GetStageInfo(StageID);

        EnemyData[0].EnemyValue = StageData.Shrot_1;
        EnemyData[1].EnemyValue = StageData.Shrot_2;
        EnemyData[2].EnemyValue = StageData.Shrot_3;
        //---------------------------------------short
        EnemyData[3].EnemyValue = StageData.Long_1;
        EnemyData[4].EnemyValue = StageData.Long_2;
        EnemyData[5].EnemyValue = StageData.Long_3;
        //---------------------------------------long
        EnemyData[6].EnemyValue = StageData.Air_1;
        EnemyData[7].EnemyValue = StageData.Air_2;
        EnemyData[8].EnemyValue = StageData.Air_3;
        //---------------------------------------air
    }
    #endregion
}
