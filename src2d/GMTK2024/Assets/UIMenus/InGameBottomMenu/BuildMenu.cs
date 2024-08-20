using System;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] GameObject buildBGContour;

    private IGameManager gameManager;
    private bool lastBuildActiveState;
    void Start()
    {
        gameManager = GameManager.Instance;
        if (buildBGContour)
        {
            buildBGContour.SetActive(gameManager.BuildActive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (root.activeInHierarchy != gameManager.BuildMenu)
        {
            root.SetActive(gameManager.BuildMenu);
        }

        if (lastBuildActiveState != gameManager.BuildActive)
        {
            RefreshUI();
            lastBuildActiveState = gameManager.BuildActive;
        }
    }

    private void RefreshUI()
    {
        if (buildBGContour && buildBGContour.activeInHierarchy != gameManager.BuildActive)
        {
            buildBGContour.SetActive(gameManager.BuildActive);
        }
    }
}
