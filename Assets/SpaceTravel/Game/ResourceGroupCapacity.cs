using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class ResourceGroupCapacity
    {
        [Tooltip("Resource group this capacity applies to.")]
        public ResourceGroup Group;

        [Tooltip("Maximum total amount for all resources in this group.")]
        public float Capacity;
    }
}