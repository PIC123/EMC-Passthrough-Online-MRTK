using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RandomColorSync : RealtimeComponent<RandomColorModel>
{
    [SerializeField] private MeshRenderer meshRenderer;
    //[SerializeField] private Color color;
    //private Color prevColor;

    //private void Update()
    //{
    //    if(color != prevColor)
    //    {
    //        model.color = color;
    //        prevColor = color;
    //    }
    //}

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void UpdateMeshRendererColor()
    {
        meshRenderer.material.color = model.color;
    }

    protected override void OnRealtimeModelReplaced(RandomColorModel previousModel, RandomColorModel currentModel)
    {
        if(previousModel != null)
        {
            previousModel.colorDidChange -= ColorDidChange;
        }

        if (currentModel.isFreshModel)
        {
            currentModel.color = meshRenderer.material.color;
        }

        UpdateMeshRendererColor();

        currentModel.colorDidChange += ColorDidChange;
    }

    private void ColorDidChange(RandomColorModel model, Color value)
    {
        UpdateMeshRendererColor();
    }

    public void setColor(Color color)
    {
        model.color = color;
    }
}
