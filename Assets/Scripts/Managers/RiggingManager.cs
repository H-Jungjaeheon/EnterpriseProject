using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RiggingType
{
    Shoes,
    Bowl,
    Spoon
}

[System.Serializable]
public class Rigging
{
    [Tooltip("��� ID")]
    public int id;

    [Tooltip("��� ����")]
    public RiggingType type;

    [Tooltip("��� ������� ����")]
    public bool isUnlock;

    [Tooltip("��� ������")]
    public Sprite icon;

    [Tooltip("��� �̸�")]
    public string name;

    [Tooltip("��� ����")]
    public string explanation;

    [Tooltip("��� ���")]
    public int rank;

    [Tooltip("��� ����")]
    public int level;

    [Tooltip("�ִ� ��� ����ġ")]
    public int maxExp;

    [Tooltip("���� ��� ����ġ")]
    public int nowExp;
}

public class RiggingManager : MonoBehaviour
{
    [Tooltip("��� �����͵�")]
    public Rigging[] riggingDatas;

    [SerializeField]
    [Tooltip("��� ���� ��ư��")]
    private Button[] equipButtons;

    [SerializeField]
    [Tooltip("��� ���� ��ư �̹�����")]
    private Image[] buttonImage;

    /// <summary>
    /// ��� ���� ȭ�� Ȱ��ȭ �� �Ź� ������ ����� ��ư ����
    /// </summary>
    private void OnEnable()
    {
        ButtonSetting(0);
    }

    /// <summary>
    /// ��� ���� ��ư�� ��� ���� ����(��ư)
    /// </summary>
    /// <param name="typeIndex"> ��� ���� �ε���(RiggingType) </param>
    public void ButtonSetting(int typeIndex)
    {
        RiggingType riggingType = (RiggingType)typeIndex; //���� ��� Ÿ��
        int buttonIndex = 0; //���� ��ư �ε���

        for (int nowIndex = riggingDatas.Length - 1; nowIndex >= 0; nowIndex--)
        {
            if (riggingDatas[nowIndex].type == riggingType && riggingDatas[nowIndex].isUnlock)
            {
                //equipButtons[buttonIndex].onClick.AddListener 
                buttonImage[buttonIndex].sprite = riggingDatas[nowIndex].icon;

                buttonIndex++;
            }
        }
    }
}
