using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private Coroutine ShakeCorutine;

    [SerializeField]
    private float ShakeTime;
    [SerializeField]
    private float Intensity;

    private void Awake()
    {
        Instance = this;
    }

    public void OnCameraShake(float ShakeTime = 1.0f, float Intensity = 0.1f)
    {
        this.ShakeTime = ShakeTime;
        this.Intensity = Intensity;

        StartShake();
    }

    private void StartShake()
    {
        ShakeCorutine = StartCoroutine(Shake());
    }

    private void StopShake()
    {
        StopCoroutine(ShakeCorutine);
    }

    IEnumerator Shake()
    {
        yield return null;

        Vector2 StartPos = this.transform.position;

        while (ShakeTime > 0.0f)
        {
            yield return null;

            transform.position = StartPos + Random.insideUnitCircle * Intensity;
            transform.position += new Vector3(0, 0, -10);

            ShakeTime -= Time.deltaTime;
        }

        transform.position = StartPos;
        transform.position += new Vector3(0, 0, -10);

        StopShake();
    }
}
