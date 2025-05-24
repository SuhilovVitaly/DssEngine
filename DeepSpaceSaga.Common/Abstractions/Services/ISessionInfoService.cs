namespace DeepSpaceSaga.Common.Abstractions.Session.Entities;

/// <summary>
/// Represents session information interface for managing game session state and counters
/// </summary>
public interface ISessionInfoService
{
    Guid Id { get; }
    int Turn { get; set; }
    int TickTotal { get; }
    int TickCounter { get; }
    int TurnCounter { get; }
    int CycleCounter { get; }
    bool IsCalculationInProgress { get; set; }
    bool IsPaused { get; set; }
    int Speed { get; set; }
    void SetSpeed(int speed);
    int IncrementTurn();
    int IncrementTickTotal();
    int IncrementTickCounter();
    void ResetTickCounter();
    int IncrementTurnCounter();
    void ResetTurnCounter();
    int IncrementCycleCounter();
    string ToString();

    void Reset();
} 