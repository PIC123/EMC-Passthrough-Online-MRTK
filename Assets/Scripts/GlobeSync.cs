using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class GlobeSync : RealtimeComponent<GlobeSyncModel>
{
    private GlobeManager _globeManager;
    // Start is called before the first frame update
    private void Awake()
    {
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

    protected override void OnRealtimeModelReplaced(GlobeSyncModel previousModel, GlobeSyncModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.currMarkerTitleDidChange -= MarkerTitleDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                if (_globeManager.selectedMarker.title != "")
                {
                    currentModel.currMarkerTitle = _globeManager.selectedMarker.title;
                    UpdateMarkerTitle();
                }
                else
                currentModel.currMarkerTitle = "Cambridge";

            // Update the mesh render to match the new model

            // Register for events so we'll know if the color changes later
            currentModel.currMarkerTitleDidChange += MarkerTitleDidChange;
        }
    }

    private void MarkerTitleDidChange(GlobeSyncModel model, string title)
    {
        UpdateMarkerTitle();
    }

    private void UpdateMarkerTitle()
    {
        GlobeManager.Marker marker = GameObject.Find(model.currMarkerTitle).GetComponent<MapPinManager>().markerData;
        _globeManager.UpdateSelectedMarker(marker);
    }

    public void SetCurrMarkerTitle(string title)
    {
        model.currMarkerTitle = title;
    }
}
