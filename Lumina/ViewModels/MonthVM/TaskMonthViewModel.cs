using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Lumina.Models;
using Lumina.Services;

namespace Lumina.ViewModels;

public partial class TaskMonthViewModel : ViewModelBase
{
    private readonly TaskService _taskService;
    public TaskMonthViewModel(TaskService taskService)
    {
        _taskService = taskService;
        LoadTasks();
    }
    [ObservableProperty]
    private DateTime _displayTime;
    
    partial void OnDisplayTimeChanged(DateTime value)
    {
        LoadTasks();
    }

    public ObservableCollection<TaskViewModel> Tasks { get; set; } = new();

    public void LoadTasks()
    {
        if (_taskService == null) return;
        Tasks.Clear();
        var tasksFromDb = _taskService.GetTasksByDate(DisplayTime, 0, ignoreHour: true); 
        foreach (var task in tasksFromDb)
        {
            Tasks.Add(new TaskViewModel(task, _taskService)); 
        }
    }
}