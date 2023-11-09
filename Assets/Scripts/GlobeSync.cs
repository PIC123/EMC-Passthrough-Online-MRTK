using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class GlobeSync : RealtimeComponent<GlobeSyncModel>
{
    private GlobeManager _globeManager;
    private GameObject globe;
    // Start is called before the first frame update
    private void Awake()
    {
        _globeManager = GameObject.Find("Globe").GetComponent<GlobeManager>();
        globe = GameObject.Find("Globe Module");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnRealtimeModelReplaced(GlobeSyncModel previousModel, GlobeSyncModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.currMarkerTitleDidChange -= MarkerTitleDidChange;
            previousModel.currMapLayerDidChange -= GlobeLayerDidChange;
            previousModel.globeRorationDidChange -= GlobeRotationDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                if (_globeManager.selectedMarker.title != "")
                {
                    currentModel.currMarkerTitle = _globeManager.selectedMarker.title;
                    UpdateMarkerTitle();
                    UpdateGlobeLayer();
                    UpdateGlobeRotation();
                }
                else
                {
                    currentModel.currMarkerTitle = "Cambridge";
                    currentModel.currMapLayer = 0;
                    currentModel.globeRoration = globe.transform.eulerAngles;
                }

            // Update the mesh render to match the new model

            // Register for events so we'll know if the color changes later
            currentModel.currMarkerTitleDidChange += MarkerTitleDidChange;
            currentModel.currMapLayerDidChange += GlobeLayerDidChange;
            currentModel.globeRorationDidChange += GlobeRotationDidChange;
        }
    }

    private void MarkerTitleDidChange(GlobeSyncModel model, string title)
    {
        UpdateMarkerTitle();
    }

    private void GlobeLayerDidChange(GlobeSyncModel model, int layer)
    {
        UpdateGlobeLayer();
    }

    private void GlobeRotationDidChange(GlobeSyncModel model, Vector3 rotation)
    {
        UpdateGlobeRotation();
    }

    private void UpdateMarkerTitle()
    {
        GlobeManager.Marker marker = GameObject.Find(model.currMarkerTitle).GetComponent<MapPinManager>().markerData;
        Debug.Log("marker found");
        _globeManager.UpdateSelectedMarker(marker);
    }

    private void UpdateGlobeLayer()
    {
        _globeManager.setGlobeMap(model.currMapLayer);
    }

    private void UpdateGlobeRotation()
    {
        globe.transform.eulerAngles = model.globeRoration;
        Debug.Log("Rotated");
    }

    public void SetCurrMarkerTitle(string title)
    {
        model.currMarkerTitle = title;
    }

    public void SetGlobeLayer(int layer)
    {
        model.currMapLayer = layer;
    }

    public void SetGlobeRotation(Vector3 rotation)
    {
        model.globeRoration = rotation;
    }
}
