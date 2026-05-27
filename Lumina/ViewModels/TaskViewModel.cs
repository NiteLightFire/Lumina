using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Lumina.Models;
using Lumina.Services;
using System.Threading.Tasks;
using Avalonia.Media;

namespace Lumina.ViewModels;

public partial class TaskViewModel : ViewModelBase
{
    private readonly TaskModel _taskModel;
    private readonly TaskService _taskService;
    
    [ObservableProperty]
    private string? _title;
    [ObservableProperty]
    private string? _description;
    [ObservableProperty] 
    private DateTime _dateTime;
    [ObservableProperty]
    private string _backgroundColor;
    [ObservableProperty] 
    private bool _isCompleted;
    
    public Color SelectedColor
    {
        get => Color.Parse(BackgroundColor ?? "#1E1A3D");
        set
        {
            BackgroundColor = value.ToString();
            OnPropertyChanged(nameof(SelectedColor));
            OnPropertyChanged(nameof(BackgroundColor));
        }
    }
    public DateTimeOffset? DateOffset
    {
        get => new DateTimeOffset(DateTime.SpecifyKind(DateTime, DateTimeKind.Local));
        set
        {
            if (value.HasValue)
            {
                DateTime = value.Value.DateTime.Date + DateTime.TimeOfDay;
                OnPropertyChanged(nameof(DateTime));
                OnPropertyChanged(nameof(TimeSpan));
            }
        }
    }
    public TimeSpan? TimeSpan
    {
        get => DateTime.TimeOfDay;
        set
        {
            if (value.HasValue)
            {
                DateTime = DateTime.Date + value.Value;
                OnPropertyChanged(nameof(DateTime));
                OnPropertyChanged(nameof(DateOffset)); 
            }
        }
    }

    
    public TaskModel GetModel() => _taskModel;
    
    public TaskViewModel(TaskModel task, TaskService taskService)
    {
        _taskModel = task;
        _taskService = taskService;

        Title = task.Title;
        Description = task.Description;
        DateTime = task.DateTime == default ? DateTime.Now : task.DateTime;

        BackgroundColor = string.IsNullOrEmpty(task.BackgroundColor) ? "#1E1A3D" : task.BackgroundColor;
        IsCompleted = task.IsCompleted;
    }
    

    [RelayCommand]
    public void OpenModalCommand()
    {
        WeakReferenceMessenger.Default.Send(new EditTaskMessage(this));
    }
    [RelayCommand]
    public async Task SaveChanges()
    {
        if (string.IsNullOrWhiteSpace(Title)) return;
        
        _taskModel.Title = Title;
        _taskModel.Description = Description;
        _taskModel.DateTime = DateTime;
        _taskModel.IsCompleted = IsCompleted;
        _taskModel.BackgroundColor = BackgroundColor;

        await _taskService.SaveOrUpdateTaskAsync(_taskModel);
        
        if (!_taskService.AllTasks.Any(t => t.Id == _taskModel.Id))
        {
            _taskService.AllTasks.Add(_taskModel);
        }
        else 
        {
            var index = _taskService.AllTasks.IndexOf(_taskService.AllTasks.First(t => t.Id == _taskModel.Id));
            _taskService.AllTasks[index] = _taskModel;
        }

        WeakReferenceMessenger.Default.Send(new CloseEditMessage());
    }



    [RelayCommand]
    public void CloseEditCommand()
    {
        WeakReferenceMessenger.Default.Send(new CloseEditMessage());
    }

    private void SyncFromModel()
    {
        Title = _taskModel.Title;
        Description = _taskModel.Description;
        DateTime = _taskModel.DateTime;
        IsCompleted = _taskModel.IsCompleted;
        BackgroundColor = _taskModel.BackgroundColor;
    }
    
    partial void OnIsCompletedChanged(bool value)
    {
        _taskModel.IsCompleted = value;
        Task.Run(async () => await _taskService.SaveOrUpdateTaskAsync(_taskModel));
    }
    
    [RelayCommand]
    public async Task DeleteCommand()
    {
        await _taskService.DeleteTaskAsync(_taskModel);
        WeakReferenceMessenger.Default.Send(new EditTaskMessage.TaskDeletedMessage());
    }

}