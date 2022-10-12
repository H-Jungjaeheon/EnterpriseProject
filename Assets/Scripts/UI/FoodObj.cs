using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoodObj : MonoBehaviour
{
    [SerializeField]
    private int foodObjIndex;

    [HideInInspector]
    public Vector3 nextPos;

    [HideInInspector]
    public Vector3 lastPos;

    [SerializeField]
    private Vector3[] arrangingArrivalPositions;

    private int nowIndex;
    public int lastIndex;

    void Start()
    {
        StartSetting();
    }

    private void StartSetting()
    {
        nowIndex = foodObjIndex;
        lastIndex = nowIndex;

        lastIndex--;

        if (lastIndex < 0)
        {
            lastIndex = 2;
        }
    }

    public void FoodMovingAnimStart(bool isGoNextPos)
    {
        nowIndex = isGoNextPos ? nowIndex + 1 : nowIndex - 1;
        lastIndex = isGoNextPos ? lastIndex + 1 : lastIndex - 1;

        if (nowIndex > 2)
        {
            nowIndex = 0;
        }
        else if (nowIndex < 0)
        {
            nowIndex = 2;
        }

        if (lastIndex > 2)
        {
            lastIndex = 0;
        }
        else if (lastIndex < 0)
        {
            lastIndex = 2;
        }

        nextPos = arrangingArrivalPositions[nowIndex];
        lastPos = arrangingArrivalPositions[nowIndex];
        StartCoroutine(MovingAnim(isGoNextPos));
    }
    private IEnumerator MovingAnim(bool isGoNextPos)
    {
        if (isGoNextPos)
        {
            transform.DOMove(nextPos, 0.5f);
        }
        else
        {
            transform.DOMove(lastPos, 0.5f);
        }
        yield return null;
    }
}
