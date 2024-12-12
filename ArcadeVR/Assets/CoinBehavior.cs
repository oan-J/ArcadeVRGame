using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CoinBehavior : MonoBehaviour
{
    public AudioClip coinCollectSound;
    private AudioSource audioSource;
    private CoinBoard coinBoard;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = coinCollectSound;

        coinBoard = FindObjectOfType<CoinBoard>();
        
        // Get the XRGrabInteractable component and subscribe to its event
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnCoinGrabbed);
        }
    }

    private void OnCoinGrabbed(SelectEnterEventArgs args)
    {
        if (audioSource != null && coinCollectSound != null)
        {
            AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
        }

        if (coinBoard != null)
        {
            coinBoard.AddCoin();
        }

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        // Unsubscribe when destroyed
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnCoinGrabbed);
        }
    }
}