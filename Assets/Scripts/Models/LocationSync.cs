using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class LocationSync : RealtimeComponent<LocationSyncModel>
{
    private MapManager mapManager;

    private void Awake()
    {
        mapManager = GetComponent<MapManager>();
    }


    private void UpdateLocation()
    {
        mapManager.setLatLong(model.latitude, model.longitude);
    }

    protected override void OnRealtimeModelReplaced(LocationSyncModel previousModel, LocationSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.latitudeDidChange -= LocationDidChange;
            previousModel.longitudeDidChange -= LocationDidChange;
        }

        if (currentModel.isFreshModel)
        {
            currentModel.latitude = mapManager.mapRenderer.Center.LatitudeInDegrees;
            currentModel.longitude = mapManager.mapRenderer.Center.LongitudeInDegrees;
        }

        UpdateLocation();

        currentModel.latitudeDidChange += LocationDidChange;
        currentModel.longitudeDidChange += LocationDidChange;
    }

    private void LocationDidChange(LocationSyncModel model, double latitude)
    {
        UpdateLocation();
    }

    //private void LatitudeDidChange(LocationSyncModel model, double latitude)
    //{
    //    UpdateLocation();
    //}

    //private void LongitudeDidChange(LocationSyncModel model, double longitude)
    //{
    //    UpdateLocation();
    //}

    public void setLatLong(double latitude, double longitude)
    {
        model.latitude = latitude;
        model.longitude = longitude;
    }

}
