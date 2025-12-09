using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class ShipState
    {
        [Tooltip("Identifier of the active ship definition.")]
        public string ShipDefinitionId;

        [Tooltip("List of active module instances placed on the ship grid.")]
        public List<ModuleInstanceState> Modules = new();
    }
}