using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Lumina.Models;
using Lumina.Services;

namespace Lumina.ViewModels;

public partial class DayMonthViewModel : ObservableObject
{
    [ObservableProperty] private bool _hasTasks;
    [ObservableProperty]
    public string _dayNumber;
    [ObservableProperty]
    public DateTime? _date;
    public DayMonthViewModel(string _dayNumber, DateTime? _date,  bool hasTasks)
    {
        DayNumber = _dayNumber;
        Date = _date;
        HasTasks = hasTasks;
    }
}
