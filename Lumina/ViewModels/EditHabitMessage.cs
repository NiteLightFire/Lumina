using System;

namespace Lumina.ViewModels;

public class EditHabitMessage
{
    public HabitViewModel HabitToEdit { get; }
    public EditHabitMessage(HabitViewModel habit) => HabitToEdit = habit;
}