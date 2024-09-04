using System.ComponentModel;

namespace RenderingTest.Client.Models
{
    public class ChargeViewModel : INotifyPropertyChanged
    {
        private int _cost = 0;
        private int _markUp = 0;

        public Guid Id { get; set; }

        public Guid AssociatedRouteId { get; set; }

        public int Cost
        {
            get => _cost;
            set
            {
                if (_cost != value)
                {
                    _cost = value;
                    OnPropertyChanged(nameof(Cost));
                }
            }
        }

        public int MarkUp
        {
            get => _markUp;
            set
            {
                if (_markUp != value)
                {
                    _markUp = value;
                    OnPropertyChanged(nameof(MarkUp));
                }
            }
        }

        public string Name { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
