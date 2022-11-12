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

        SetResolution();
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

    private void SetResolution()
    {
        int setWidth = 1440; // 사용자 설정 너비
        int setHeight = 2960; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }

}
