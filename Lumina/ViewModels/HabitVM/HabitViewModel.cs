using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Lumina.Models;
using Lumina.Services;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace Lumina.ViewModels;


public partial class HabitViewModel : ObservableObject
{
    private readonly HabitModel _habit;
    private readonly HabitService _habitService;
    

    [ObservableProperty]
    private string? _title;
    [ObservableProperty]
    private string? _description;
    [ObservableProperty] 
    private DateTime _startDate;
    [ObservableProperty]
    private string _backgroundColor;
    [ObservableProperty] 
    private TimeSpan _timeOfDay;
    [ObservableProperty]
    private int _periodicityDays;
    [ObservableProperty]
    private string? _backgroundImagePath;
    [ObservableProperty]
    private int _habitDaysLong;
    public Color SelectedColor
    {
        get => Color.Parse(BackgroundColor ?? "#1E1A3D");
        set
        {
            if (BackgroundColor != value.ToString())
            {
                BackgroundColor = value.ToString();
                BackgroundImagePath = null;
                OnPropertyChanged(nameof(SelectedColor));
            }
        }
    }
    public DateTimeOffset? DateOffset
    {
        get => new DateTimeOffset(DateTime.SpecifyKind(StartDate, DateTimeKind.Local));
        set
        {
            if (value.HasValue)
            {
                StartDate = value.Value.DateTime.Date;
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(DateOffset));
            }
        }
    }   
    
    public HabitViewModel(HabitModel habit, HabitService habitService)
    {
        _habit = habit;
        _habitService = habitService;
        
        Title = habit.Title;
        Description = habit.Description;
        StartDate = habit.StartDate;
        TimeOfDay = habit.TimeOfDay;
        PeriodicityDays = habit.PeriodicityDays;
        HabitDaysLong = habit.HabitDaysLong;
        
        BackgroundColor = habit.BackgroundColor ?? "#00BFFF"; 
        BackgroundImagePath = habit.BackgroundImagePath;

        UpdateStreak();
    }
    
    public HabitModel GetModel() => _habit;
    public Guid Id => _habit.Id; 
    
    [RelayCommand]
    public void OpenModalCommand()
    {
        WeakReferenceMessenger.Default.Send(new EditHabitMessage(this));
    }
    [RelayCommand]
    public async Task SaveChanges()
    {
        if (_habitService?.AllHabit == null) return;
        
        _habit.Title = Title ?? string.Empty;
        _habit.Description = Description ?? string.Empty;
        _habit.StartDate = StartDate; 
        _habit.BackgroundColor = BackgroundColor;
        _habit.BackgroundImagePath = BackgroundImagePath;
        _habit.TimeOfDay = TimeOfDay;
        _habit.PeriodicityDays = PeriodicityDays;
        _habit.HabitDaysLong = HabitDaysLong;
        
        await _habitService.SaveOrUpdateHabitAsync(_habit);
        
        var existingHabit = _habitService.AllHabit.FirstOrDefault(h => h.Id == _habit.Id);
        if (existingHabit != null)
        {
            int index = _habitService.AllHabit.IndexOf(existingHabit);
            _habitService.AllHabit[index] = _habit; 
        }
        else
        {
            _habitService.AllHabit.Add(_habit);
        }
        
        await _habitService.GenerateTasks(); 

        WeakReferenceMessenger.Default.Send(new CloseEditMessage());
    }



    [RelayCommand]
    public void CloseEditCommand()
    {
        WeakReferenceMessenger.Default.Send(new CloseEditMessage());
    }
    
    [ObservableProperty]
    private int _streak;

    public void UpdateStreak()
    {
        Streak = _habitService.GetStreak(Id);
    }
    

}
