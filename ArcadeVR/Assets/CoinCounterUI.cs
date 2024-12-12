using UnityEngine;
using TMPro;  // 如果使用TextMeshPro
using UnityEngine.UI;

public class CoinCounterUI : MonoBehaviour 
{
    private Camera mainCamera;
    private Canvas canvas;
    private RectTransform rectTransform;
    
    [SerializeField]
    private Vector3 offset = new Vector3(80f, 50f, 2f); // UI偏移量
    
    void Start()
    {
        // 获取主相机引用
        mainCamera = Camera.main;
        canvas = GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        
        if (mainCamera == null)
        {
            Debug.LogError("找不到主相机！");
            return;
        }
        
        // 设置画布为相机的子对象
        canvas.worldCamera = mainCamera;
        
        // 将画布放置在相机前方
        transform.SetParent(mainCamera.transform);
        transform.localPosition = offset;
        transform.localRotation = Quaternion.identity;
    }
    
    void LateUpdate()
    {
        // 确保UI始终面向相机
        transform.rotation = mainCamera.transform.rotation;
        
        // 保持在固定位置
        transform.position = mainCamera.transform.position + mainCamera.transform.TransformVector(offset);
    }
    
    // 更新金币数量的方法
    public void UpdateCoinCount(int count)
    {
        TextMeshProUGUI coinText = GetComponentInChildren<TextMeshProUGUI>();
        if (coinText != null)
        {
            coinText.text = "金币: " + count.ToString();
        }
    }
}