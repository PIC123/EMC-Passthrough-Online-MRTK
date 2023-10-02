using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Microsoft.Maps.Unity;
using Microsoft.Geospatial;

public class MapTableSync : RealtimeComponent<MapTableSyncModel>
{
    //private MapRenderer _mapRenderer;
    private MapManager _mapManager;

    private void Awake()
    {
        //_mapRenderer = GetComponent<MapRenderer>();
        _mapManager = GetComponent<MapManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnRealtimeModelReplaced(MapTableSyncModel previousModel, MapTableSyncModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.latitudeDidChange -= LatDidChange;
            previousModel.longitudeDidChange -= LongDidChange;
            previousModel.zoomLevelDidChange -= ZoomDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                currentModel.latitude = _mapManager.mapRenderer.Center.LatitudeInDegrees;
                currentModel.longitude = _mapManager.mapRenderer.Center.LongitudeInDegrees;
                currentModel.zoomLevel = _mapManager.mapRenderer.ZoomLevel;

            // Update the mesh render to match the new model
            UpdateLat();
            UpdateLong();
            UpdateZoom();

            // Register for events so we'll know if the color changes later
            previousModel.latitudeDidChange += LatDidChange;
            previousModel.longitudeDidChange += LongDidChange;
            previousModel.zoomLevelDidChange += ZoomDidChange;
        }
    }

    private void LatDidChange(MapTableSyncModel model, double latitude)
    {
        // Update the mesh renderer
        UpdateLat();
    }

    private void LongDidChange(MapTableSyncModel model, double longitude)
    {
        // Update the mesh renderer
        UpdateLat();
    }

    private void ZoomDidChange(MapTableSyncModel model, float zoom)
    {
        // Update the mesh renderer
        UpdateLat();
    }

    private void UpdateLat()
    {
        _mapManager.setLatLong(model.latitude, model.longitude);
        //_mapRenderer.Center = new LatLon(model.latitude, model.longitude);
    }

    private void UpdateLong()
    {
        _mapManager.setLatLong(model.latitude, model.longitude);
        //_mapRenderer.Center = new LatLon(model.latitude, model.longitude);
    }

    private void UpdateZoom()
    {
        _mapManager.setZoom(model.zoomLevel);
        //_mapRenderer.ZoomLevel = model.zoomLevel;
    }

    public void setLat(double latitude)
    {
        model.latitude = latitude;
    }

    public void setLong(double longitude)
    {
        model.longitude = longitude;
    }

    public void setZoom(float zoomLevel)
    {
        model.zoomLevel = zoomLevel;
    }
}
