using Sparky.Templates.EventHandlers;
using System;

namespace Sparky.Services.Interfaces
{
    internal interface ICurrentTimeService
    {
        event EventHandler<TimeEventArgs> OnCurrTimeChanged;

        string GetCurrentTime();
    }
}