using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lumina.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace Lumina.Services;

public class TaskService
{
    private static readonly string DataFile = Path.Combine(
        AppContext.BaseDirectory, "data", "tasks.json");

    public ObservableCollection<TaskModel> AllTasks { get; } = new();

    public TaskService()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(DataFile)!);
        LoadTasks();
    }

    public void LoadTasks()
    {
        if (!File.Exists(DataFile)) return;
        try
        {
            var json = File.ReadAllText(DataFile);
            var tasks = JsonSerializer.Deserialize<List<TaskModel>>(json) ?? new();
            AllTasks.Clear();
            foreach (var t in tasks) AllTasks.Add(t);
        }
        catch { }
    }

    private void SaveTasks()
    {
        var json = JsonSerializer.Serialize(AllTasks.ToList(), new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(DataFile, json);
    }

    public Task SaveOrUpdateTaskAsync(TaskModel task)
    {
        var existing = AllTasks.FirstOrDefault(t => t.Id == task.Id);
        if (existing == null)
            AllTasks.Add(task);
        else
        {
            var idx = AllTasks.IndexOf(existing);
            AllTasks[idx] = task;
        }
        SaveTasks();
        return Task.CompletedTask;
    }

    public IEnumerable<TaskModel> GetTasksByDate(DateTime date, double stepMinutes, bool ignoreHour = false)
    {
        if (ignoreHour)
            return AllTasks.Where(t => t.DateTime.Date == date.Date);

        DateTime start = date;
        DateTime end = date.AddMinutes(stepMinutes);
        return AllTasks.Where(t => t.DateTime >= start && t.DateTime < end);
    }

    public Task DeleteTaskAsync(TaskModel task)
    {
        AllTasks.Remove(task);
        SaveTasks();
        return Task.CompletedTask;
    }
}
