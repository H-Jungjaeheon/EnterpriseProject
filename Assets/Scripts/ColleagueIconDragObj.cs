using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColleagueIconDragObj : MonoBehaviour, IDragHandler
{
    [SerializeField]
    [Tooltip("아이콘 인덱스")]
    private int iconIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        print("w");
        transform.position = eventData.position;
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        //transform.position = mousePosition;
    }
}
