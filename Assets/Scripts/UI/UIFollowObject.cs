using UnityEngine;
using UnityEngine.UI;

public class UIFollowObject : MonoBehaviour
{
    public Transform targetObject; // ��Ҫ���������
    public Vector3 offset = new Vector3(0f, 2f, 0f); // ����������ƫ����
    public Camera mainCamera; // �������

    private RectTransform uiRectTransform;

    void Start()
    {
        uiRectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (targetObject != null)
        {
            // ���������������ת��Ϊ��Ļ����
            Vector3 screenPos = mainCamera.WorldToScreenPoint(targetObject.position + offset);

            // ����UIԪ�ص�λ��
            uiRectTransform.position = screenPos;
        }
    }
}
