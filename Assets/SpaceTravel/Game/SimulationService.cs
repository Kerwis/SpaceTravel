using System;

namespace SpaceTravel.Game
{
    public static class SimulationService
    {
        private const float FullProductionHours = 8f;
        private const float FullProductionMultiplier = 1.0f;
        private const float ReducedProductionMultiplier = 0.2f;

        public static void SimulateOffline(GameState state, DateTime nowUtc, IGameDefinitions defs)
        {
            if (state == null)
                return;

            var last = TimeUtil.FromUnixSeconds(state.LastSimUnixSeconds);
            if (nowUtc <= last)
                return;

            double totalSec = (nowUtc - last).TotalSeconds;
            if (totalSec <= 0)
                return;

            double fullPhaseSec = Math.Min(totalSec, FullProductionHours * 3600.0);
            double reducedPhaseSec = Math.Max(0.0, totalSec - fullPhaseSec);

            if (fullPhaseSec > 0.0)
            {
                ProductionService.TickProduction(state, fullPhaseSec, FullProductionMultiplier, applyOverclock: false, defs);
            }

            if (reducedPhaseSec > 0.0)
            {
                ProductionService.TickProduction(state, reducedPhaseSec, ReducedProductionMultiplier, applyOverclock: false, defs);
            }

            state.LastSimUnixSeconds = TimeUtil.ToUnixSeconds(nowUtc);
        }

        public static void TickOnline(GameState state, float deltaTime, DateTime nowUtc, IGameDefinitions defs)
        {
            if (state == null)
                return;

            ProductionService.TickProduction(state, deltaTime, FullProductionMultiplier, applyOverclock: true, defs);

            state.LastSimUnixSeconds = TimeUtil.ToUnixSeconds(nowUtc);
        }
    }
}