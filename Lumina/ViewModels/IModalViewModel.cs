using CommunityToolkit.Mvvm.Input;

namespace Lumina.ViewModels;

public interface IModalViewModel
{
    IRelayCommand OpenModalCommand { get; }
}