using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class UIController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Animator buildButtonAnimator;
    [SerializeField] private Animator destroyButtonAnimator;

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
}
