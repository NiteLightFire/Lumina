using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lumina.Models;

namespace Lumina.Services;

public class HabitService
{
    private static readonly string DataFile = Path.Combine(
    AppContext.BaseDirectory, "data", "habits.json");

    private readonly TaskService _taskService;

    public HabitService(TaskService taskService)
    {
        _taskService = taskService;
        Directory.CreateDirectory(Path.GetDirectoryName(DataFile)!);
        LoadHabits();
    }

    public ObservableCollection<HabitModel> AllHabit { get; } = new();

    public IEnumerable<HabitModel> GetHabits() => AllHabit;

    public void LoadHabits()
    {
        if (!File.Exists(DataFile)) return;
        try
        {
            var json = File.ReadAllText(DataFile);
            var habits = JsonSerializer.Deserialize<List<HabitModel>>(json) ?? new();
            AllHabit.Clear();
            foreach (var h in habits) AllHabit.Add(h);
        }
        catch { }
    }

    private void SaveHabits()
    {
        var json = JsonSerializer.Serialize(AllHabit.ToList(), new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(DataFile, json);
    }

    public int GetStreak(Guid habitId)
    {
        var habit = AllHabit.FirstOrDefault(h => h.Id == habitId);
        if (habit == null) return 0;

        var completedDates = _taskService.AllTasks
            .Where(t => t.HabitId == habitId && t.IsCompleted && t.DateTime.Date <= DateTime.Today)
            .Select(t => t.DateTime.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();

        if (!completedDates.Any()) return 0;

        int streak = 0;
        DateTime lastCompleted = completedDates.First();
        if ((DateTime.Today - lastCompleted).Days > habit.PeriodicityDays) return 0;

        DateTime checkDate = lastCompleted;
        foreach (var date in completedDates)
        {
            if (date == checkDate) { streak++; checkDate = checkDate.AddDays(-habit.PeriodicityDays); }
            else if (date < checkDate) break;
        }
        return streak;
    }

    public async Task GenerateTasks()
    {
        foreach (var habit in AllHabit)
        {
            for (int i = 0; i < habit.HabitDaysLong; i += habit.PeriodicityDays)
            {
                var dateForTask = habit.StartDate.AddDays(i).Date;
                bool exists = _taskService.AllTasks.Any(t => t.HabitId == habit.Id && t.DateTime.Date == dateForTask);
                if (!exists)
                {
                    var task = new TaskModel
                    {
                        Title = habit.Title,
                        Description = habit.Description,
                        BackgroundColor = habit.BackgroundColor,
                        DateTime = dateForTask.Add(habit.TimeOfDay),
                        IsCompleted = false,
                        HabitId = habit.Id,
                    };
                    await _taskService.SaveOrUpdateTaskAsync(task);
                }
            }
        }
    }

    public Task SaveOrUpdateHabitAsync(HabitModel habit)
    {
        var existing = AllHabit.FirstOrDefault(h => h.Id == habit.Id);
        if (existing == null)
            AllHabit.Add(habit);
        else
        {
            var idx = AllHabit.IndexOf(existing);
            AllHabit[idx] = habit;
        }
        SaveHabits();
        return Task.CompletedTask;
    }
}
