using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class PanelSyncModel
{
    [RealtimeProperty(1, true, true)] // Panel content type (0 = video, 1 = image, 2 = image series)
    private int _contentType;
    [RealtimeProperty(2, true, true)]
    private int _contentIndex;
}


