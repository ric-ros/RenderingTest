using System.ComponentModel;
using System.Threading;

namespace RenderingTest.Client.Models;

public class PersonNotifyModel : INotifyPropertyChanged
{
    private string? _firstName;
    private string? _lastName;
    private decimal _wage;

    public string? FirstName
    {
        get => _firstName;
        set
        {
            if (_firstName != value)
            {
                IntensiveOperation();
                _firstName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));
            }
        }
    }

    public string? LastName
    {
        get => _lastName;
        set
        {
            if (_lastName != value)
            {
                IntensiveOperation();
                _lastName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
            }
        }
    }

    public decimal Wage
    {
        get => _wage;
        set
        {
            if (_wage != value)
            {
                IntensiveOperation();
                _wage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Wage)));
            }
        }
    }

    private void IntensiveOperation()
    {
        for (int i = 0; i < 100; i++)
        {
            string str = new string('a', 1000);
            str = str.Replace("a", "b").ToUpper();
        }
        Thread.Sleep(50);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
public class PersonModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Wage { get; set; }
}
