using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuffAdData
{
    [TextArea]
    [Tooltip("버프 효과 내용")]
    public string buffEffect;

    [Tooltip("광고 레벨")]
    public int level;

    [Tooltip("광고 경험치")]
    public int exp;

    [Tooltip("버프 남은 시간")]
    public int durationTime;

    [Tooltip("현재 광고 효과 지속 중인지 판별")]
    public bool isDuration;

    [Tooltip("광고 시청 버튼 오브젝트")]
    public GameObject watchAdButtonObj;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
