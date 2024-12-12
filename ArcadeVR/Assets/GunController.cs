using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class GunController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;  // 子弹预制体
    [SerializeField] private Transform shootPoint;     // 射击点
    [SerializeField] private float shootDelay = 0.5f;

    private XRGrabInteractable grabInteractable;
    private bool canShoot = true;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        // 创建并设置附着点
        GameObject attachPoint = new GameObject("AttachPoint");
        attachPoint.transform.SetParent(transform);
        attachPoint.transform.localPosition = Vector3.zero;
        // 设置正确的旋转，使枪在抓取时旋转-90度
        attachPoint.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        
        // 设置抓取点
        grabInteractable.attachTransform = attachPoint.transform;
        
        // 配置Grab Interactable
        grabInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        grabInteractable.throwOnDetach = false;
    }

    public void Shoot()
    {
        if (!canShoot) return;
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        canShoot = false;

        // 在射击点生成子弹
        if (bulletPrefab != null && shootPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            // 子弹会通过BulletScript自动向前飞行
        }

        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}