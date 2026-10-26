using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndCrab : MonoBehaviour
{
    private InputSystem_Actions _inputActions;
    [SerializeField] private int coinsRequired = 10;
    [SerializeField] private GameObject coinRequirement;
    [SerializeField] private GameObject actionPrompt;
    [SerializeField] private int nextScene = -1;

    void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Interact.Enable();
        
        TextMeshPro text = coinRequirement.GetComponentInChildren<TextMeshPro>();
        text.text = $"YOU ARE TOO BROKE";
        
        text = actionPrompt.GetComponentInChildren<TextMeshPro>();
        text.text = $"PRESS E TO GO\n TO THE NEXT LEVEL";
        
        CoinBar.Instance.SetNeeded(coinsRequired);
        if (nextScene == -1)
            nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    void OnDisable()
    {
        _inputActions.Player.Interact.performed -= GoToNextLevel;
        _inputActions.Player.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory player = other.GetComponent<PlayerInventory>();

        if (IsEnoughCoins(player))
        {
            _inputActions.Player.Interact.performed += GoToNextLevel;
            actionPrompt.SetActive(true);
        }
        else
        {
            coinRequirement.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _inputActions.Player.Interact.performed -= GoToNextLevel;
        actionPrompt.SetActive(false);
        coinRequirement.SetActive(false);
    }

    private void GoToNextLevel(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(nextScene);
    }

    private bool IsEnoughCoins(PlayerInventory player)
    {
        return player.CollectiblesCount[CoinType.Coin] >= coinsRequired;
    }
}
