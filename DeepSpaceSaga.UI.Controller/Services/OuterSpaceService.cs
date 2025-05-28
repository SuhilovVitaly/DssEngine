namespace DeepSpaceSaga.UI.Controller.Services;

public class OuterSpaceService : IOuterSpaceService
{
    public int ActiveObjectId { get; private set; }
    public int SelectedObjectId { get; private set; }

    public void EventController_OnSelectCelestialObject(ICelestialObject celestialObject)
    {
        SelectedObjectId = celestialObject.Id;
    }

    public void EventController_OnShowCelestialObject(ICelestialObject celestialObject)
    {
        ActiveObjectId = celestialObject.Id;
    }

    public void EventController_OnHideCelestialObject(ICelestialObject celestialObject)
    {
        ActiveObjectId = 0;
    }

    public void EventController_OnUnselectCelestialObject(ICelestialObject @object)
    {
        SelectedObjectId = 0;
        ActiveObjectId = 0;
    }

    public void CleanActiveObject()
    {
        ActiveObjectId = 0;
    }

    public void CleanSelectedObject()
    {
        SelectedObjectId = 0;
    }
}
