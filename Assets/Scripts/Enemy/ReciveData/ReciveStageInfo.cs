using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReciveStageInfo : MonoBehaviour
{
    [Serializable]
    public class StageInfos
    {
        public StageInfo[] StageData;
    }

    [SerializeField]
    StageInfos DataList;

    void Awake()
    {
        string JsonData = Resources.Load<TextAsset>("JsonData/StageData").ToString();

        DataList = JsonUtility.FromJson<StageInfos>(JsonData);
    }

    public StageInfo GetStageInfo(int ID)
    {
        return DataList.StageData[ID];
    }
}
