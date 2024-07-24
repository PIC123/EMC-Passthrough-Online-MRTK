using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class EnvSyncModel
{
    [RealtimeProperty(1, true, true)]
    private bool _sphereEnabled;
    [RealtimeProperty(2, true, true)]
    private bool _particlesEnabled;
}


