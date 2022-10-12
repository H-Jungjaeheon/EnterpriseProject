using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    float MoveSpeed;
    [SerializeField]
    RectTransform NewSpawnPos;
    [SerializeField]
    Vector2 EndMovePos;
    [SerializeField]
    Player player;

    void Awake()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        MoveSpeed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveSpeed = ((player.transform.position.x - player.StendPosX) * -5) * Time.deltaTime;

        if (MoveSpeed > 0)
            MoveSpeed = 0.0f;

        this.transform.position += new Vector3(MoveSpeed, 0, 0);

        if (rectTransform.anchoredPosition.x <= EndMovePos.x)
            rectTransform.anchoredPosition = new Vector2(NewSpawnPos.anchoredPosition.x + 4000 - 10, NewSpawnPos.anchoredPosition.y);
    }
}
