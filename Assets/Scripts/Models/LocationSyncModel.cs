using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class LocationSyncModel
{
    [RealtimeProperty(1, true, true)]
    private double _latitude;
    [RealtimeProperty(2, true, true)]
    private double _longitude;
}


