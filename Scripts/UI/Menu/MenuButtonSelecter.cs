using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuUI
{
    /// <summary>
    /// �V�[���؂�ւ����j���[�̃{�^����I���ł��邩�ǂ����𑀍삷��N���X
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
        /// GameState��Idle����UI��interactive�̐ݒ�
        /// </summary>
        public void IdleInteractive()
        {
            titleButton.interactable = true;
            tutorialEndButton.interactable = false;
        }
    }
}