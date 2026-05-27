using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging; 
using Lumina.Models;
using Lumina.Services;
using System.Globalization;

namespace Lumina.ViewModels;

public partial class MonthViewModel : ObservableObject, 
    IModalViewModel, 
    IRecipient<EditTaskMessage>, 
    IRecipient<CloseEditMessage>
{
    private readonly TaskService _taskService;
    
    [ObservableProperty] private ObservableCollection<TaskModel> _monthTasks = new();
    [ObservableProperty] private ObservableCollection<TaskModel> _electedDayTask = new();
    [ObservableProperty] private TaskMonthViewModel _selectedDayTasks;
    [ObservableProperty] private DateTime _currentMonth;
    
    [ObservableProperty] private bool _isVisible;      
    [ObservableProperty] private bool _isVisibleTask; 
    [ObservableProperty] private TaskViewModel? _selectedTask;

    public MonthViewModel(TaskService taskService)
    {
        _taskService = taskService;
        SelectedDayTasks = new TaskMonthViewModel(taskService);
        
        CurrentMonth = DateTime.Today;
        GenerateDays();
        
        WeakReferenceMessenger.Default.RegisterAll(this);
        
        IsVisible = false;
        IsVisibleTask = false;
    }

    public ObservableCollection<DayMonthViewModel> Days { get; } = new();

    IRelayCommand IModalViewModel.OpenModalCommand => OpenModalCommand;

    [RelayCommand]
    public void OpenModal() 
    {
        var newTask = new TaskModel { Id = Guid.NewGuid(), DateTime = DateTime.Now }; 
        SelectedTask = new TaskViewModel(newTask, _taskService);
        IsVisibleTask = true; 
    }

    [RelayCommand]
    public void OpenDayCommand(DayMonthViewModel day)
    {
        if (day?.Date == null) return;
        SelectedDayTasks.DisplayTime = day.Date.Value;
        SelectedDayTasks.LoadTasks();
        IsVisible = true;
    }
    
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
        IsVisibleTask = true;
    }
    
    public void Receive(CloseEditMessage message)
    {
        IsVisibleTask = false;
        SelectedTask = null;
        SelectedDayTasks.LoadTasks(); 
    }

    [RelayCommand]
    private void CloseModal() 
    {
        IsVisibleTask = false;
        SelectedTask = null;
    }

    [RelayCommand]
    public void CloseDayCommand() 
    {
        IsVisible = false;
    }
    
    [ObservableProperty] private ObservableCollection<int> _weekNumbers = new();

    private void GenerateDays()
    {
        Days.Clear();
        WeekNumbers.Clear(); 
        
        DateTime firstDayOfMonth = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        int offset = ((int)firstDayOfMonth.DayOfWeek + 6) % 7; 
        for (int i = 0; i < offset; i++)
            Days.Add(new DayMonthViewModel("", null, false));
        for (int i = 1; i <= lastDayOfMonth.Day; i++)
        {
            var date = new DateTime(CurrentMonth.Year, CurrentMonth.Month, i);
            bool hasTasks = _taskService.AllTasks.Any(t => t.DateTime.Date == date.Date);
            Days.Add(new DayMonthViewModel(i.ToString(), new DateTime(CurrentMonth.Year, CurrentMonth.Month, i), hasTasks));
        }
        
        for (int i = 0; i < Days.Count; i += 7)
        {
            var dateInWeek = Days.Skip(i).Take(7).FirstOrDefault(d => d.Date != null)?.Date 
                             ?? firstDayOfMonth;

            int weekNum = ISOWeek.GetWeekOfYear(dateInWeek);
            WeekNumbers.Add(weekNum);
        }
    }
    
    [RelayCommand]
    public void PreviousMonthCommand()
    {
        ChangeMonth(-1);
    }
    [RelayCommand]
    public void NextMonthCommand()
    {
        ChangeMonth(1);
    }

    private void ChangeMonth(int index)
    {
        CurrentMonth = CurrentMonth.AddMonths(index);
        GenerateDays();
    }

    private void LoadMonthTasks() { }
}
