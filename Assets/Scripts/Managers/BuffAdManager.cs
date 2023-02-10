using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuffAdKind
{
    FirstBuff,
    SecondBuff,
    ThirdBuff
}

[System.Serializable]
public class BuffAdData
{
    [Tooltip("광고 레벨")]
    public int level;

    [Tooltip("광고 경험치")]
    public int exp;

    [Tooltip("버프 남은 시간")]
    public int durationTime;

    [TextArea]
    [Tooltip("버프 효과 내용")]
    public string buffEffect;

    [Tooltip("현재 광고 효과 지속 중인지 판별")]
    public bool isDuration;

    [Tooltip("광고 시청 버튼 오브젝트")]
    public GameObject watchAdButtonObj;

    [Tooltip("경험치 바 이미지")]
    public Image expBarImage;

    [Tooltip("버프 효과 내용 텍스트")]
    public Text buffEffectText;

    [Tooltip("버프 레벨 텍스트")]
    public Text buffLevelText;

    [Tooltip("버프 경험치 텍스트")]
    public Text buffExpText;

    [Tooltip("버프 남은 시간 텍스트")]
    public Text durationTimeText;
}

public class BuffAdManager : MonoBehaviour
{
    [Tooltip("버프 광고 데이터들")]
    public BuffAdData[] adDatas;

    [Tooltip("광고 경험치 최댓값")]
    const int MAX_EXP = 3;

    [Tooltip("광고 효과 지속시간")]
    const int DURATION_TIME = 1200;

    [Tooltip("현재 광고 효과 지속중인지 판뵬")]
    bool isCoolTimeCounting;

    /// <summary>
    /// 광고 버튼 클릭 시 실행 함수
    /// </summary>
    /// <param name="adIndex"> 현재 시청할 광고 인덱스 </param>
    public void ClickAdButton(int adIndex)
    {
        BuffAdData nowAdDatas = adDatas[adIndex];

        nowAdDatas.exp++;
        
        if (nowAdDatas.exp >= MAX_EXP) //버프 레벨업 시
        {
            nowAdDatas.level++;
            nowAdDatas.exp = 0;
            //버프 내용 바뀌는 코드 작성
        }

        nowAdDatas.buffLevelText.text = $"{nowAdDatas.level}";
        nowAdDatas.buffExpText.text = $"{nowAdDatas.exp}/{MAX_EXP}";

        nowAdDatas.expBarImage.fillAmount = nowAdDatas.exp / MAX_EXP;

        nowAdDatas.isDuration = true;
        nowAdDatas.watchAdButtonObj.SetActive(false);

        if (isCoolTimeCounting == false)
        {
            isCoolTimeCounting = true;

            StartCoroutine(RenewAdCoolTime());
        }

        //광고 시청 시작
    }

    /// <summary>
    /// 광고 재시청 쿨타임 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator RenewAdCoolTime()
    {
        while (true)
        {

            yield return null;
        }
    }

    /// <summary>
    /// 정보 수정
    /// </summary>
    void DataReSetting()
    {
        
    }

}
