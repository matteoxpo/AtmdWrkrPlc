using System;
using System.Reactive;
using ReactiveUI;
using System.Collections.Generic;

namespace Presentation.ViewModels.WorkPlace
{
    public class WorkPlaceHelpViewModel : ReactiveObject, IRoutableViewModel
    {
        public WorkPlaceHelpViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen;
        }

        public string? UrlPathSegment { get; }
        public IScreen HostScreen { get; }

        // public RoutingState Router { get; } = new RoutingState();


    }
}
