using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using MenuUI;
using Tarret;

public enum GameStateType
{
    None,
    Idle,
    Start,
    Play,
    End
}

namespace Manager
{
    public class TutorialGameStateManager : MonoBehaviour, IGameStateChangeable
    {
        GameStateType _gameStateType = GameStateType.None;
        IGameStateEnterble _gameStateEnterble;
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


        public void StopGame()
        {
            Time.timeScale = 0;
        }
        public void RebootGame()
        {
            Time.timeScale = 1;
        }

        public IGameStateEnterble GetState()
        {
            return this._gameStateEnterble;
        }

        public void ToIdle()
        {
            _gameStateType = GameStateType.Idle;
            _gameStateEnterble = _stateTypes[_gameStateType];
            _gameStateEnterble.TutorialStateEnter();
        }

        public void ToStart()
        {
            _gameStateType = GameStateType.Start;
            _gameStateEnterble = _stateTypes[_gameStateType];
            _gameStateEnterble.TutorialStateEnter();
        }

        public void ToPlay()
        {
            _gameStateType = GameStateType.Play;
            _gameStateEnterble = _stateTypes[_gameStateType];
            _gameStateEnterble.TutorialStateEnter();
        }

        public void ToEnd()
        {
            if (_gameStateType != GameStateType.Play) return;
            _gameStateType = GameStateType.End;
            _gameStateEnterble = _stateTypes[_gameStateType];
            _gameStateEnterble.TutorialStateEnter();
        }

        public void ToNone()
        {
            _gameStateType = GameStateType.None;
            _gameStateEnterble = _stateTypes[_gameStateType];
            _gameStateEnterble.TutorialStateEnter();
        }
        #region
#if UNITY_EDITOR
        void Update()
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


    public class IdleState : IGameStateEnterble
    {
        GameStartManager gameStartManager;
        MenuButtonSelecter menuButtonSelecter;

        public IdleState(GameStartManager gameStartManager, MenuButtonSelecter menuButtonSelecter)
        {
            this.gameStartManager = gameStartManager;
            this.menuButtonSelecter = menuButtonSelecter;
        }

        public void TutorialStateEnter()
        {
            gameStartManager.Reset();
            menuButtonSelecter.IdleInteractive();
        }

        public void SilhouetteStateEnter()
        {
            gameStartManager.Reset();
            menuButtonSelecter.IdleInteractive();
        }
    }

    public class StartState : IGameStateEnterble
    {
        ISpawnable spawnable;
        MenuButtonSelecter menuButtonSelecter;
        TarretAttacker tarretAttacker;

        public StartState(MenuButtonSelecter menuButtonSelecter, ISpawnable spawnable, TarretAttacker tarretAttacker)
        {
            this.spawnable = spawnable;
            this.menuButtonSelecter = menuButtonSelecter;
            this.tarretAttacker = tarretAttacker;
        }

        public void TutorialStateEnter()
        {
            ScoreManager.Instance.ResetScore();
            menuButtonSelecter.AllChangeInteractive(false);
            spawnable.SpawnStart();
            tarretAttacker.IsAttackable(false);
        }

        public void SilhouetteStateEnter()
        {
            ScoreManager.Instance.ResetScore();
            menuButtonSelecter.AllChangeInteractive(false);
            tarretAttacker.IsAttackable(false);
        }
    }

    public class PlayState : IGameStateEnterble
    {
        ISpawnable spawnable;
        GameTimer gameTimer;
        MenuButtonSelecter menuButtonSelecter;
        TarretAttacker tarretAttacker;


        public PlayState(ISpawnable spawnable, GameTimer gameTimer, MenuButtonSelecter menuButtonSelecter, TarretAttacker tarretAttacker)
        {
            this.spawnable = spawnable;
            this.gameTimer = gameTimer;
            this.menuButtonSelecter = menuButtonSelecter;
            this.tarretAttacker = tarretAttacker;
        }

        public void TutorialStateEnter()
        {
            spawnable.SpawnEnd();
            gameTimer.CountStart();
            menuButtonSelecter.AllChangeInteractive(true);
            menuButtonSelecter.GamePlayInteractive(true);
            tarretAttacker.IsAttackable(true);
        }

        public void SilhouetteStateEnter()
        {
            spawnable.SpawnStart();
            gameTimer.CountStart();
            menuButtonSelecter.AllChangeInteractive(true);
            menuButtonSelecter.GamePlayInteractive(true);
            tarretAttacker.IsAttackable(true);
        }
    }

    public class EndState : IGameStateEnterble
    {
        ISpawnable spawnable;
        GameTimer gameTimer;
        GameStartManager gameStartManager;
        MenuButtonSelecter menuButtonSelecter;

        public EndState(ISpawnable spawnable, GameTimer gameTimer, GameStartManager gameStartManager, MenuButtonSelecter menuButtonSelecter)
        {
            this.spawnable = spawnable;
            this.gameTimer = gameTimer;
            this.gameStartManager = gameStartManager;
            this.menuButtonSelecter = menuButtonSelecter;
        }

        public void TutorialStateEnter()
        {
            spawnable.Reset();
            gameTimer.CountEnd();
            gameStartManager.GameEnd();
            menuButtonSelecter.GamePlayInteractive(false);
        }

        public void SilhouetteStateEnter()
        {
            spawnable.SpawnEnd();
            gameTimer.CountEnd();
            gameStartManager.GameEnd();
            menuButtonSelecter.GamePlayInteractive(false);
        }
    }

    public class NoneState : IGameStateEnterble
    {
        public NoneState()
        {

        }

        public void TutorialStateEnter() { }
        public void SilhouetteStateEnter() { }
    }
}

public interface IGameStateChangeable
{
    public void ToIdle();
    public void ToStart();
    public void ToPlay();
    public void ToEnd();
}

public interface IGameStateEnterble
{
    public void TutorialStateEnter();
    public void SilhouetteStateEnter();
}