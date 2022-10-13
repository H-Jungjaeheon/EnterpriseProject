using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoodObj : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteThisObj;

    [SerializeField]
    private int foodObjIndex;

    [HideInInspector]
    public Vector3 nextPos;

    [HideInInspector]
    public Vector3 lastPos;

    [SerializeField]
    private Vector3[] arrangingArrivalPositions;

    public int nowIndex;
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
        lastIndex = nowIndex;
        nowIndex = isGoNextPos ? nowIndex + 1 : nowIndex - 1;

        if (nowIndex > 2)
        {
            nowIndex = 0;
        }
        else if (nowIndex < 0)
        {
            nowIndex = 2;
        }

        nextPos = arrangingArrivalPositions[nowIndex];
        lastPos = arrangingArrivalPositions[nowIndex];
        StartCoroutine(MovingAnim(isGoNextPos));
    }
    private IEnumerator MovingAnim(bool isGoNextPos)
    {
        if (isGoNextPos)
        {
            if (nextPos == arrangingArrivalPositions[0])
            {
                spriteThisObj.sortingOrder = 13;
            }
            else if (nextPos == arrangingArrivalPositions[1])
            {
                spriteThisObj.sortingOrder = 11;
            }
            else
            {
                spriteThisObj.sortingOrder = 12;
            }
            transform.DOMove(nextPos, 0.5f);
        }
        else
        {
            if (lastPos == arrangingArrivalPositions[0])
            {
                spriteThisObj.sortingOrder = 13;
            }
            else if (lastPos == arrangingArrivalPositions[1])
            {
                spriteThisObj.sortingOrder = 11;
            }
            else
            {
                spriteThisObj.sortingOrder = 12;
            }
            transform.DOMove(lastPos, 0.5f);
        }
        yield return null;
    }
}
