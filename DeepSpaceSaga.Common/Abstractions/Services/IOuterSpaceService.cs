﻿using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IOuterSpaceService
{
    event Action<CelestialObjectSaveFormatDto>? OnHideCelestialObject;
    event Action<CelestialObjectSaveFormatDto>? OnShowCelestialObject;
    event Action<CelestialObjectSaveFormatDto>? OnSelectCelestialObject;

    int ActiveObjectId { get; }
    int SelectedObjectId { get; }

    void CleanActiveObject();
    void CleanSelectedObject();

    /// <summary>
    /// Handles mouse movement on tactical map
    /// </summary>
    /// <param name="coordinatesGame">Game coordinates</param>
    /// <param name="coordinatesScreen">Screen coordinates</param>
    void HandleMouseMove(GameSessionDto gameSession, SpaceMapPoint celestialCoordinates, SpaceMapPoint screenCoordinates);

    /// <summary>
    /// Handles mouse click on tactical map
    /// </summary>
    /// <param name="coordinates">Click coordinates</param>
    void HandleMouseClick(GameSessionDto gameSession, SpaceMapPoint coordinates);
}
