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
        int setWidth = 1440; // ����� ���� �ʺ�
        int setHeight = 2960; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }

}
