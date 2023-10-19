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
    private GlobeManager _globeManager;

    private void Awake()
    {
        //_mapRenderer = GetComponent<MapRenderer>();
        _mapManager = GetComponent<MapManager>();
        _globeManager = GameObject.Find("Globe").GetComponent<GlobeManager>();
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
            previousModel.waterHeightDidChange -= WaterLevelDidChange;
            previousModel.particlesVisibleDidChange -= ParticlesVisibleDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                currentModel.latitude = _mapManager.mapRenderer.Center.LatitudeInDegrees;
                currentModel.longitude = _mapManager.mapRenderer.Center.LongitudeInDegrees;
                currentModel.zoomLevel = _mapManager.mapRenderer.ZoomLevel;
                currentModel.waterHeight = _mapManager.waterLevel;
                currentModel.particlesVisible = _mapManager.particles.gameObject.activeSelf;

            // Update the mesh render to match the new model
            UpdateLat();
            UpdateLong();
            UpdateZoom();
            UpdateWaterLevel();
            UpdateParticlesVisible();

            // Register for events so we'll know if the color changes later
            currentModel.latitudeDidChange += LatDidChange;
            currentModel.longitudeDidChange += LongDidChange;
            currentModel.zoomLevelDidChange += ZoomDidChange;
            currentModel.waterHeightDidChange += WaterLevelDidChange;
            currentModel.particlesVisibleDidChange += ParticlesVisibleDidChange;
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

    private void WaterLevelDidChange(MapTableSyncModel model, float waterLevel)
    {
        // Update the mesh renderer
        UpdateWaterLevel();
    }

    private void ParticlesVisibleDidChange(MapTableSyncModel model, bool particlesVisible)
    {
        // Update the mesh renderer
        UpdateParticlesVisible();
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

    private void UpdateWaterLevel()
    {
        _mapManager.pinchSlider.SliderValue = model.waterHeight;
    }

    private void UpdateParticlesVisible()
    {
        _mapManager.particles.gameObject.SetActive(model.particlesVisible);
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

    public void setWaterLevel(float waterLevel)
    {
        model.waterHeight = waterLevel;
    }

    public void setParticlesVisible(bool particlesVisible)
    {
        model.particlesVisible = particlesVisible;
    }
}
