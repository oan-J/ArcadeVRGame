// BulletScript.cs
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float lifeTime = 5f;

// 如果你想添加射击特效，可以在BulletScript中添加拖尾效果：
void Start()
{
    // 添加拖尾效果
    TrailRenderer trail = gameObject.AddComponent<TrailRenderer>();
    trail.time = 0.1f;  // 拖尾持续时间
    trail.startWidth = 0.05f;
    trail.endWidth = 0.0f;
    trail.material = new Material(Shader.Find("Sprites/Default"));
    
    // 设置子弹速度
    GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
    Destroy(gameObject, lifeTime);
}

    void OnCollisionEnter(Collision collision)
    {
        // 碰撞到物体时销毁子弹
        Destroy(gameObject);
    }
}