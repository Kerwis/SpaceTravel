using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class ResourceAmount
    {
        [Tooltip("Resource definition used by this entry (must be assigned in inspector).")]
        public ResourceDefinition Resource;

        [Tooltip("Amount of the resource.")]
        public float Amount;
    }
}