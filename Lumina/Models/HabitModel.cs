using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Lumina.Models;

public partial class HabitModel : ObservableObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private DateTime _startDate = DateTime.Now;
    [ObservableProperty] private TimeSpan _timeOfDay;
    [ObservableProperty] private int _periodicityDays;
    [ObservableProperty] private int _habitDaysLong;
    [ObservableProperty] private string? _backgroundColor;
    [ObservableProperty] private string? _backgroundImagePath;
}
