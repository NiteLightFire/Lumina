using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Lumina.Models;
using Lumina.Services;

namespace Lumina.ViewModels;

public partial class HourViewModel : ObservableObject
{
    private readonly TaskService _taskService;
    private double _currentStepMinutes = 60;
    public HourViewModel(TaskService taskService)
    {
        _taskService = taskService;
        LoadTasks(60);
    }
    [ObservableProperty]
    private DateTime _displayTime;
    
    partial void OnDisplayTimeChanged(DateTime value)
    {
        LoadTasks(_currentStepMinutes);
    }

    public ObservableCollection<TaskViewModel> Tasks { get; set; } = new();
    
    [ObservableProperty]
    private bool _hasTasks;

    public void LoadTasks(double stepMinutes)
    {
        if (_taskService == null) return;
        _currentStepMinutes = stepMinutes;
        Tasks.Clear();
        var tasksFromDb = _taskService.GetTasksByDate(DisplayTime, stepMinutes); 
        foreach (var task in tasksFromDb)
        {
            Tasks.Add(new TaskViewModel(task, _taskService)); 
        }
        HasTasks = Tasks.Any();
    }
}
