using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Contents
{
    StatUpgradeContents,
    TestContents,
    ContentsLength
}

public class BattleUIManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("���� �������� ������ â ������Ʈ")]
    private GameObject nowContentsPanelObj;

    [SerializeField]
    [Tooltip("������ â ������Ʈ ����")]
    private GameObject[] contentsPanelObjs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnotherContentsPopUp(GameObject PopUpObj)
    {
        //�˾� ��Ʈ�� �ִϸ��̼� �ֱ�
        GameObject lastViewContents = PopUpObj;
        if (PopUpObj.activeSelf)
        {
            nowContentsPanelObj = contentsPanelObjs[(int)Contents.StatUpgradeContents];
        }
        else
        {
            nowContentsPanelObj = PopUpObj;
        }
        lastViewContents.SetActive(false);
        nowContentsPanelObj.SetActive(true);
    }

    public void AnotherContentsChangeScrollView(GameObject PopUpObj)
    {
        GameObject lastViewContents = PopUpObj;
        if (PopUpObj.activeSelf)
        {
            nowContentsPanelObj = contentsPanelObjs[(int)Contents.StatUpgradeContents];
        }
        else
        {
            nowContentsPanelObj = PopUpObj;
        }
        lastViewContents.SetActive(false);
        nowContentsPanelObj.SetActive(true);
    }
}
