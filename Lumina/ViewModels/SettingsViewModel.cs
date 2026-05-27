using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lumina.Models;
using Lumina.Services;

namespace Lumina.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly SettingsService _settingsService;
    
    public SettingsViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;
        _hour = _settingsService.CurrentSettings.Hours;
    }

    [ObservableProperty] 
    private int _hour;
    public int[] AllowedValues { get; } = { 1, 2, 3, 4, 6, 8, 12, 24, 48, 96 };

    partial void OnHourChanged(int value)
    {
        Task.Run(async () => await _settingsService.ChangeHour(value));
    }
    
    [RelayCommand]
    public void SetHour(int hour)
    {
        Hour = hour; 
    }
}