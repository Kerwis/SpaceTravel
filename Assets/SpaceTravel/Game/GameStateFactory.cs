using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceTravel.Game
{
    public static class GameStateFactory
    {
        private const string StarterShipId = "starter_ship";

        public static GameState CreateNew(IGameDefinitions defs, DateTime nowUtc)
        {
            var state = new GameState
            {
                LastSimUnixSeconds = TimeUtil.ToUnixSeconds(nowUtc),
                Ship = CreateStarterShipState(defs),
                Storage = CreateStarterStorageState(),
                Drone = CreateStarterDroneState(),
                Crew = CreateStarterCrewState(),
                Progress = CreateStarterProgress()
            };

            return state;
        }

        private static ShipState CreateStarterShipState(IGameDefinitions defs)
        {
            var shipDef = defs.GetShipDefinition(StarterShipId);
            if (shipDef == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"[GameStateFactory] ShipDefinition with Id '{StarterShipId}' not found.");
#endif
                return new ShipState
                {
                    ShipDefinitionId = StarterShipId,
                    Modules = new List<ModuleInstanceState>()
                };
            }

            var shipState = new ShipState
            {
                ShipDefinitionId = shipDef.Id,
                Modules = new List<ModuleInstanceState>()
            };

            return shipState;
        }

        private static StorageState CreateStarterStorageState()
        {
            var storage = new StorageState
            {
                Resources = new List<ResourceStack>(),
                Capacities = new List<ResourceGroupCapacity>()
            };

            storage.SetCapacity(ResourceGroup.Raw, 100f);
            storage.SetCapacity(ResourceGroup.Refined, 100f);
            storage.SetCapacity(ResourceGroup.Currency, 100000f);
            
            storage.SetAmount("scrap", 10);

            return storage;
        }

        private static DroneState CreateStarterDroneState()
        {
            return new DroneState
            {
                Unlocked = false,
                Health = 1.0f,
                Level = 1
            };
        }

        private static CrewState CreateStarterCrewState()
        {
            return new CrewState
            {
                Health = 1.0f,
                RadiationResistance = 1.0f
            };
        }

        private static PlayerProgressionState CreateStarterProgress()
        {
            return new PlayerProgressionState
            {
                Level = 1,
                TotalGoldEarned = 0,
                PvpBattlesPlayed = 0
            };
        }
    }
}
