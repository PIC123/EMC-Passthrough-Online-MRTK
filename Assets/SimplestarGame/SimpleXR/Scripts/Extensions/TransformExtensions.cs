using UnityEngine;

public static class TransformExtensions
{
    public static Transform FindRecursively(
        this Transform self,
        string name
    )
    {
        var child = self.Find(name);
        if (null == child)
        {
            foreach (Transform c in self.transform)
            {
                child = FindRecursively(c, name);
                if (null != child)
                {
                    break;
                }
            }
        }
        return child;
    }

    public static Transform FindHumanoidHeadTransform(
        this Transform self )
    {
        Transform humanHeadTransform = null;
        if (self.TryGetComponent(out Animator animator))
        {
            if (animator.avatar && animator.avatar.isHuman)
            {
                var humanBones = animator.avatar.humanDescription.human;
                foreach (var humanBone in humanBones)
                {
                    if ("Head" == humanBone.humanName)
                    {
                        humanHeadTransform = self.FindRecursively(humanBone.boneName);
                        break;
                    }

                }
            }
        }
        return humanHeadTransform;
    }
}
