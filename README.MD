# Lumina

**Lumina** — современное десктопное приложение для управления задачами, привычками и повышения продуктивности. Все данные хранятся **локально** на вашем устройстве.

## О проекте

Lumina помогает структурировать день, вырабатывать полезные привычки и отслеживать прогресс с помощью удобного визуального интерфейса.

## Основные возможности

- ✅ **Задачи** — создание, редактирование, выполнение и цветовая маркировка
- 🔥 **Привычки** — отслеживание регулярных действий и streaks (серий)
- 📅 **Календарь** — Day View (по часам) и Month View
- ⚙️ **Настройки** — изменение количества колонок в дневном режиме
- 🎨 **Кастомизация** — выбор цветов для задач и привычек

### Функции в разработке

- 🧘 **Режим концентрации** — специальный фокусирующий режим (**в разработке**)
- 🎨 **Полная кастомизация UI** — темы, цвета интерфейса и др. (**в разработке**)

## Технологии

- **C#**
- **Avalonia UI** (кросс-платформенный)
- **.NET 9**
- **MVVM** + CommunityToolkit.Mvvm
- Локальное хранение данных (JSON)

Lumina/
├── Lumina.sln
├── Lumina/
│   ├── Lumina.csproj
│   ├── App.axaml
│   ├── App.axaml.cs
│   ├── Program.cs
│   ├── app.manifest
│   │
│   ├── Assets/
│   │   └── avalonia-logo.ico
│   │
│   ├── Models/
│   │   ├── TaskModel.cs
│   │   ├── HabitModel.cs
│   │   └── SettingsModel.cs
│   │
│   ├── Services/
│   │   ├── TaskService.cs
│   │   ├── HabitService.cs
│   │   └── SettingsService.cs
│   │
│   ├── ViewModels/
│   │   ├── MainWindowViewModel.cs
│   │   ├── DayViewModel.cs
│   │   ├── HourViewModel.cs
│   │   ├── MonthViewModel.cs
│   │   ├── DayMonthViewModel.cs
│   │   ├── TaskMonthViewModel.cs
│   │   ├── TaskViewModel.cs
│   │   ├── HabitViewModel.cs
│   │   ├── StreakViewModel.cs
│   │   ├── SettingsViewModel.cs
│   │   ├── ConcentrateViewModel.cs
│   │   ├── EditTaskMessage.cs
│   │   ├── EditHabitMessage.cs
│   │   └── ViewModelBase.cs
│   │
│   ├── Views/
│   │   ├── MainWindow.axaml
│   │   ├── MainWindow.cs
│   │   │
│   │   ├── Day/
│   │   │   ├── DayView.axaml
│   │   │   └── HourView.axaml
│   │   │
│   │   ├── Month/
│   │   │   ├── MonthView.axaml
│   │   │   ├── DayMonthView.axaml
│   │   │   └── TaskMonthView.axaml
│   │   │
│   │   ├── Habit/
│   │   │   ├── StreakView.axaml
│   │   │   ├── HabitView.axaml
│   │   │   └── HabitEditView.axaml
│   │   │
│   │   ├── Components/
│   │   │   ├── AddButtonView.axaml
│   │   │   ├── TaskEditView.axaml
│   │   │   └── TaskView.axaml
│   │   │
│   │   └── Pages/
│   │       ├── ConcentrateView.axaml
│   │       └── SettingsView.axaml
│   │
│   └── ViewLocator.cs
│
├── README.md
├── .gitignore
└── data/    
    Lumina.csproj
    ├── tasks.json
    ├── habits.json
    └── settings.json               # Создаётся автоматически при запуске