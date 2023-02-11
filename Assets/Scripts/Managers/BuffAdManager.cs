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
    [Tooltip("���� ����")]
    public int level;

    [Tooltip("���� ����ġ")]
    public int exp;

    [Tooltip("���� ���� ���� �ð�(��)")]
    public int durationTime;

    [Tooltip("���� ���� �ð� ���� ���� ����")]
    public float renewalBaseTime;

    [Tooltip("���� ȿ�� ��ġ(%)")]
    public int percentageFigure;

    [Tooltip("���� ȿ�� ��ġ ������(%)")]
    public int increasedValue;

    [Tooltip("���� ���� ȿ�� ���� ������ �Ǻ�")]
    public bool isDuration;

    [Tooltip("���� ��û ��ư ������Ʈ")]
    public GameObject watchAdButtonObj;

    [Tooltip("����ġ �� �̹���")]
    public Image expBarImage;

    [Tooltip("���� ȿ�� ���� �ؽ�Ʈ")]
    public Text buffEffectText;

    [Tooltip("���� ���� �ؽ�Ʈ")]
    public Text buffLevelText;

    [Tooltip("���� ����ġ �ؽ�Ʈ")]
    public Text buffExpText;

    [Tooltip("���� ���� �ð� �ؽ�Ʈ")]
    public Text durationTimeText;
}

public class BuffAdManager : MonoBehaviour
{
    private RewardedAd rewardedAd;

    AdRequest request;

    [Tooltip("���� ���̵�")]
    const string AD_UNIT_ID = "ca-app-pub-3940256099942544/5224354917";

    [Tooltip("���� ���� �����͵�")]
    public BuffAdData[] adDatas;

    [Tooltip("���� ����ġ �ִ�")]
    const int MAX_EXP = 3;

    [Tooltip("���� ȿ�� ���ӽð�(��)")]
    const int DURATION_TIME = 20;

    [Tooltip("���� ���� ȿ�� ���������� �Ǻ�")]
    bool isCoolTimeCounting;

    [Tooltip("���� ���� �ε���")]
    int nowAdIndex;

    void Start()
    {
        RequestRewardedAd();
    }

    /// <summary>
    /// ���� �ҷ�����
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
    /// ���� ������ ��û���� ��
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void HandleUserEarnedReward(object sender, Reward args)
    {
        BuffAdData nowAdDatas = adDatas[nowAdIndex];

        //����� �Ҹ� ����

        nowAdDatas.exp++;

        if (nowAdDatas.exp >= MAX_EXP) //���� ������ �� ����
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
    /// ���� ����Ǿ��� ��
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardedAd();
    }

    /// <summary>
    /// ���� ��ư Ŭ�� �� ���� �Լ�
    /// </summary>
    /// <param name="adIndex"> ���� ��û�� ���� �ε��� </param>
    public void ClickAdButton(int adIndex)
    {
        nowAdIndex = adIndex;

        if (rewardedAd.IsLoaded()) //���� ȣ��
        {
            //����� �Ҹ� �ѱ�

            rewardedAd.Show();
        }
        else
        {
            RequestRewardedAd();
        }
    }

    /// <summary>
    /// ���� ���û ��Ÿ�� �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator RenewAdCoolTime()
    {
        int coolTimeEndCount; //���� ��Ȱ��ȭ�� ���� ����

        while (true)
        {
            coolTimeEndCount = 0;

            for (int adCoolTimeIndex = 0; adCoolTimeIndex < 3; adCoolTimeIndex++)
            {
                if (adDatas[adCoolTimeIndex].isDuration) //���� ��û �� Ȱ��ȭ�� ���� �ð� �����͸� ����ϱ�
                {
                    adDatas[adCoolTimeIndex].renewalBaseTime += Time.deltaTime;

                    if (adDatas[adCoolTimeIndex].renewalBaseTime >= 60) //���� �ʰ� 1���� ������ ������ ����
                    {
                        adDatas[adCoolTimeIndex].renewalBaseTime = 0;

                        adDatas[adCoolTimeIndex].durationTime--;

                        if (adDatas[adCoolTimeIndex].durationTime <= 0)
                        {
                            adDatas[adCoolTimeIndex].isDuration = false;

                            adDatas[adCoolTimeIndex].watchAdButtonObj.SetActive(true);

                            adDatas[adCoolTimeIndex].durationTime = DURATION_TIME;
                        }

                        adDatas[adCoolTimeIndex].durationTimeText.text = $"{adDatas[adCoolTimeIndex].durationTime}��";
                    }
                }
                else
                {
                    coolTimeEndCount++;
                }
            }

            if (coolTimeEndCount == 3) //���� ������ ��� ��Ȱ��ȭ �Ǿ��ٸ�, ���� ����
            {
                isCoolTimeCounting = false;
                break;
            }

            yield return null;
        }
    }
}
