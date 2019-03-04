using System;
using OWM.Domain.Entities;

namespace OWM.Application.Services.EventHandlers
{
    public class MilesPledgedArgs : EventArgs
    {
        public MilesPledged MilesPledged { get; }

        public MilesPledgedArgs(MilesPledged miles)
        {
            MilesPledged = miles;
        }
    }
}