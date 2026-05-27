using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Lumina.Models;

public partial class TaskModel : ObservableObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string? _description = string.Empty;
    public DateTime DateTime { get; set; } = DateTime.Now;
    [ObservableProperty] private bool _isCompleted = false;
    public string? BackgroundColor { get; set; }
    public Guid? HabitId { get; set; }
}
