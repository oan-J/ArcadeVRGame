using UnityEngine;
using UnityEngine.XR;

public class VRUIFollow : MonoBehaviour
{
    [SerializeField] private float distance = 1f; // UI与相机的距离
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private Vector3 offset = new Vector3(0.3f, 0.2f, 0f); // 右上角偏移

    private Camera xrCamera;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        // 获取XR相机
        xrCamera = Camera.main;
        if (xrCamera == null)
        {
            Debug.LogError("找不到XR相机！");
            return;
        }

        // 设置初始位置和大小
        transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
    }

    void LateUpdate()
    {
        if (xrCamera == null) return;

        // 计算目标位置：相机位置 + 相机前方 * 距离 + 偏移
        Vector3 forward = xrCamera.transform.forward;
        Vector3 right = xrCamera.transform.right;
        Vector3 up = xrCamera.transform.up;

        targetPosition = xrCamera.transform.position + 
                        forward * distance +
                        right * offset.x +
                        up * offset.y;

        // 始终面向相机
        targetRotation = Quaternion.LookRotation(transform.position - xrCamera.transform.position);

        // 平滑移动
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}