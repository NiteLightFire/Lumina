using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lumina.Services;

namespace Lumina.ViewModels;


public partial class MainWindowViewModel : ObservableObject
{
    private readonly TaskService _taskService;
    private readonly SettingsService _settingsService;
    private readonly HabitService _habitService;
    
    public MainWindowViewModel(TaskService taskService, SettingsService settingsService, HabitService habitService)
    {
        _taskService = taskService;
        _settingsService = settingsService;
        _habitService = habitService;

        ShowDay();
    }
    
    [ObservableProperty] 
    private object? _currentPage;
    
    /*private DayViewModel? _dayPage;
    private MonthViewModel? _monthPage;
    private StreakViewModel? _streakPage;
    private ConcentrateViewModel? _concentratePage;
    private SettingsViewModel? _settingsPage;

    [RelayCommand]
    public void ShowDay() 
    {
        _dayPage ??= new DayViewModel(_taskService, _settingsService);
        CurrentPage = _dayPage;
    }
    
    [RelayCommand]
    public void ShowMonth() 
    {
        _monthPage ??= new MonthViewModel(_taskService);
        CurrentPage = _monthPage;
    }

    [RelayCommand]
    public void ShowStreak() 
    {
        _streakPage ??= new StreakViewModel(_habitService);
        CurrentPage = _streakPage;
    }
    
    [RelayCommand]
    public void ShowConcentrate() 
    {
        _concentratePage ??= new ConcentrateViewModel();
        CurrentPage = _concentratePage;
    }
    
    [RelayCommand]
    public void ShowSettings() 
    {
        _settingsPage ??= new SettingsViewModel();
        CurrentPage = _settingsPage;
    }
    */
    
    
    [RelayCommand]
    public void ShowDay() => CurrentPage = new DayViewModel(_taskService, _settingsService);

    [RelayCommand]
    public void ShowMonth() => CurrentPage = new MonthViewModel(_taskService);
    
    [RelayCommand]
    public void ShowStreak() => CurrentPage = new StreakViewModel(_habitService);

    [RelayCommand]
    public void ShowConcentrate() => CurrentPage = new ConcentrateViewModel();

    [RelayCommand]
    public void ShowSettings() => CurrentPage = new SettingsViewModel(_settingsService);
}
