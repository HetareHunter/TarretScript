using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Manager
{
    public class FinishGameOperator : MonoBehaviour
    {
        TutorialGameStateManager stateManager;

        private void Start()
        {
            stateManager = gameObject.GetComponent<TutorialGameStateManager>();
        }

        public void GameEnd()
        {
            stateManager.ToEnd();
        }
    }
}