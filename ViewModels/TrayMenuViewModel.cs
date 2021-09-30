using Light.Infrastructure;
using Light.ViewModels.Base;

namespace Light.ViewModels
{
    internal sealed class TrayMenuViewModel : ViewModelBase
    {

        private double _topLocation;
        private double _leftLocation;

        public double TopLocation
        {
            get => _topLocation;
            private set => Set(ref _topLocation, value);
        }

        public double LeftLocation
        {
            get => _leftLocation;
            private set => Set(ref _leftLocation, value);
        }

        public TrayMenuViewModel()
        {
            var trayNotifier = new TrayNotifier();

            trayNotifier.OnLocationChanged += (_, args) =>
            {
                LeftLocation = args.Location.X;
                TopLocation = args.Location.Y;
            };
        }
    }
}
