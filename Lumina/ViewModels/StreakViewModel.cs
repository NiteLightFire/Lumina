using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Lumina.Services;
using Lumina.Models;

namespace Lumina.ViewModels;

public partial class StreakViewModel : ObservableObject, IRecipient<EditHabitMessage>, IRecipient<CloseEditMessage>, IModalViewModel
{
    
    private readonly HabitService _habitService;
    public ObservableCollection<HabitViewModel> Habits { get; } = new();

    public StreakViewModel(HabitService habitService)
    {
        _habitService = habitService;
        
        WeakReferenceMessenger.Default.RegisterAll(this);
        
        LoadHabits();
    }

    private void LoadHabits()
    {
        if (_habitService == null) return;
        Habits.Clear();
        var habitsFromDb = _habitService.GetHabits(); 
        foreach (var habit in habitsFromDb)
        {
            Habits.Add(new HabitViewModel(habit, _habitService)); 
        }
    }
    
    [ObservableProperty]
    private HabitViewModel? _selectedHabit;
    
    [ObservableProperty]
    private bool _isVisible;
    
    public void Receive(EditHabitMessage message)
    {
        if (message?.HabitToEdit == null) return;
        
        var original = message.HabitToEdit.GetModel();

        var habitClone = new HabitModel 
        { 
            Id = original.Id, 
            Title = original.Title,
            Description = original.Description,
            StartDate = original.StartDate,
            TimeOfDay = original.TimeOfDay,
            PeriodicityDays = original.PeriodicityDays,
            BackgroundColor = original.BackgroundColor,
            HabitDaysLong = original.HabitDaysLong 
        };

        SelectedHabit = new HabitViewModel(habitClone, _habitService);
        IsVisible = true;   
    }

    public void Receive(CloseEditMessage message)
    {
        IsVisible = false;
        SelectedHabit = null;
        LoadHabits(); 
    }

    [RelayCommand]
    public void OpenModal()
    {
        var newHabit = new HabitModel { Id = Guid.NewGuid(), StartDate = DateTime.Now }; 
        SelectedHabit = new HabitViewModel(newHabit, _habitService);
        IsVisible = true;
    }

    [RelayCommand]
    private void CloseModal()
    {
        IsVisible = false;
        SelectedHabit = null;
    }
}
