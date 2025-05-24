# MVP Pattern Implementation in DeepSpaceSaga

## Overview

Мы успешно внедрили MVP (Model-View-Presenter) паттерн для экрана `ScreenMainMenu` с разделением на два проекта:
- **DeepSpaceSaga.UI.Controller** - бизнес-логика и координация 
- **DeepSpaceSaga.UI** - отображение и презентация

Это обеспечивает четкое разделение ответственности и улучшенную тестируемость.

## Архитектура

### Разделение проектов:

```
DeepSpaceSaga.UI.Controller/          (Бизнес-логика)
├── Abstractions/
│   └── IMainMenuController.cs        // Интерфейс контроллера
├── Models/
│   └── GameInfoModel.cs              // Модели данных
└── MainMenuController.cs             // Реализация контроллера

DeepSpaceSaga.UI/                     (Отображение)
├── Presenters/
│   ├── IMainMenuPresenter.cs         // Интерфейс презентера
│   └── MainMenuPresenter.cs          // Реализация презентера
└── Screens/MainMenu/
    └── ScreenMainMenu.cs             // View (форма)
```

## Компоненты архитектуры

### 1. Model (GameInfoModel)
**Местоположение**: `DeepSpaceSaga.UI.Controller/Models/GameInfoModel.cs`

Содержит данные для отображения:
```csharp
public class GameInfoModel
{
    public string GameTitle { get; set; }
    public string VersionInfo { get; set; }
    public bool IsLoadGameEnabled { get; set; }
    public string? StatusMessage { get; set; }
}
```

### 2. View (ScreenMainMenu)
**Местоположение**: `DeepSpaceSaga.UI/Screens/MainMenu/ScreenMainMenu.cs`

Отвечает только за отображение:
- Привязывается к IMainMenuPresenter
- Обрабатывает события UI
- Обновляет отображение на основе событий от Presenter
- Не содержит бизнес-логику

### 3. Presenter (MainMenuPresenter)
**Местоположение**: `DeepSpaceSaga.UI/Presenters/MainMenuPresenter.cs`

Координирует взаимодействие между View и Controller:
- Обрабатывает события от View
- Вызывает методы Controller
- Уведомляет View об изменениях через события
- Обрабатывает ошибки и логирование

### 4. Controller (MainMenuController)
**Местоположение**: `DeepSpaceSaga.UI.Controller/MainMenuController.cs`

Содержит бизнес-логику:
- Взаимодействует с игровыми сервисами
- Обрабатывает игровые команды
- Управляет состоянием игры
- Не знает о UI

## Поток данных

```
User Action → View → Presenter → Controller → Game Services
           ←      ←           ←            ←
```

### Пример: Нажатие "NEW GAME"

1. **View** (`ScreenMainMenu`): Пользователь нажимает кнопку
   ```csharp
   private async void OnNewGameClick(object? sender, EventArgs e)
   {
       await _presenter.HandleNewGameAsync();
   }
   ```

2. **Presenter** (`MainMenuPresenter`): Обрабатывает команду
   ```csharp
   public async Task HandleNewGameAsync()
   {
       await _controller.StartNewGameAsync();
   }
   ```

3. **Controller** (`MainMenuController`): Выполняет бизнес-логику
   ```csharp
   public async Task StartNewGameAsync()
   {
       // Start new game session
       // Navigate to tactical map screen
   }
   ```

## Преимущества реализации

### ✅ **Разделение ответственности**
- **Controller**: Только бизнес-логика
- **Presenter**: Только координация UI
- **View**: Только отображение

### ✅ **Тестируемость**
```csharp
[Test]
public async Task HandleNewGameAsync_ShouldCallController()
{
    // Arrange
    var mockController = new Mock<IMainMenuController>();
    var presenter = new MainMenuPresenter(mockController.Object);
    
    // Act
    await presenter.HandleNewGameAsync();
    
    // Assert
    mockController.Verify(x => x.StartNewGameAsync(), Times.Once);
}
```

### ✅ **Асинхронность**
Все операции поддерживают async/await:
```csharp
private async void OnNewGameClick(object? sender, EventArgs e)
{
    UpdateButtonStates(); // Отключить кнопки
    await _presenter.HandleNewGameAsync();
    UpdateButtonStates(); // Включить кнопки
}
```

### ✅ **Обработка ошибок**
Централизованная обработка в Presenter:
```csharp
_presenter.ErrorOccurred += (sender, error) => ShowError(error);
```

### ✅ **Автоматическое обновление UI**
```csharp
_presenter.GameInfoUpdated += (sender, gameInfo) => 
{
    _currentGameInfo = gameInfo;
    UpdateButtonStates();
    Invalidate(); // Перерисовать
};
```

## Регистрация в DI

### Controllers регистрируются в UI.Controller проекте:
```csharp
services.AddScoped<IMainMenuController, MainMenuController>();
```

### Presenters регистрируются в UI проекте:
```csharp
services.AddScoped<IMainMenuPresenter, MainMenuPresenter>();
```

### Views получают Presenters через конструктор:
```csharp
public ScreenMainMenu(IMainMenuPresenter presenter)
{
    _presenter = presenter;
}
```

## Сравнение с предыдущим подходом

### ❌ **До (прямая связь)**:
```csharp
public partial class ScreenMainMenu : Form
{
    private GameManager _gameManager;
    
    private void Event_StartNewGameSession(object sender, EventArgs e)
    {
        _gameManager.SessionStart(); // Прямая связь
    }
}
```

### ✅ **После (MVP)**:
```csharp
public partial class ScreenMainMenu : Form
{
    private readonly IMainMenuPresenter _presenter;
    
    private async void OnNewGameClick(object? sender, EventArgs e)
    {
        await _presenter.HandleNewGameAsync(); // Через Presenter
    }
}
```

## Следующие шаги

Для расширения MVP на другие экраны:

1. **Создать Controller** в `DeepSpaceSaga.UI.Controller`:
   - Интерфейс в `Abstractions/`
   - Реализацию в корне проекта
   
2. **Создать Presenter** в `DeepSpaceSaga.UI`:
   - Интерфейс и реализацию в `Presenters/`
   
3. **Обновить View**:
   - Изменить конструктор для принятия Presenter
   - Заменить прямые вызовы на события Presenter
   
4. **Зарегистрировать в DI**:
   - Controller и Presenter в `StartupExtensions.AddMvpComponents()`

## Примеры миграции

### Для TacticalMap экрана:

1. Создать `ITacticalMapController` и `TacticalMapController`
2. Создать `ITacticalMapPresenter` и `TacticalMapPresenter`  
3. Обновить `ScreenTacticalMap` для использования `ITacticalMapPresenter`
4. Зарегистрировать компоненты в DI

Этот MVP подход обеспечивает чистую архитектуру с четким разделением между бизнес-логикой (Controller) и UI логикой (Presenter/View), что значительно улучшает поддерживаемость и тестируемость кода. 