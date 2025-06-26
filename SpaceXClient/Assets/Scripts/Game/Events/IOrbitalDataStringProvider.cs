using System;

namespace Game.Events
{
    public interface IOrbitalDataStringProvider
    {
        event Action<string> OnOrbitalDataChanged;
    }
}