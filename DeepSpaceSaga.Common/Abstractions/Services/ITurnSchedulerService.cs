using DeepSpaceSaga.Common.Abstractions.Session.Entities;

namespace DeepSpaceSaga.Common.Abstractions.Services;

/// <summary>
/// Interface for managing game loop execution with configurable ticks, turns and cycles
/// </summary>
public interface ITurnSchedulerService : IDisposable
{
    /// <summary>
    /// Starts the execution loop
    /// </summary>
    /// <param name="onTickCalculation">Callback to be executed on each calculation step</param>
    /// <exception cref="ArgumentNullException">Thrown when onTickCalculation is null</exception>
    void Start(Action<ISessionInfoService, CalculationType> onTickCalculation);

    /// <summary>
    /// Stops the execution loop
    /// </summary>
    void Stop();

    /// <summary>
    /// Resumes the execution loop
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown when the executor has been disposed</exception>
    /// <exception cref="InvalidOperationException">Thrown when trying to resume without prior Start call</exception>
    void Resume();
}