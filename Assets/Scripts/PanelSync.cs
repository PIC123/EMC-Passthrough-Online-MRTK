using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Normal.Realtime;

public class PanelSync : RealtimeComponent<PanelSyncModel>
{
    [SerializeField] private HyperwallManager _hypeManager;
    private void Awake()
    {
        _hypeManager = GameObject.Find("Hyperwalls").GetComponent<HyperwallManager>();
    }

    protected override void OnRealtimeModelReplaced(PanelSyncModel previousModel, PanelSyncModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.contentIndexDidChange -= ContentIndexDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                PanelData pd = gameObject.GetComponent<PanelData>();
                if(gameObject.GetComponent<VideoPlayer>().clip == null)
                {
                    Debug.Log($"target img: {gameObject.GetComponent<Renderer>().material.mainTexture}");
                    for (int i = 0; i < _hypeManager.Images.Length - 1; i++)
                    {
                        Debug.Log($"currImg: {_hypeManager.Images[i]}");
                        if (_hypeManager.Images[i] == gameObject.GetComponent<Renderer>().material.mainTexture)
                        {
                            Debug.Log("found texture");
                            currentModel.contentIndex = i;
                            currentModel.contentType = 1;
                        }
                    }
                } else
                {
                    Debug.Log($"target vid: {gameObject.GetComponent<VideoPlayer>().clip}");
                    for (int i = 0; i < _hypeManager.Clips.Length - 1; i++)
                    {
                        Debug.Log($"curr vid: {_hypeManager.Clips[i]}");
                        if (_hypeManager.Clips[i] == gameObject.GetComponent<VideoPlayer>().clip)
                        {
                            Debug.Log("found clip");
                            currentModel.contentIndex = i;
                            currentModel.contentType = 0;
                        }
                    }
                }

                UpdateContent();
            }
                //if (_globeManager.selectedMarker.title != "")
                //{
                //    currentModel.currMarkerTitle = _globeManager.selectedMarker.title;
                //    UpdateMarkerTitle();
                //    UpdateGlobeLayer();
                //    UpdateGlobeRotation();
                //}
                //else
                //{
                //    currentModel.currMarkerTitle = "Cambridge";
                //    currentModel.currMapLayer = 0;
                //    currentModel.globeRoration = globe.transform.eulerAngles;
                //}

            // Update the mesh render to match the new model

            // Register for events so we'll know if the color changes later
            currentModel.contentIndexDidChange += ContentIndexDidChange;
        }
    }

    private void ContentIndexDidChange(PanelSyncModel model, int contentIndex)
    {
        UpdateContent();
    }

    private void UpdateContent()
    {
        switch (model.contentType)
        {
            case 0: // video
                _hypeManager.selectVid(model.contentIndex);
                Debug.Log("updating vid");
                break;
            case 1: // img
                _hypeManager.selectImg(model.contentIndex);
                Debug.Log("updating img");
                break;
            case 2: // img series
                break;
        }
    }

    public void SetContent(int contentType, int contentIndex)
    {
        model.contentType = contentType;
        model.contentIndex = contentIndex;
    }
}
