using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Lumina.Models;
using Lumina.Services;

namespace Lumina.ViewModels;

public partial class DayViewModel : ObservableObject, IRecipient<EditTaskMessage>, IRecipient<EditTaskMessage.TaskDeletedMessage>, IRecipient<CloseEditMessage>, IModalViewModel 
{
    private readonly TaskService _taskService;
    private readonly SettingsService _settingsService;
    public DayViewModel(TaskService taskService, SettingsService settingsService)
    {
        _taskService = taskService;
        _settingsService = settingsService;
        
        WeakReferenceMessenger.Default.RegisterAll(this);
        
        GenerateHours();
    }
    
    [ObservableProperty]
    private ObservableCollection<HourViewModel> _hours = new();
    
    
    private void GenerateHours()
    {
        Hours.Clear();
        int count = _settingsService.CurrentSettings.Hours;
                
        if (count <= 0) count = 24; 

        double stepMinutes = 1440.0 / count;
                
        for (int i = 0; i < count; i++)
        {
            var hour = new HourViewModel(_taskService) 
            { 
                DisplayTime = CurrentDate.Date.AddMinutes(i * stepMinutes)
            };
            hour.LoadTasks(stepMinutes); 
            Hours.Add(hour);
        }
        OnPropertyChanged(nameof(FilteredHours));
    }
    
    [ObservableProperty]
    private bool _showOnlyWithTasks; 
    
    public System.Collections.Generic.IEnumerable<HourViewModel> FilteredHours => ShowOnlyWithTasks 
        ? Hours.Where(h => h.HasTasks) 
        : Hours;
    
    partial void OnShowOnlyWithTasksChanged(bool value) => OnPropertyChanged(nameof(FilteredHours));
    
    [ObservableProperty]
    private TaskViewModel? _selectedTask;
    
    [ObservableProperty]
    private bool _isVisible;

    public void Receive(EditTaskMessage message)
    {
        if (message?.TaskToEdit == null) return;

        var original = message.TaskToEdit.GetModel();

        var taskClone = new TaskModel 
        { 
            Id = original.Id, 
            Title = original.Title,
            Description = original.Description,
            DateTime = original.DateTime,
            BackgroundColor = original.BackgroundColor,
            IsCompleted = original.IsCompleted
        };

        SelectedTask = new TaskViewModel(taskClone, _taskService);
        IsVisible = true;   
    }

    public void Receive(CloseEditMessage message)
    {
        IsVisible = false;
        SelectedTask = null;
        
        int count = _settingsService.CurrentSettings.Hours;
        if (count <= 0) count = 24;
        double stepMinutes = 1440.0 / count;
        
        foreach (var hour in Hours)
        {
            hour.LoadTasks(stepMinutes);
        }
        OnPropertyChanged(nameof(FilteredHours));
    }


    [RelayCommand]
    public  void OpenModal()
    {
        var newTask = new TaskModel { Id = Guid.NewGuid(), DateTime = DateTime.Now }; 
        SelectedTask = new TaskViewModel(newTask, _taskService);
        IsVisible = true;
    }

    [RelayCommand]
    private void CloseEditPanel()
    {
        IsVisible = false;
        SelectedTask = null;
    }
    
    [RelayCommand]
    private void CloseModal()
    {
        IsVisible = false;
        SelectedTask = null;
    }
    
    [ObservableProperty]
    private DateTime _currentDate = DateTime.Today;

    [RelayCommand]
    private void PreviousDay()
    {
        ChangeDay(-1);
    }
    [RelayCommand]
    private void NextDay()
    {
        ChangeDay(1);
    }

    private void ChangeDay(int index)
    {
        CurrentDate = CurrentDate.AddDays(index);
        GenerateHours();
    }
    
    public void Receive(EditTaskMessage.TaskDeletedMessage message)
    {
        RefreshAllHours();
    }
    
    private void RefreshAllHours()
    {
        int count = _settingsService.CurrentSettings.Hours;
        if (count <= 0) count = 24;
        double stepMinutes = 1440.0 / count;
        
        foreach (var hour in Hours)
        {
            hour.LoadTasks(stepMinutes);
        }
        OnPropertyChanged(nameof(FilteredHours));
    }
}
