using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class RandomColorModel
{
    [RealtimeProperty(1,true,true)]
    private Color _color;
}


