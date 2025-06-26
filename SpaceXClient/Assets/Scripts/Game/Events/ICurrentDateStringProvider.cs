using System;

namespace Game.Events
{
    public interface ICurrentDateStringProvider
    {
        event Action<string> OnCurrentDateStringChanged;
    }
}