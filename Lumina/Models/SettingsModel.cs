using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Lumina.Models;

public partial class Settings : ObservableObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [ObservableProperty] private int _hours = 24;
}
