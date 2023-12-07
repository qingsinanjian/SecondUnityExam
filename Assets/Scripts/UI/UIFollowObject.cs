using UnityEngine;
using UnityEngine.UI;

public class UIFollowObject : MonoBehaviour
{
    public Transform targetObject; // 需要跟随的物体
    public Vector3 offset = new Vector3(0f, 2f, 0f); // 相对于物体的偏移量
    public Camera mainCamera; // 主摄像机

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
            // 将物体的世界坐标转换为屏幕坐标
            Vector3 screenPos = mainCamera.WorldToScreenPoint(targetObject.position + offset);

            // 更新UI元素的位置
            uiRectTransform.position = screenPos;
        }
    }
}
