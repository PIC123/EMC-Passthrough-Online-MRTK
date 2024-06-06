using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class EnvSync : RealtimeComponent<EnvSyncModel>
{
    [SerializeField] private EnvironmentManager envManager;

    private void Awake()
    {
        envManager = GameObject.Find("EnvironmentManager").GetComponent<EnvironmentManager>();
    }

    private void UpdateEnvMode()
    {
        envManager.showSphere = model.sphereEnabled;
    }

    private void EnvModeDidChange(EnvSyncModel model, bool sphereEnabled)
    {
        UpdateEnvMode();
    }

    public void setSphereState(bool sphereState)
    {
        model.sphereEnabled = sphereState;
    }

    protected override void OnRealtimeModelReplaced(EnvSyncModel previousModel, EnvSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.sphereEnabledDidChange -= EnvModeDidChange;
        }

        if (currentModel.isFreshModel)
        {
            currentModel.sphereEnabled = envManager.showSphere;
        }

        UpdateEnvMode();

        currentModel.sphereEnabledDidChange += EnvModeDidChange;
    }
}
