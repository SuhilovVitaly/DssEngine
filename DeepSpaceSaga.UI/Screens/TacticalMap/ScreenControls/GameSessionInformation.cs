﻿using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.UI.Controller.Services;
using DeepSpaceSaga.UI.Tools;

namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class GameSessionInformation : ControlWindow
{
    private IGameManager _gameManager;
    private bool _isInitialized;

    public IGameManager GameManager
    {
        get => _gameManager;
        set
        {
            if (_gameManager != null)
            {
                _gameManager.OnUpdateGameData -= UpdateGameData;

                _gameManager.OuterSpace.OnSelectCelestialObject -= OuterSpace_OnSelectCelestialObject;
                _gameManager.OuterSpace.OnShowCelestialObject -= OuterSpace_OnShowCelestialObject;
                _gameManager.OuterSpace.OnHideCelestialObject -= OuterSpace_OnHideCelestialObject;
            }
            
            _gameManager = value;
            
            if (_gameManager != null && !DesignModeChecker.IsInDesignMode())
            {
                _gameManager.OnUpdateGameData += UpdateGameData;

                _gameManager.OuterSpace.OnSelectCelestialObject += OuterSpace_OnSelectCelestialObject;
                _gameManager.OuterSpace.OnShowCelestialObject += OuterSpace_OnShowCelestialObject;
                _gameManager.OuterSpace.OnHideCelestialObject += OuterSpace_OnHideCelestialObject;
                _isInitialized = true;
            }
        }
    }

    // Constructor for Designer.cs compatibility
    public GameSessionInformation()
    {
        InitializeComponent();
        Title = "Game Session Information";
    }

    // Constructor with dependency injection
    public GameSessionInformation(IGameManager gameManager) : this()
    {
        GameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));        
    }

    private void OuterSpace_OnHideCelestialObject(CelestialObjectSaveFormatDto obj)
    {
        // Handle hide celestial object
    }

    private void OuterSpace_OnShowCelestialObject(CelestialObjectSaveFormatDto obj)
    {
        // Handle show celestial object
    }

    private void OuterSpace_OnSelectCelestialObject(CelestialObjectSaveFormatDto obj)
    {
        // Handle select celestial object
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    private void UpdateGameData(GameSessionDto session)
    {
        if (IsDisposed) return;
        CrossThreadExtensions.PerformSafely(this, RereshControls, session);
    }

    private void RereshControls(GameSessionDto session)
    {
        if (IsDisposed) return;
        if(session.State is null) return;

        txtTurn.Text = $"{session.State.Cycle:D3}:{session.State.Turn:D3}:{session.State.Tick:D3} [{session.State.ProcessedTurns:D5}]";
        txtSpeed.Text = session.State.IsPaused ? "Pause" : session.State.Speed + "";
        txtCelestialObjects.Text = session.CelestialObjects.Count() + "";
        crlScreenCoordinates.Text = _gameManager?.ScreenInfo?.MouseCelestialCoordinates?.X + ":" + _gameManager?.ScreenInfo?.MouseCelestialCoordinates?.Y;
        crlScreenCoordinatesRelative.Text = _gameManager?.ScreenInfo?.MouseCelestialRelativeCoordinates?.X + ":" + _gameManager?.ScreenInfo?.MouseCelestialRelativeCoordinates?.Y;
        crlGameCoordinates.Text = _gameManager?.ScreenInfo?.MouseScreenCoordinates?.X + ":" + _gameManager?.ScreenInfo?.MouseScreenCoordinates?.Y;

        crlActiveId.Text = _gameManager?.OuterSpace.ActiveObjectId.ToString();
        crlSelectedId.Text = _gameManager?.OuterSpace.SelectedObjectId.ToString();
    }
}
