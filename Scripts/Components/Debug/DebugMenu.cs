using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class DebugMenu : MonoBehaviour
    {
        private const float DistanceFromCamera = 2f;

        public static DebugMenu Instance { get; private set; } = null;

        [SerializeField] private GameObject canvas;
        [SerializeField] private TextMeshProUGUI pageCaption;
        [SerializeField] Button nextPageButton;
        [SerializeField] Button previousPageButton;
        [SerializeField] Button resetButton;
        
        private int currentPageIndex = 0;
        private Camera mainCamera;
        private DebugMenuPage[] pages;

        private void Start()
        {
            if (Instance != null)
            {
                throw new Exception("Multiple debug menu instances.");
            }

            Instance = this;
            pages = GetComponentsInChildren<DebugMenuPage>();
            canvas.SetActive(false);
            DontDestroyOnLoad(this.gameObject);
            mainCamera = Camera.main;
            InitializePaging();
            resetButton.onClick.AddListener(TriggerReset);

            foreach (var page in pages)
            {
                page.Initialize();
            }
        }

        private void InitializePaging()
        {
            nextPageButton.onClick.AddListener(GoToNextPage);
            previousPageButton.onClick.AddListener(GoToPreviousPage);
            UpdatePage();
        }

        private void TriggerReset()
        {
            pages[currentPageIndex].ResetValues();
        }

        private void UpdatePage()
        {
            foreach (var page in pages)
            {
                page.gameObject.SetActive(page == pages[currentPageIndex]);
            }

            var currentPage = pages[currentPageIndex];
            pageCaption.text = currentPage.name;

            var currentPageType = currentPage.GetType();
            var showResetButton = currentPageType.GetMethod(nameof(DebugMenuPage.ResetValues)).DeclaringType == currentPageType;
            resetButton.gameObject.SetActive(showResetButton);
        }

        private void GoToNextPage() => SkipToPage(+1);
        private void GoToPreviousPage() => SkipToPage(-1);
        private void SkipToPage(int modifier)
        {
            currentPageIndex += modifier;

            if (currentPageIndex == pages.Length)
            {
                currentPageIndex = 0;
            }
            else if (currentPageIndex == -1)
            {
                currentPageIndex = pages.Length - 1;
            }

            if (!pages[currentPageIndex].initialized)
            {
                pages[currentPageIndex].Initialize();
            }

            if (pages[currentPageIndex].initialized)
            {
                UpdatePage();
            }
            else
            {
                SkipToPage(modifier);
            }
        }

        private void UpdateTransform()
        {
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * DistanceFromCamera;
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        }

        public static void FlipSwitch()
        {
            if (!Instance.canvas.activeSelf)
            {
                Instance.UpdateTransform();
            }

            Instance.canvas.SetActive(!Instance.canvas.activeSelf);
        }
    }
}