using Light.Templates.EventHandlers;
using System;

namespace Light.Services.Interfaces
{
    internal interface ICurrentTimeService
    {
        event EventHandler<TimeEventArgs> OnCurrTimeChanged;

        string GetCurrentTime();
    }
}