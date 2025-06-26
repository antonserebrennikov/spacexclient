using System;
using UnityEngine;

namespace Game.Events
{
    public interface ISpaceSimulationTrailProvider
    {
        event Action<Vector3> OnNextPosition;
        event Action OnReset;
    }
}