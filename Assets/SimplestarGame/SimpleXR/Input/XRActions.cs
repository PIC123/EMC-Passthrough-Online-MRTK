using UnityEngine;
using UnityEngine.InputSystem;

namespace SimplestarGame.XR
{
    public class XRActions : MonoBehaviour
    {
        [SerializeField] Transform XRRig;

        void Awake()
        {
            this.xrControls = new XRControls();
            this.xrControls.Enable();

            var actions = this.xrControls.XRControlsActions;

            actions.LeftControllerGrip.performed += this.OnLeftControllerGrip;
            actions.LeftControllerGrip.canceled += this.OnLeftControllerGrip;

            actions.RightControllerGrip.performed += this.OnRightControllerGrip;
            actions.RightControllerGrip.canceled += this.OnRightControllerGrip;

            actions.LeftControllerTrigger.performed += this.OnLeftControllerTrigger;
            actions.LeftControllerTrigger.canceled += this.OnLeftControllerTrigger;

            actions.RightControllerTrigger.performed += this.OnRightControllerTrigger;
            actions.RightControllerTrigger.canceled += this.OnRightControllerTrigger;

            actions.LeftControllerPosition.performed += this.OnLeftControllerPosition;
            actions.RightControllerPosition.performed += this.OnRightControllerPosition;

            actions.LeftControllerRotation.performed += this.OnLeftControllerRotation;
            actions.RightControllerRotation.performed += this.OnRightControllerRotation;
        }

        void Update()
        {
            if (null != this.leftGripObject)
            {
                if(this.leftGripObject.TryGetComponent(out GripOffset gripOffset))
                {
                    this.leftGripObject.transform.position = this.leftHandPosition + this.leftHandRotation * gripOffset.position;
                    this.leftGripObject.transform.rotation = this.leftHandRotation * Quaternion.Euler(gripOffset.rotationEuler);
                }
            }
            if (null != this.rightGripObject)
            {
                if (this.rightGripObject.TryGetComponent(out GripOffset gripOffset))
                {
                    this.rightGripObject.transform.position = this.rightHandPosition + this.rightHandRotation * gripOffset.position;
                    this.rightGripObject.transform.rotation = this.rightHandRotation * Quaternion.Euler(gripOffset.rotationEuler);
                }
            }
        }

        void OnLeftControllerPosition(InputAction.CallbackContext context)
        {
            this.leftHandPosition = this.XRRig.rotation * context.ReadValue<Vector3>() + this.XRRig.position;
        }
        void OnRightControllerPosition(InputAction.CallbackContext context)
        {
            this.rightHandPosition = this.XRRig.rotation * context.ReadValue<Vector3>() + this.XRRig.position;
        }

        void OnLeftControllerRotation(InputAction.CallbackContext context)
        {
            this.leftHandRotation = this.XRRig.rotation * context.ReadValue<Quaternion>();
        }
        void OnRightControllerRotation(InputAction.CallbackContext context)
        {
            this.rightHandRotation = this.XRRig.rotation * context.ReadValue<Quaternion>();
        }

        void OnLeftControllerGrip(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                return;
            }
            if (context.performed)
            {

                if (null != this.leftGripObject)
                {
                    if (this.leftGripObject.TryGetComponent(out GripOffset gripOffset))
                    {
                        if (gripOffset.useGravity)
                        {
                            if (this.leftGripObject.TryGetComponent(out Rigidbody rigidbody))
                            {
                                rigidbody.useGravity = true;
                            }
                        }
                    }
                    this.leftGripObject = null;
                }
                else
                {
                    var gripObjects = GameObject.FindGameObjectsWithTag(this.GRIP_OBJECT_TAG);
                    GameObject targetGrip = null;
                    float currDistance = 0;
                    foreach (var grip in gripObjects)
                    {
                        float distance = Vector3.Distance(grip.transform.position, this.leftHandPosition);
                        float gripRadius = 0.1f;
                        if (grip.TryGetComponent(out GripOffset gripOffset))
                        {
                            gripRadius = gripOffset.gripRadius;
                        }
                        if (gripRadius > distance)
                        {
                            if (null == targetGrip)
                            {
                                targetGrip = grip;
                                currDistance = distance;
                            }
                            else if (currDistance > distance)
                            {
                                targetGrip = grip;
                                currDistance = distance;
                            }
                        }
                    }
                    if (null != targetGrip)
                    {
                        this.leftGripObject = targetGrip;
                        if (this.leftGripObject.TryGetComponent(out Rigidbody rigidbody))
                        {
                            rigidbody.useGravity = false;
                        }
                    }
                }
            }
        }

        void OnRightControllerGrip(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                return;
            }
            if (context.performed)
            {
                if (null != this.rightGripObject)
                {
                    if (this.rightGripObject.TryGetComponent(out GripOffset gripOffset))
                    {
                        if (gripOffset.useGravity)
                        {
                            if (this.rightGripObject.TryGetComponent(out Rigidbody rigidbody))
                            {
                                rigidbody.useGravity = true;
                            }
                        }
                    }
                    this.rightGripObject = null;
                }
                else
                {
                    var gripObjects = GameObject.FindGameObjectsWithTag(this.GRIP_OBJECT_TAG);
                    GameObject targetGrip = null;
                    float currDistance = 0;
                    foreach (var grip in gripObjects)
                    {
                        float distance = Vector3.Distance(grip.transform.position, this.rightHandPosition);
                        float gripRadius = 0.1f;
                        if (grip.TryGetComponent(out GripOffset gripOffset))
                        {
                            gripRadius = gripOffset.gripRadius;
                        }
                        if (gripRadius > distance)
                        {
                            if (null == targetGrip)
                            {
                                targetGrip = grip;
                                currDistance = distance;
                            }
                            else if (currDistance > distance)
                            {
                                targetGrip = grip;
                                currDistance = distance;
                            }
                        }
                    }
                    if (null != targetGrip)
                    {
                        this.rightGripObject = targetGrip;
                        if (this.rightGripObject.TryGetComponent(out Rigidbody rigidbody))
                        {
                            rigidbody.useGravity = false;
                        }
                    }
                }
            }
        }

        void OnLeftControllerTrigger(InputAction.CallbackContext context)
        {
            if (null != this.leftGripObject)
            {
                if (this.leftGripObject.TryGetComponent(out ITrigger trigger))
                {
                    trigger.OnTrigger(context.performed, context.ReadValue<float>());
                }
            }
        }

        void OnRightControllerTrigger(InputAction.CallbackContext context)
        {
            if (null != this.rightGripObject)
            {
                if (this.rightGripObject.TryGetComponent(out ITrigger trigger))
                {
                    trigger.OnTrigger(context.performed, context.ReadValue<float>());
                }
            }
        }

        readonly string GRIP_OBJECT_TAG = "Grip";
        GameObject leftGripObject;
        GameObject rightGripObject;
        Vector3 leftHandPosition;
        Quaternion leftHandRotation;
        Vector3 rightHandPosition;
        Quaternion rightHandRotation;
        XRControls xrControls;
    }
}