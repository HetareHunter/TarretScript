using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using MenuUI;
using Tarret;

namespace Manager
{
    public class SilhouetteGameStateManager : MonoBehaviour, IGameStateChangeable
    {
        GameStateType _gameStateType = GameStateType.None;
        IGameStateEnterble _enterGameStater;
        /// <summary>gameStaterのインスタンスのキャッシュ </summary>
        Dictionary<GameStateType, IGameStateEnterble> _stateTypes = new Dictionary<GameStateType, IGameStateEnterble>();

        [Inject]
        ISpawnable _spawnable;
        [SerializeField] GameObject gameStartUI;
        [SerializeField] GameObject tarret;
        [SerializeField] GameObject SceneMovePanel;
        MenuButtonSelecter _menuButtonSelecter;
        GameStartManager _gameStartManager;
        GameTimer _gameTimer;
        TarretAttacker _tarretAttacker;

        private void Start()
        {
            _gameStartManager = gameStartUI.GetComponent<GameStartManager>();
            _gameTimer = GetComponent<GameTimer>();
            _menuButtonSelecter = SceneMovePanel.GetComponent<MenuButtonSelecter>();
            _tarretAttacker = tarret.GetComponent<TarretAttacker>();

            _stateTypes.Add(GameStateType.Idle, new IdleState(_gameStartManager, _menuButtonSelecter));
            _stateTypes.Add(GameStateType.Start, new StartState(_menuButtonSelecter, _spawnable, _tarretAttacker));
            _stateTypes.Add(GameStateType.Play, new PlayState(_spawnable, _gameTimer, _menuButtonSelecter, _tarretAttacker));
            _stateTypes.Add(GameStateType.End, new EndState(_spawnable, _gameTimer, _gameStartManager, _menuButtonSelecter));
            _stateTypes.Add(GameStateType.None, new NoneState());
            ToIdle();
        }

        public IGameStateEnterble GetState()
        {
            return this._enterGameStater;
        }

        public void ToIdle()
        {
            _gameStateType = GameStateType.Idle;
            _enterGameStater = _stateTypes[_gameStateType];
            _enterGameStater.SilhouetteStateEnter();
        }

        public void ToStart()
        {
            _gameStateType = GameStateType.Start;
            _enterGameStater = _stateTypes[_gameStateType];
            _enterGameStater.SilhouetteStateEnter();
        }

        public void ToPlay()
        {
            _gameStateType = GameStateType.Play;
            _enterGameStater = _stateTypes[_gameStateType];
            _enterGameStater.SilhouetteStateEnter();
        }

        public void ToEnd()
        {
            if (_gameStateType != GameStateType.Play) return;
            _gameStateType = GameStateType.End;
            _enterGameStater = _stateTypes[_gameStateType];
            _enterGameStater.SilhouetteStateEnter();
        }

        public void ToNone()
        {
            _gameStateType = GameStateType.None;
            _enterGameStater = _stateTypes[_gameStateType];
            _enterGameStater.SilhouetteStateEnter();
        }

        public string CurrentGameStateName()
        {
            return _enterGameStater.ToString();
        }

        public void StopGame()
        {
            Time.timeScale = 0;
        }
        public void RebootGame()
        {
            Time.timeScale = 1;
        }

        /// <summary>
        /// UIのボタンによってゲームを強制終了する場合
        /// </summary>
        public void DrawGame()
        {
            ToEnd();
        }

        #region
#if UNITY_EDITOR
        private void Update()
        {
            var keyCode = Input.inputString;
            switch (keyCode)
            {
                case "z":
                    ToIdle();
                    break;

                case "x":
                    ToStart();
                    _gameStartManager.GameStart();
                    break;

                case "c":
                    ToPlay();
                    break;

                case "v":
                    ToEnd();
                    break;
            }
        }

#endif
        #endregion
    }
}