using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    #region ATTRIBUTS
    [SerializeField] private UIDocument mainMenuDocument = null;

    private Button startButton = null;
    private Button quitButton = null;

    void OnEnable()
    {
        startButton = mainMenuDocument.rootVisualElement.Q<Button>("Start");
        startButton.clicked += OnStartclicked;

        quitButton = mainMenuDocument.rootVisualElement.Q<Button>("Quit");
        quitButton.clicked += OnQuitClicked;
    }

    void OnDisable()
    {
        startButton.clicked -= OnStartclicked;
        quitButton.clicked -= OnQuitClicked;
    }

    private void OnStartclicked()
    {
        Debug.Log("Start");
    }

    private void OnQuitClicked()
    {
        Debug.Log("Quit");
    }
    #endregion ATTRIBUTS
}
