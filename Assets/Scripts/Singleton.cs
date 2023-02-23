using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    protected bool isDontDestroyObj;

    private static T gameManager = null;
    public static T Instance
    {
        get
        {
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<T>();
                if (gameManager == null)
                {
                    gameManager = new GameObject().AddComponent<T>();
                }
            }
            return gameManager;
        }
    }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        if (isDontDestroyObj)
        {
            if (gameManager == null)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
