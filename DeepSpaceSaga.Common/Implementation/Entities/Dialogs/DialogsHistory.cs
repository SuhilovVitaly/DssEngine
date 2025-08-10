namespace DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

using System.Collections.Generic;

public class DialogsHistory
{
    // Collection to store dialog keys
    private readonly HashSet<string> _dialogKeys = new();

    /// <summary>
    /// Adds a dialog key to the history
    /// </summary>
    /// <param name="key">Dialog key to add</param>
    public void Add(string key)
    {
        _dialogKeys.Add(key);
    }

    /// <summary>
    /// Checks if dialog key exists in history
    /// </summary>
    /// <param name="key">Dialog key to check</param>
    /// <returns>True if key exists, false otherwise</returns>
    public bool IsExist(string key)
    {
        return _dialogKeys.Contains(key);
    }
}
