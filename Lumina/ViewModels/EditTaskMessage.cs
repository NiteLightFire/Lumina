using System;

namespace Lumina.ViewModels;

public class EditTaskMessage
{
    public TaskViewModel TaskToEdit { get; }
    public EditTaskMessage(TaskViewModel task) => TaskToEdit = task;
    
    public class TaskDeletedMessage { }
}
