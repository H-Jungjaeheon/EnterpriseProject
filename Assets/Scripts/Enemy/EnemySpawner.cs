using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("EnemySpawner변수")]
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

    //초기화
    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            PoolingEnemyQueue.Enqueue(CreateNewEnemy());
        }
    }

    //초기화된 적 미리 소환
    private Enemy CreateNewEnemy()
    {
        var newEnemy = Instantiate(PoolingEnemyPrefabs[0]).GetComponent<Enemy>();
        newEnemy.gameObject.SetActive(false);
        newEnemy.transform.SetParent(transform);
        return newEnemy;
    }

    //적 활성화
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

    //적 오브젝트 풀로 리턴
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
