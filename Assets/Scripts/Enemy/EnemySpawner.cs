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
    private float MaxY, MinY;

    [SerializeField]
    private Coroutine EnemySpawnCorutine;

    private void Awake()
    {
        Instance = this;
        Initialize(10);
    }

    private void Start()
    {
        Instance.StartEnemySpawn(5);
    }

    //�ʱ�ȭ
    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            PoolingEnemyQueue.Enqueue(CreateNewEnemy());
        }
    }

    //�ʱ�ȭ�� �� �̸� ��ȯ
    private Enemy CreateNewEnemy()
    {
        var newEnemy = Instantiate(PoolingEnemyPrefabs[0]).GetComponent<Enemy>();
        newEnemy.gameObject.SetActive(false);
        newEnemy.transform.SetParent(transform);
        return newEnemy;
    }

    //�� Ȱ��ȭ
    private Enemy GetEnemy()
    {
        if(Instance.PoolingEnemyQueue.Count > 0)
        {
            var enemy = Instance.PoolingEnemyQueue.Dequeue();
            enemy.transform.SetParent(null);
            enemy.gameObject.SetActive(true);
            return enemy;
        }

        else
        {
            var newEnemy = Instance.CreateNewEnemy();
            newEnemy.gameObject.SetActive(true);
            newEnemy.transform.SetParent(null);
            return newEnemy;
        }
    }

    //�� ������Ʈ Ǯ�� ����
    public static void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(Instance.transform);
        Instance.PoolingEnemyQueue.Enqueue(enemy);
    }

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
            Instance.GetEnemy();

            SpawnCnt--;
        }
    }
}
