using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProcessesMonitor.ViewModels;

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public Person(string firstName , string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";
    
    public ObservableCollection<Person> People { get; }
    
    public MainWindowViewModel()
    {
        var people = new List<Person> 
        {
            new Person("Neil", "Armstrong"),
            new Person("Buzz", "Lightyear"),
            new Person("James", "Kirk")
        };
        People = new ObservableCollection<Person>(people);
    }
}