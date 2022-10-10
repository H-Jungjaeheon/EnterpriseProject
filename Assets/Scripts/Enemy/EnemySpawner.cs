using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("EnemySpawner����")]
    [SerializeField]
    private GameObject[] PoolingEnemyPrefabs;
    [SerializeField]
    private Queue<Enemy> PoolingEnemyQueue = new Queue<Enemy>();
    [SerializeField]
    private float MaxTime, MinTime;
    [SerializeField]
    private float MaxY, MinY;

    [SerializeField]
    private Coroutine EnemySpawnCorutine;

    [Header("StageInfo����")]
    [SerializeField]
    private StageInfo StageData;


    private void Awake()
    {
        Instance = this;
        Initialize(8);
    }

    private void Start()
    {
        Instance.StartEnemySpawn(5);
        StageData = this.GetComponent<ReciveStageInfo>().GetStageInfo(0);
    }

    #region Pool�Լ�
    //�ʱ�ȭ
    private void Initialize(int initCount)
    {
        for (int i = 0; i < PoolingEnemyPrefabs.Length; i++)
        {
            for (int j = 0; j < initCount; j++)
            {
                PoolingEnemyQueue.Enqueue(CreateNewEnemy(i));
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
    private Enemy GetEnemy()
    {
        Enemy enemy = null;

        //pool�� ���� ���� �� ��ȯ
        if(Instance.PoolingEnemyQueue.Count > 0)
        {
            enemy = Instance.PoolingEnemyQueue.Dequeue();
            enemy.transform.SetParent(null);
            enemy.gameObject.SetActive(true);
        }

        //���� �� ���� ����
        else
        {
            enemy = Instance.CreateNewEnemy(0);
            enemy.gameObject.SetActive(true);
            enemy.transform.SetParent(null);
        }



        return enemy;
    }

    //�� ������Ʈ Ǯ�� ����
    public static void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(Instance.transform);
        Instance.PoolingEnemyQueue.Enqueue(enemy);
    }
    #endregion

    #region Spawn�Լ�
    public void StartEnemySpawn(int SpawnCnt)
    {
        EnemySpawnCorutine = StartCoroutine(EnemySpawn(SpawnCnt));
    }

    public void StopEnemySpawn()
    {
        StopCoroutine(EnemySpawnCorutine);
    }

    private IEnumerator EnemySpawn(int SpawnCnt)
    {
        yield return null;

        while(SpawnCnt > 0)
        {
            Enemy enemy = Instance.GetEnemy();
            enemy.transform.position = new Vector2(this.transform.position.x, Random.Range(MinY, MaxY));

            SpawnCnt--;
            yield return new WaitForSeconds(Random.Range(MinTime, MaxTime));
        }
    }
    #endregion
}
