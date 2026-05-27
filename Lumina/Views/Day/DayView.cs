using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using System;
using Lumina.Services;
using Lumina.ViewModels;

namespace Lumina.Views.Pages;

public partial class DayView : UserControl
{
    private Point _pPress;
    private Vector _oPress;
    private bool _isPressed;

    public DayView()
    {
        InitializeComponent();
    }

    public void ScrollViewer_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (sender is ScrollViewer scroller)
        {
            _isPressed = true;
            _pPress = e.GetPosition(this);
            _oPress = scroller.Offset;
        }
    }
    
    public void ScrollViewer_PointerMoved(object sender, PointerEventArgs e)
    {
        if (_isPressed && sender is ScrollViewer scroller)
        {
            var pCurr = e.GetPosition(this);
            var diff = _pPress - pCurr;
            double newX = _oPress.X + diff.X;
            
            scroller.Offset = new Vector(Math.Max(0, newX), scroller.Offset.Y);
        }
    }

    public void ScrollViewer_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        _isPressed = false;
    }
}