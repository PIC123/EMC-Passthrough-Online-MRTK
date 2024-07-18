using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class GlobeSyncModel
{
    [RealtimeProperty(1, true, true)]
    private string _currMarkerTitle;

    [RealtimeProperty(2, true, true)]
    private int _currMapLayer;

    [RealtimeProperty(3, true, true)]
    private Vector3 _globeRoration;
}


