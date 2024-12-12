// GunInput.cs - 修改版
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GunInput : MonoBehaviour
{
    private XRDirectInteractor interactor;
    private PlayerInput playerInput;

    void Start()
    {
        interactor = GetComponent<XRDirectInteractor>();
        playerInput = FindObjectOfType<PlayerInput>();
        
        // 订阅输入事件
        if (playerInput != null)
        {
            playerInput.actions["ButtonA"].performed += OnButtonAPressed;
        }
    }

    private void OnButtonAPressed(InputAction.CallbackContext context)
    {
        if (interactor.selectTarget != null)
        {
            GunController gun = interactor.selectTarget.GetComponent<GunController>();
            if (gun != null)
            {
                gun.Shoot();
            }
        }
    }

    void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.actions["ButtonA"].performed -= OnButtonAPressed;
        }
    }
}