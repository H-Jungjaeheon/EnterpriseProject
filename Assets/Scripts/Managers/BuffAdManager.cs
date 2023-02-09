using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuffAdData
{
    [TextArea]
    [Tooltip("���� ȿ�� ����")]
    public string buffEffect;

    [Tooltip("���� ����")]
    public int level;

    [Tooltip("���� ����ġ")]
    public int exp;

    [Tooltip("���� ���� �ð�")]
    public int durationTime;

    [Tooltip("���� ���� ȿ�� ���� ������ �Ǻ�")]
    public bool isDuration;

    [Tooltip("���� ��û ��ư ������Ʈ")]
    public GameObject watchAdButtonObj;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
