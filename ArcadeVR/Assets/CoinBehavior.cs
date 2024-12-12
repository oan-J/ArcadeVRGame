using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CoinBehavior : MonoBehaviour
{
    public AudioClip coinCollectSound;
    public float soundVolume = 3f; // Added volume control, increase this value to make it louder
    private AudioSource audioSource;
    private CoinBoard coinBoard;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        // Add AudioSource if it doesn't exist
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configure AudioSource
        audioSource.playOnAwake = false;
        audioSource.clip = coinCollectSound;
        audioSource.spatialBlend = 1f; // Full 3D sound
        audioSource.volume = soundVolume;

        // Find CoinBoard
        coinBoard = FindObjectOfType<CoinBoard>();
        
        // Setup grab interaction
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnCoinGrabbed);
        }
    }

    private void OnCoinGrabbed(SelectEnterEventArgs args)
    {
        // Play sound at the coin's position with increased volume
        if (coinCollectSound != null)
        {
            AudioSource.PlayClipAtPoint(coinCollectSound, transform.position, soundVolume);
        }
        else
        {
            Debug.LogWarning("Coin collect sound is not assigned!");
        }

        // Update score
        if (coinBoard != null)
        {
            coinBoard.AddCoin();
        }

        // Destroy the coin
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnCoinGrabbed);
        }
    }
}