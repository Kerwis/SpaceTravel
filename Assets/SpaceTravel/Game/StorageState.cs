using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class StorageState
    {
        [Tooltip("List of current resource amounts per resource type.")]
        public List<ResourceStack> Resources = new();

        [Tooltip("Storage capacity for each resource group.")]
        public List<ResourceGroupCapacity> Capacities = new();
    }
}