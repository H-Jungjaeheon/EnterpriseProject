using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StageInfo
{
    //스테이지 전체 정보
    public int ID;
    public int StageNumber;
    public string IsBossStage;

    //몬스터 스폰 수
    public int Shrot_1;
    public int Long_1;
    public int Air_1;

    public int Shrot_2;
    public int Long_2;
    public int Air_2;

    public int Shrot_3;
    public int Long_3;
    public int Air_3;
}
