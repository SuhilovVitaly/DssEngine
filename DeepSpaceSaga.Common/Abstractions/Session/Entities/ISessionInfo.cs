namespace DeepSpaceSaga.Common.Abstractions.Session.Entities;

/// <summary>
/// Represents session information interface for managing game session state and counters
/// </summary>
public interface ISessionInfo
{
    Guid Id { get; }
    int Turn { get; set; }
    int TickTotal { get; }
    int TickCounter { get; }
    int TurnCounter { get; }
    int CycleCounter { get; }
    bool IsPaused { get; set; }
    SessionState State { get; set; }
    
    int IncrementTurn();
    int IncrementTickTotal();
    int IncrementTickCounter();
    void ResetTickCounter();
    int IncrementTurnCounter();
    void ResetTurnCounter();
    int IncrementCycleCounter();
    string ToString();
} 