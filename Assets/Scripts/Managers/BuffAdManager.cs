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
    [Tooltip("���� ����")]
    public int level;

    [Tooltip("���� ����ġ")]
    public int exp;

    [Tooltip("���� ���� �ð�")]
    public int durationTime;

    [TextArea]
    [Tooltip("���� ȿ�� ����")]
    public string buffEffect;

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
    [Tooltip("���� ���� �����͵�")]
    public BuffAdData[] adDatas;

    [Tooltip("���� ����ġ �ִ�")]
    const int MAX_EXP = 3;

    [Tooltip("���� ȿ�� ���ӽð�")]
    const int DURATION_TIME = 1200;

    [Tooltip("���� ���� ȿ�� ���������� �ǔ�")]
    bool isCoolTimeCounting;

    /// <summary>
    /// ���� ��ư Ŭ�� �� ���� �Լ�
    /// </summary>
    /// <param name="adIndex"> ���� ��û�� ���� �ε��� </param>
    public void ClickAdButton(int adIndex)
    {
        BuffAdData nowAdDatas = adDatas[adIndex];

        nowAdDatas.exp++;
        
        if (nowAdDatas.exp >= MAX_EXP) //���� ������ ��
        {
            nowAdDatas.level++;
            nowAdDatas.exp = 0;
            //���� ���� �ٲ�� �ڵ� �ۼ�
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

        //���� ��û ����
    }

    /// <summary>
    /// ���� ���û ��Ÿ�� �Լ�
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
    /// ���� ����
    /// </summary>
    void DataReSetting()
    {
        
    }

}
