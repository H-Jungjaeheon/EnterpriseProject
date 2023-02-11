using System;
using System.Collections;
using GoogleMobileAds.Api;
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

    [Tooltip("현재 버프 남은 시간(분)")]
    public int durationTime;

    [Tooltip("버프 남은 시간 갱신 기준 변수")]
    public float renewalBaseTime;

    [Tooltip("버프 효과 수치(%)")]
    public int percentageFigure;

    [Tooltip("버프 효과 수치 증가값(%)")]
    public int increasedValue;

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
    private RewardedAd rewardedAd;

    AdRequest request;

    [Tooltip("광고 아이디")]
    const string AD_UNIT_ID = "ca-app-pub-3940256099942544/5224354917";

    [Tooltip("버프 광고 데이터들")]
    public BuffAdData[] adDatas;

    [Tooltip("광고 경험치 최댓값")]
    const int MAX_EXP = 3;

    [Tooltip("광고 효과 지속시간(분)")]
    const int DURATION_TIME = 20;

    [Tooltip("현재 광고 효과 지속중인지 판별")]
    bool isCoolTimeCounting;

    [Tooltip("현재 광고 인덱스")]
    int nowAdIndex;

    void Start()
    {
        RequestRewardedAd();
    }

    /// <summary>
    /// 광고 불러오기
    /// </summary>
    void RequestRewardedAd()
    {
        rewardedAd = new RewardedAd(AD_UNIT_ID);

        request = new AdRequest.Builder().Build();

        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        rewardedAd.LoadAd(request);
    }

    /// <summary>
    /// 광고를 끝까지 시청했을 때
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void HandleUserEarnedReward(object sender, Reward args)
    {
        BuffAdData nowAdDatas = adDatas[nowAdIndex];

        //배경음 소리 끄기

        nowAdDatas.exp++;

        if (nowAdDatas.exp >= MAX_EXP) //버프 레벨업 시 실행
        {
            nowAdDatas.exp = 0;

            nowAdDatas.level++;
            nowAdDatas.percentageFigure += nowAdDatas.increasedValue;

            nowAdDatas.buffEffectText.text = $"{nowAdDatas.percentageFigure}%";
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
    }

    /// <summary>
    /// 광고가 종료되었을 때
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardedAd();
    }

    /// <summary>
    /// 광고 버튼 클릭 시 실행 함수
    /// </summary>
    /// <param name="adIndex"> 현재 시청할 광고 인덱스 </param>
    public void ClickAdButton(int adIndex)
    {
        nowAdIndex = adIndex;

        if (rewardedAd.IsLoaded()) //광고 호출
        {
            //배경음 소리 켜기

            rewardedAd.Show();
        }
        else
        {
            RequestRewardedAd();
        }
    }

    /// <summary>
    /// 광고 재시청 쿨타임 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator RenewAdCoolTime()
    {
        int coolTimeEndCount; //현재 비활성화된 버프 개수

        while (true)
        {
            coolTimeEndCount = 0;

            for (int adCoolTimeIndex = 0; adCoolTimeIndex < 3; adCoolTimeIndex++)
            {
                if (adDatas[adCoolTimeIndex].isDuration) //광고 시청 후 활성화된 버프 시간 데이터만 계산하기
                {
                    adDatas[adCoolTimeIndex].renewalBaseTime += Time.deltaTime;

                    if (adDatas[adCoolTimeIndex].renewalBaseTime >= 60) //기준 초가 1분이 넘으면 데이터 갱신
                    {
                        adDatas[adCoolTimeIndex].renewalBaseTime = 0;

                        adDatas[adCoolTimeIndex].durationTime--;

                        if (adDatas[adCoolTimeIndex].durationTime <= 0)
                        {
                            adDatas[adCoolTimeIndex].isDuration = false;

                            adDatas[adCoolTimeIndex].watchAdButtonObj.SetActive(true);

                            adDatas[adCoolTimeIndex].durationTime = DURATION_TIME;
                        }

                        adDatas[adCoolTimeIndex].durationTimeText.text = $"{adDatas[adCoolTimeIndex].durationTime}분";
                    }
                }
                else
                {
                    coolTimeEndCount++;
                }
            }

            if (coolTimeEndCount == 3) //현재 버프가 모두 비활성화 되었다면, 연산 정지
            {
                isCoolTimeCounting = false;
                break;
            }

            yield return null;
        }
    }
}
