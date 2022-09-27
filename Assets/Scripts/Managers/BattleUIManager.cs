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
    [Tooltip("현재 보여지는 콘텐츠 창 오브젝트")]
    private GameObject nowContentsPanelObj;

    [SerializeField]
    [Tooltip("콘텐츠 창 오브젝트 모음")]
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
        //팝업 다트윈 애니메이션 넣기
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
