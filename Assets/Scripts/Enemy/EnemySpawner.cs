using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

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

    [Header("EnemySpawner변수")]
    [SerializeField]
    private GameObject[] PoolingEnemyPrefabs;

    private Queue<Enemy> PoolingShortQueue = new Queue<Enemy>();
    private Queue<Enemy> PoolingLongQueue = new Queue<Enemy>();
    private Queue<Enemy> PoolingAirQueue = new Queue<Enemy>();

    public List<Enemy> SpawnEnemyList = new List<Enemy>();

    [SerializeField]
    private float MaxTime, MinTime;
    [SerializeField]
    private float MaxY, MinY;

    [SerializeField]
    private Coroutine EnemySpawnCorutine;

    [Header("StageInfo변수")]
    [SerializeField]
    private int StageID = -1;
    [SerializeField]
    private StageInfo StageData;

    // 몬스터 수량 배열화(순서 : short1, short2, short3, long1...)
    [SerializeField]
    private DataForm[] EnemyData;

    [Header("나중에 옮길 UI")]
    [SerializeField]
    private Image FrontProcessBar;
    [SerializeField]
    private TextMeshProUGUI DifficultyTxt;
    [SerializeField]
    private TextMeshProUGUI StageTxt;

    private void Awake()
    {
        Instance = this;
        Initialize(8);
    }

    #region Pool함수
    //초기화 타입별로 소환
    private void Initialize(int initCount)
    {
        for (int i = 0; i < PoolingEnemyPrefabs.Length; i++)
        {
            for (int j = 0; j < initCount; j++)
            {
                if (i == 0)
                    PoolingShortQueue.Enqueue(CreateNewEnemy(i));

                else if (i == 1)
                    PoolingLongQueue.Enqueue(CreateNewEnemy(i));

                else
                    PoolingAirQueue.Enqueue(CreateNewEnemy(i));
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
    private Enemy GetEnemy(EnemyType type, int form)
    {
        Enemy enemy = null;

        if (type == EnemyType.ShortDis)
        {
            //pool에 적이 있을 시 소환
            if (Instance.PoolingShortQueue.Count > 0)
            {
                enemy = Instance.PoolingShortQueue.Dequeue();
                enemy.transform.SetParent(null);
                enemy.gameObject.SetActive(true);
            }
        }

        else if (type == EnemyType.LongDis)
        {
            //pool에 적이 있을 시 소환
            if (Instance.PoolingLongQueue.Count > 0)
            {
                enemy = Instance.PoolingLongQueue.Dequeue();
                enemy.transform.SetParent(null);
                enemy.gameObject.SetActive(true);
            }
        }

        else if (type == EnemyType.Air)
        {
            //pool에 적이 있을 시 소환
            if (Instance.PoolingAirQueue.Count > 0)
            {
                enemy = Instance.PoolingAirQueue.Dequeue();
                enemy.transform.SetParent(null);
                enemy.gameObject.SetActive(true);
            }
        }

        //없을 시 새로 생성
        else
        {
            enemy = Instance.CreateNewEnemy((int)type);
            enemy.gameObject.SetActive(true);
            enemy.transform.SetParent(null);
        }

        enemy.GetComponent<Enemy>().BasicSetting(form);
        SpawnEnemyList.Add(enemy);

        return enemy;
    }

    //적 오브젝트 풀로 리턴
    public static void ReturnEnemy(EnemyType type, Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(Instance.transform);

        Instance.SpawnEnemyList.Remove(enemy);

        //타입별 리턴(리펙토링 필요)
        if (type == EnemyType.ShortDis)
            Instance.PoolingShortQueue.Enqueue(enemy);

        else if (type == EnemyType.LongDis)
            Instance.PoolingLongQueue.Enqueue(enemy);

        if (type == EnemyType.Air)
            Instance.PoolingAirQueue.Enqueue(enemy);
    }
    #endregion

    #region Spawn함수
    public void StartEnemySpawn()
    {
        StageID++;
        ReciveData(StageID);
        EnemySpawnCorutine = StartCoroutine(EnemySpawn());
        UISetting();
    }

    public void UISetting()
    {
        string StageNumStr = StageData.StageNumber.ToString();

        StageTxt.text = $"{StageNumStr[0]}-{StageNumStr[1]}";

        switch (int.Parse(StageNumStr[2].ToString()))
        {
            case 1:
                StartCoroutine(SetProcessBar(0.0f));
                break;
            case 2:
                StartCoroutine(SetProcessBar(0.35f));
                break;
            case 3:
                StartCoroutine(SetProcessBar(0.6f));
                break;
            case 4:
                StartCoroutine(SetProcessBar(0.82f));
                break;
            case 5:
                StartCoroutine(SetProcessBar(1.0f));
                break;
        }
    }

    public IEnumerator SetProcessBar(float value)
    {
        yield return null;

        if (value > 0)
        {
            //Debug.Log("Up");

            while(FrontProcessBar.fillAmount < value)
            {
                yield return null;
                FrontProcessBar.fillAmount += Time.deltaTime;
            }
        }

        else
        {
            //Debug.Log("Down");

            while (FrontProcessBar.fillAmount > value)
            {
                yield return null;
                FrontProcessBar.fillAmount -= Time.deltaTime * 2;
            }
        }

        yield break;
    }

    public void StopEnemySpawn()
    {
        StopCoroutine(EnemySpawnCorutine);
    }

    private IEnumerator EnemySpawn()
    {
        yield return null;

        //일반스테이지 일 때
        if (StageData.IsBossStage == "FALSE")
        {
            for (int i = 0; i < EnemyData.Length; i++)
            {
                if (EnemyData[i].EnemyValue > 0)
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

        //일반스테이지가 아닐 때
        else
        {

        }
    }
    #endregion

    #region 데이터 전달 함수
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
