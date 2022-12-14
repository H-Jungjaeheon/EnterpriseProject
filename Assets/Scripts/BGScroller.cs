using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGScrollData
{
    public Transform[] BackGrounds;
    public float FixMoveSpeed;
    public float MoveSpeed;

    public Vector3 NextPos;
}

public class BGScroller : MonoBehaviour
{
    [SerializeField]
    BGScrollData[] ScrollDatas;

    [SerializeField]
    float ResetPosX;
    // Start is called before the first frame update

    private void Start()
    {
        for (int i = 0; i < ScrollDatas.Length; i++)
        {
            ScrollDatas[i].NextPos = ScrollDatas[i].BackGrounds[ScrollDatas[i].BackGrounds.Length - 1].localPosition;
        }
    }

    void Update()
    {
        for (int i = 0; i < ScrollDatas.Length; i++)
        {
            for (int j = 0; j < ScrollDatas[i].BackGrounds.Length; j++)
            {
                //ScrollDatas[i].BackGrounds[j].localPosition += new Vector3(-ScrollDatas[i].Speed, 0, 0) * Time.deltaTime;

                ScrollDatas[i].MoveSpeed = ((Player.Instance.transform.position.x - Player.Instance.StendPosX) * -ScrollDatas[i].FixMoveSpeed) * Time.deltaTime;

                if (ScrollDatas[i].MoveSpeed > 0)
                    ScrollDatas[i].MoveSpeed = 0.0f;

                ScrollDatas[i].BackGrounds[j].localPosition += new Vector3(ScrollDatas[i].MoveSpeed, 0, 0);

                if (ScrollDatas[i].BackGrounds[j].localPosition.x <= ResetPosX)
                {
                    ScrollDatas[i].BackGrounds[j].localPosition = ScrollDatas[i].NextPos;
                }
            }
        }
    }
}