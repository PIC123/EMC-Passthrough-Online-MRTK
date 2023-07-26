using UnityEngine;

namespace SimplestarGame.XR
{
	public class XRAvatarController : MonoBehaviour
	{
		[SerializeField] RuntimeAnimatorController controller = null;
		[SerializeField] Transform head;
		[SerializeField] Transform leftController;
		[SerializeField] Transform rightController;
		[SerializeField] Transform XRRig;

		internal Animator animator = null;
		internal Transform headTransform = null;

		internal void AttachGameObject(GameObject targetObject)
		{
			if (targetObject.TryGetComponent(out Animator animator))
			{
				if (!animator.gameObject.TryGetComponent(out XRAvatarController avatarController))
				{
					avatarController = animator.gameObject.AddComponent<XRAvatarController>();
				}
				avatarController.animator = animator;
				avatarController.headTransform = targetObject.transform.FindHumanoidHeadTransform();
				avatarController.controller = this.controller;
				avatarController.head = this.head;
				avatarController.leftController = this.leftController;
				avatarController.rightController = this.rightController;
				avatarController.XRRig = this.XRRig;
				animator.runtimeAnimatorController = this.controller;
			}
		}

		internal void DetachGameObject(GameObject targetObject)
		{
			if (targetObject.TryGetComponent(out XRAvatarController vRmAvatarController))
			{
				Destroy(vRmAvatarController);
			}
		}

		void OnAnimatorIK(int layerIndex)
		{
			if (!this.animator)
				return;

            this.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
			this.animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            if (null != this.leftController)
            {
                this.animator.SetIKPosition(AvatarIKGoal.LeftHand, this.leftController.position + this.leftController.TransformDirection(new Vector3(-this.handOffset.x, this.handOffset.y, this.handOffset.z) * 0.01f));
                this.animator.SetIKRotation(AvatarIKGoal.LeftHand, this.leftController.rotation * Quaternion.Euler(this.handRotOffset.x, -this.handRotOffset.y, -this.handRotOffset.z));
            }

            this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
			this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
			if (null != this.rightController)
			{
				this.animator.SetIKPosition(AvatarIKGoal.RightHand, this.rightController.position + this.rightController.TransformVector(new Vector3(this.handOffset.x, this.handOffset.y, this.handOffset.z) * 0.01f));
				this.animator.SetIKRotation(AvatarIKGoal.RightHand, this.rightController.rotation * Quaternion.Euler(this.handRotOffset.x, this.handRotOffset.y, this.handRotOffset.z));
			}

            if (null != this.head)
            {
				this.transform.position = new Vector3(this.head.position.x, Mathf.Max(this.head.position.y - 0.6f - this.head.forward.y * 0.1f, 0.3f), this.head.position.z) + this.waistOffset;
                this.transform.rotation = Quaternion.Euler(0, this.head.rotation.eulerAngles.y, 0) * Quaternion.Euler(this.waistRotOffset.x, this.waistRotOffset.y, this.waistRotOffset.z);

				this.animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
				this.animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
				this.animator.SetIKPosition(AvatarIKGoal.LeftFoot, new Vector3(this.head.position.x, XRRig.position.y, this.head.position.z) + this.transform.TransformDirection(new Vector3(-this.footOffset.x, this.footOffset.y, this.footOffset.z) * 0.1f));
				this.animator.SetIKRotation(AvatarIKGoal.LeftFoot, this.transform.rotation * Quaternion.Euler(this.footRotOffset.x, this.footRotOffset.y, -this.footRotOffset.z));

				this.animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
				this.animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
                this.animator.SetIKPosition(AvatarIKGoal.RightFoot, new Vector3(this.head.position.x, XRRig.position.y, this.head.position.z) + this.transform.TransformDirection(new Vector3(this.footOffset.x, this.footOffset.y, this.footOffset.z) * 0.1f));
                this.animator.SetIKRotation(AvatarIKGoal.RightFoot, this.transform.rotation * Quaternion.Euler(this.footRotOffset.x, -this.footRotOffset.y, this.footRotOffset.z));
            }
		}

		void LateUpdate()
		{
			if (null != this.headTransform)
			{
				this.headTransform.position = this.head.position + this.waistOffset + this.head.TransformDirection(new Vector3(this.headOffset.x, this.headOffset.y, this.headOffset.z));
				this.headTransform.rotation = this.head.rotation * Quaternion.Euler(this.headRotOffset.x, this.headRotOffset.y, -this.headRotOffset.z);
			}
		}

		Vector3 headOffset = new Vector3(0f, 0f, 0f);
		Vector3 headRotOffset = new Vector3(0f, -90f, 90f);
		Vector3 handOffset = new Vector3(-1.36f, 6.97f, -3.45f);
		Vector3 handRotOffset = new Vector3(489.9f, -133.6f, -220.2f);
		Vector3 waistOffset = new Vector3(0, 0.0f, 0f);
		Vector3 waistRotOffset = new Vector3(0f, 0f, 0);
		Vector3 footOffset = new Vector3(1.26f, 1.08f, -0.54f);
		Vector3 footRotOffset = new Vector3(0f, -14.27f, 0f);
	}
}