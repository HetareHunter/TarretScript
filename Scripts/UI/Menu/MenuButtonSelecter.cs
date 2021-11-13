using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuUI
{
    /// <summary>
    /// シーン切り替えメニューのボタンを選択できるかどうかを操作するクラス
    /// </summary>
    public class MenuButtonSelecter : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        [SerializeField] GameObject titleButtonObj;
        Button titleButton;
        [SerializeField] GameObject TutorialEndButtonObj;
        Button tutorialEndButton;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (titleButtonObj != null)
            {
                titleButton = titleButtonObj.GetComponent<Button>();
            }
            if (TutorialEndButtonObj != null)
            {
                tutorialEndButton = TutorialEndButtonObj.GetComponent<Button>();
            }
        }

        public void ChangeInteractiveAfterPushedButton()
        {
            canvasGroup.interactable = false;
        }

        public void AllChangeInteractive(bool activate)
        {
            canvasGroup.interactable = activate;
        }

        public void GamePlayInteractive(bool activate)
        {
            titleButton.interactable = !activate;
            tutorialEndButton.interactable = activate;
        }
        /// <summary>
        /// GameStateがIdle時のUIのinteractiveの設定
        /// </summary>
        public void IdleInteractive()
        {
            titleButton.interactable = true;
            tutorialEndButton.interactable = false;
        }
    }
}