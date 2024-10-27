using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class UIController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Animator buildButtonAnimator;
    [SerializeField] private Animator destroyButtonAnimator;
    [SerializeField] private Animator mainMenuAnimator;
    [SerializeField] private Animator restartGameAnimator;
    [SerializeField] private Animator photoModeAnimator;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void Start()
    {
        SetBuildButtonActive();
    }

    public void SetBuildButtonActive()
    {
        buildButtonAnimator.SetBool("Active", true);
        destroyButtonAnimator.SetBool("Active", false);

        gameManager.SetWorkModeToBuild();
    }

    public void SetDestroyButtonActive()
    {
        buildButtonAnimator.SetBool("Active", false);
        destroyButtonAnimator.SetBool("Active", true);

        gameManager.SetWorkModeToDestroy();
    }

    public void SetMenuActive(bool active)
    {
        mainMenuAnimator.SetBool("Active", active);
    }

    public void SetRestartGameActive(bool active)
    {
        restartGameAnimator.SetBool("Active", active);

        gameManager.SetGamePaused(active);
    }

    public void StartRestartGame()
    {
        restartGameAnimator.SetTrigger("Restart");
    }

    public void SetPhotoModeActive(bool active)
    {
        photoModeAnimator.SetBool("Active", active);
    }
}
