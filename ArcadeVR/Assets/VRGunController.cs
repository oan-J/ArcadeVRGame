using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class VRGunController : MonoBehaviour
{
    private InputDevice rightController;
    private bool controllerFound = false;
    public Transform firePoint;
    public float shootRange = 100f;
    private MeshRenderer gunRenderer;
    
    // Audio variables
    private AudioSource audioSource;
    public AudioClip shootSound;
    
    // Coin prefab reference
    public GameObject coinPrefab;  // Assign your coin prefab in Inspector
    
    private const string CHICK_TAG = "CyberChick";
    private Dictionary<GameObject, int> hitCounts = new Dictionary<GameObject, int>();
    private Color originalGunColor;
    private Dictionary<GameObject, Color> originalChickColors = new Dictionary<GameObject, Color>();

    private float gunFlashDuration = 0.1f;
    private float chickFlashDuration = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = 0.7f;

        gunRenderer = GetComponent<MeshRenderer>();
        if (gunRenderer == null)
        {
            Debug.LogError("No MeshRenderer found on the gun!");
        }
        else
        {
            originalGunColor = gunRenderer.material.color;
        }

        if (firePoint == null)
        {
            firePoint = transform;
        }
        
        FindRightController();
    }

    void FindRightController()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller,
            inputDevices);

        if (inputDevices.Count > 0)
        {
            rightController = inputDevices[0];
            controllerFound = true;
        }
    }

    void Update()
    {
        if (!controllerFound)
        {
            FindRightController();
            return;
        }

        bool primaryButtonValue;
        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue))
        {
            if (primaryButtonValue)
            {
                ShootChick();
            }
        }
    }

    void FlashColor(GameObject obj, Color flashColor)
    {
        MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
        {
            if (!originalChickColors.ContainsKey(obj))
            {
                originalChickColors[obj] = renderer.material.color;
            }
            renderer.material.color = flashColor;
        }

        StartCoroutine(ResetColor(obj));
    }

    System.Collections.IEnumerator ResetColor(GameObject obj)
    {
        yield return new WaitForSeconds(chickFlashDuration);

        if (obj != null && originalChickColors.ContainsKey(obj))
        {
            MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.material.color = originalChickColors[obj];
            }
        }
    }

    System.Collections.IEnumerator FlashGunColor()
    {
        if (gunRenderer != null)
        {
            gunRenderer.material.color = Color.red;
            yield return new WaitForSeconds(gunFlashDuration);
            gunRenderer.material.color = originalGunColor;
        }
    }
void SpawnCoin(Vector3 position)
{
    if (coinPrefab != null)
    {
        Instantiate(coinPrefab, position, Quaternion.identity);
    }
    else
    {
        Debug.LogWarning("Coin prefab not assigned!");
    }
}

    void DestroyChick(GameObject chick)
    {
        originalChickColors.Remove(chick);
        hitCounts.Remove(chick);
        Destroy(chick);
    }

    void ShootChick()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        
        StartCoroutine(FlashGunColor());
        
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, shootRange))
        {
            if (hit.transform.CompareTag(CHICK_TAG))
            {
                GameObject hitChick = hit.transform.gameObject;
                
                if (!hitCounts.ContainsKey(hitChick))
                {
                    hitCounts[hitChick] = 0;
                }
                
                hitCounts[hitChick]++;
                int currentHits = hitCounts[hitChick];

                FlashColor(hitChick, Color.yellow);

                // Spawn coin at chick's position with same scale
                SpawnCoin(hitChick.transform.position);

                float currentScale = hitChick.transform.localScale.x;
                if (currentScale < 5f)
                {
                    if (currentHits >= 2)
                    {
                        DestroyChick(hitChick);
                    }
                    else
                    {
                        hitChick.transform.localScale = hitChick.transform.localScale / 2f;
                    }
                }
                else
                {
                    if (currentHits < 4)
                    {
                        hitChick.transform.localScale = hitChick.transform.localScale / 2f;
                    }
                    else if (currentHits == 4)
                    {
                        DestroyChick(hitChick);
                    }
                }
            }
        }
    }
}