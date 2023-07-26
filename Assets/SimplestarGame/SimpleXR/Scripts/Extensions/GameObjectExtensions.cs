using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetLayerRecursively(
        this GameObject self,
        int layer
    )
    {
        self.layer = layer;

        foreach (Transform n in self.transform)
        {
            SetLayerRecursively(n.gameObject, layer);
        }
    }

    public static void SetTagRecursively(
        this GameObject self,
        string tag
    )
    {
        self.tag = tag;

        foreach (Transform n in self.transform)
        {
            SetTagRecursively(n.gameObject, tag);
        }
    }
}