using Microsoft.Geospatial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class MapTableSyncModel {
    [RealtimeProperty(1, true, true)]
    private double _latitude;

    [RealtimeProperty(2, true, true)]
    private double _longitude;

    [RealtimeProperty(3, true, true)]
    private float _zoomLevel;

    [RealtimeProperty(4, true, true)]
    private float _waterHeight;

    [RealtimeProperty(5, true, true)]
    private bool _particlesVisible;
}


