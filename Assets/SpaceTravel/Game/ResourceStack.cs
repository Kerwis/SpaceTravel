using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class ResourceStack
    {
        [Tooltip("Resource identifier.")]
        public string Id;

        [Tooltip("Current amount of this resource.")]
        public float Amount;
    }
}