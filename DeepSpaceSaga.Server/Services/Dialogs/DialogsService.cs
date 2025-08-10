namespace DeepSpaceSaga.Server.Services;

public class DialogsService : IDialogsService
{
    private readonly Dictionary<string, IDialog> _dialogsDictionary = [];
    private readonly DialogsHistory _dialogsHistory = new();

    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public DialogsService(string scenarioName)
    {
        LoadDialogs(scenarioName);
    }

    public void AddDialogs(IList<IDialog> dialogs)
    {
        foreach (var dialog in dialogs)
        {
            AddDialog(dialog);
        }
    }

    public void AddDialog(IDialog dialog)
    {
        if (!_dialogsDictionary.ContainsKey(dialog.Key))
        {
            _dialogsDictionary.Add(dialog.Key, dialog);
        }
    }

    public List<IDialog> GetConnectedDialogs(IDialog dialog)
    {
        var connectedDialogs = new HashSet<IDialog>();

        void FindConnectedDialogs(IDialog currentDialog)
        {
            if (currentDialog.Exits == null)
                return;

            foreach (var exit in currentDialog.Exits)
            {
                if (exit.NextKey == "-1")
                    continue;

                if (_dialogsDictionary.TryGetValue(exit.NextKey, out var nextDialog))
                {
                    if (connectedDialogs.Add(nextDialog))
                    {
                        FindConnectedDialogs(nextDialog);
                    }
                }
            }
        }

        FindConnectedDialogs(dialog);
        return connectedDialogs.ToList();
    }

    public IList<IDialog> DialogsActivation(ICommand command, ISessionContextService context)
    {
        var result = new List<IDialog>();
        switch (command.Type)
        {
            case CommandTypes.UiSelectCelestialObject:
                SelectCelestialObject(command, result, context);
                break;
            case CommandTypes.DialogInitiationByTurn:
                GetDialogsByTurn(command, result, context);
                break;
            default:
                break;
        }
        return result;
    }

    private void GetDialogsByTurn(ICommand command, List<IDialog> dialogs, ISessionContextService context)
    {
        foreach (var dialog in _dialogsDictionary.Values.Where(x =>
                         !x.ChainPart &&
                         x.Type == DialogTypes.TurnFinished))
        {
            if(!_dialogsHistory.IsExist(dialog.Key))
            {
                var turn = int.Parse(dialog.TriggerValue);

                if(turn == 0)
                {
                    continue;
                }

                if(turn > context.SessionInfo.Turn)
                {
                    continue;
                }

                _dialogsHistory.Add(dialog.Key);
                dialogs.Add(dialog);
            }            
        }
    }

    private void SelectCelestialObject(ICommand command, List<IDialog> dialogs, ISessionContextService context)
    {
        var celestialObject = context.GameSession.CelestialObjects.FirstOrDefault(x => x.Value.Id == command.TargetCelestialObjectId).Value;
        
        if (celestialObject == null)
            return;

        // Dictionary mapping object types to their activation flags and triggers
        var objectTypeMapping = new Dictionary<Type, (string Flag, DialogTrigger Trigger)>
        {
            //{ typeof(ISpaceStation), ("StationSelectActivated", DialogTrigger.SpaceStation) },
            { typeof(IAsteroid), ("AsteroidSelectActivated", DialogTrigger.Asteroid) }
        };

        // Find matching object type and process dialogs
        foreach (var (objectType, (flag, trigger)) in objectTypeMapping)
        {
            if (objectType.IsInstanceOfType(celestialObject) && !_dialogsHistory.IsExist(flag))
            {
                var matchingDialogs = _dialogsDictionary.Values.Where(x =>
                    x.Trigger == trigger &&
                    !x.ChainPart &&
                    x.Type == DialogTypes.SelectCelestialObject);

                foreach (var dialog in matchingDialogs)
                {
                    _dialogsHistory.Add(flag);
                    dialogs.Add(dialog);
                }
                break; // Object can't be of multiple types simultaneously
            }
        }
    }

    private void LoadDialogs(string scenarioName)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string dialogsPath = Path.Combine(baseDirectory, "Data", "Scenarios", scenarioName, "Dialogs");

        if (!Directory.Exists(dialogsPath))
        {
            Directory.CreateDirectory(dialogsPath);
            return;
        }

        var dialogFiles = Directory.GetFiles(dialogsPath, "*.json");

        foreach (var file in dialogFiles)
        {
            try
            {
                var jsonContent = File.ReadAllText(file);
                var dialogs = JsonSerializer.Deserialize<List<BaseDialog>>(jsonContent);

                if (dialogs != null)
                {
                    foreach (var dialog in dialogs)
                    {
                        AddDialog(dialog);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
