using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    public class GameLoopController : MonoBehaviour
    {
        [Tooltip("Definitions database instance.")]
        public DefinitionsDatabase Definitions;

        public static GameLoopController Instance { get; private set; }

        /// <summary>
        /// Fired once when GameState and Definitions are ready to use.
        /// </summary>
        public static event Action Initialized;

        public GameState State { get; private set; }

        public void ResetGame()
        {
            SaveService.DeleteSave();

            var nowUtc = DateTime.UtcNow;
            State = GameStateFactory.CreateNew(Definitions, nowUtc);
            _lastOnlineTickUtc = nowUtc;

#if UNITY_EDITOR
            Debug.Log("[GameLoopController] Game state reset.");
#endif

            Initialized?.Invoke();
        }

        private DateTime _lastOnlineTickUtc;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (Definitions == null)
            {
                Debug.LogError("[GameLoopController] DefinitionsDatabase reference is not set.");
            }
        }

        private void Start()
        {
            InitializeGameState();
        }

        private void InitializeGameState()
        {
            var nowUtc = DateTime.UtcNow;

            if (SaveService.HasSave())
            {
                State = SaveService.Load();
                if (State == null)
                {
                    Debug.LogWarning("[GameLoopController] Failed to load existing save, creating new state.");
                    State = GameStateFactory.CreateNew(Definitions, nowUtc);
                }
                else
                {
                    SimulationService.SimulateOffline(State, nowUtc, Definitions);
                }
            }
            else
            {
                State = GameStateFactory.CreateNew(Definitions, nowUtc);
            }

            _lastOnlineTickUtc = nowUtc;

            Initialized?.Invoke();
        }

        private void Update()
        {
            if (State == null || Definitions == null)
                return;

            float deltaTime = Time.deltaTime;
            var nowUtc = DateTime.UtcNow;

            SimulationService.TickOnline(State, deltaTime, nowUtc, Definitions);
            _lastOnlineTickUtc = nowUtc;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Save();
                return;
            }

            ResumeFromBackground();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        private void Save()
        {
            if (State == null)
                return;

            var nowUtc = DateTime.UtcNow;
            State.LastSimUnixSeconds = TimeUtil.ToUnixSeconds(nowUtc);
            SaveService.Save(State);
        }

        private void ResumeFromBackground()
        {
            var nowUtc = DateTime.UtcNow;
            if (State == null || Definitions == null)
                return;

            SimulationService.SimulateOffline(State, nowUtc, Definitions);
            _lastOnlineTickUtc = nowUtc;
        }
    }
}
