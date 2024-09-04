using System.ComponentModel;

namespace RenderingTest.Client.ViewModels
{
    public class RouteViewModel : INotifyPropertyChanged
    {
        private string? _origin;
        private string? _destination;
        private string? _currency;

        public Guid Id { get; set; }

        public string? Origin
        {
            get => _origin;
            set
            {
                if (_origin != value)
                {
                    _origin = value;
                    OnPropertyChanged(nameof(Origin));
                }
            }
        }

        public string? Destination
        {
            get => _destination;
            set
            {
                if (_destination != value)
                {
                    _destination = value;
                    OnPropertyChanged(nameof(Destination));
                }
            }
        }

        public string? Currency
        {
            get => _currency;
            set
            {
                if (_currency != value)
                {
                    _currency = value;
                    OnPropertyChanged(nameof(Currency));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
