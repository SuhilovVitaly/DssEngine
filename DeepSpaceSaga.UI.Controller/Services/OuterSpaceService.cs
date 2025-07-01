namespace DeepSpaceSaga.UI.Controller.Services;

public class OuterSpaceService : IOuterSpaceService
{
    public event Action<CelestialObjectDto>? OnHideCelestialObject;
    public event Action<CelestialObjectDto>? OnShowCelestialObject;
    public event Action<CelestialObjectDto>? OnSelectCelestialObject;

    public int ActiveObjectId { get; private set; }
    public int SelectedObjectId { get; private set; }

    public void CleanActiveObject()
    {
        ActiveObjectId = 0;
    }

    public void CleanSelectedObject()
    {
        SelectedObjectId = 0;
    }

    public void HandleMouseMove(GameSessionDto gameSession, SpaceMapPoint coordinates)
    {
        var objectsInRange = gameSession.GetCelestialObjectsByDistance(coordinates, 20).Where(celestialObject =>
                celestialObject.Id != gameSession.GetPlayerSpaceShip().Id);

        if (objectsInRange.Count() == 0)
        {
            if(ActiveObjectId > 0)
            {
                ActiveObjectId = 0;
                OnHideCelestialObject?.Invoke(null);                
            }            

            return;
        }

        var celestialObject = objectsInRange.First();

        if (ActiveObjectId != celestialObject.Id)
        {
            ActiveObjectId = celestialObject.Id;
            OnShowCelestialObject?.Invoke(celestialObject);
        }            
    }

    public void HandleMouseClick(GameSessionDto gameSession, SpaceMapPoint coordinates)
    {
        var objectsInRange = gameSession.GetCelestialObjectsByDistance(coordinates, 20).Where(celestialObject =>
                celestialObject.Id != gameSession.GetPlayerSpaceShip().Id);

        if (objectsInRange.Count() == 0) return;

        var celestialObject = objectsInRange.First();

        SelectedObjectId = celestialObject.Id;

        OnSelectCelestialObject?.Invoke(celestialObject);
    }
}
