using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using System; 

namespace Lumina.Views.Components;

public partial class TaskMonthView : UserControl
{
    public TaskMonthView()
    {
        InitializeComponent();
        
    }

    private Point _pPress;
    private Vector _oPress;
    private bool _isPressed;
    
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
            double newY = _oPress.Y + diff.Y;
        
            scroller.Offset = new Vector(scroller.Offset.X, Math.Max(0, newY));
        }
    }

    public void ScrollViewer_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        _isPressed = false;
    }
}
