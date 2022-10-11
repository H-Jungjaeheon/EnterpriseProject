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
    private float MaxTime, MinTime;
    [SerializeField]
    private float MaxY, MinY;

    [SerializeField]
    private Coroutine EnemySpawnCorutine;

    [Header("StageInfo변수")]
    [SerializeField]
    private StageInfo StageData;

    // 몬스터 수량 배열화(순서 : short1, short2, short3, long1...)
    [SerializeField]
    private int[] MonsterData = new int[9];


    private void Awake()
    {
        Instance = this;
        Initialize(8);
    }

    private void Start()
    {
        Instance.StartEnemySpawn(5);
        ReciveData(4);
    }

    #region Pool함수
    //초기화
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

    //초기화된 적 미리 소환
    private Enemy CreateNewEnemy(int Type)
    {
        var newEnemy = Instantiate(PoolingEnemyPrefabs[Type]).GetComponent<Enemy>();
        newEnemy.gameObject.SetActive(false);
        newEnemy.transform.SetParent(transform);
        return newEnemy;
    }

    //적 활성화
    private Enemy GetEnemy()
    {
        Enemy enemy = null;

        //pool에 적이 있을 시 소환
        if(Instance.PoolingEnemyQueue.Count > 0)
        {
            enemy = Instance.PoolingEnemyQueue.Dequeue();
            enemy.transform.SetParent(null);
            enemy.gameObject.SetActive(true);
        }

        //없을 시 새로 생성
        else
        {
            enemy = Instance.CreateNewEnemy(0);
            enemy.gameObject.SetActive(true);
            enemy.transform.SetParent(null);
        }

        return enemy;
    }

    //적 오브젝트 풀로 리턴
    public static void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(Instance.transform);
        Instance.PoolingEnemyQueue.Enqueue(enemy);
    }
    #endregion

    #region Spawn함수
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

    #region 데이터 전달 함수
    void ReciveData(int StageID)
    {
        StageData = this.GetComponent<ReciveStageInfo>().GetStageInfo(StageID);

        MonsterData[0] = StageData.Shrot_1;
        MonsterData[1] = StageData.Shrot_2;
        MonsterData[2] = StageData.Shrot_3;
        //---------------------------------------short
        MonsterData[3] = StageData.Long_1;
        MonsterData[4] = StageData.Long_2;
        MonsterData[5] = StageData.Long_3;
        //---------------------------------------long
        MonsterData[6] = StageData.Air_1;
        MonsterData[7] = StageData.Air_2;
        MonsterData[8] = StageData.Air_3;
        //---------------------------------------air
    }
    #endregion
}
