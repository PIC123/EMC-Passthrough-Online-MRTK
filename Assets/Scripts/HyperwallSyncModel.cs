using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class HyperwallSyncModel
{
    [RealtimeProperty(1, true, true)]
    private List<GameObject> _allPanelList;
    [RealtimeProperty(2, true, true)]
    private int _selectedHyperwall = -1;
    [RealtimeProperty(3, true, true)]
    private int _selectedPanel = -1;
}
