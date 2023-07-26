#if !(UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || (UNITY_ANDROID && !UNITY_EDITOR))
#define OVRPLUGIN_UNSUPPORTED_PLATFORM
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ControllerSwitcher : MonoBehaviour
{
  public GameObject keyboardController;
  public Transform keyboardHead;
  public GameObject XRRig;
  public Transform XRHead;
  public Transform XRLeftHand;
  public Transform XRRightHand;
  public Transform realtime;

  private RealtimeAvatarManager realtimeAvatarManager;

  void Start()
  {
    realtimeAvatarManager = realtime.GetComponent<RealtimeAvatarManager>();
    realtimeAvatarManager.avatarCreated += OnAvatarCreated;

#if OVRPLUGIN_UNSUPPORTED_PLATFORM
    XRRig.SetActive(false);
    keyboardController.SetActive(true);
#else
    XRRig.SetActive(true);
#endif
  }

  void Update()
  {
  }

  public void OnAvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
  {
    if (!isLocalAvatar) return;

#if OVRPLUGIN_UNSUPPORTED_PLATFORM
    avatar.localPlayer.head = keyboardHead;
#else
    avatar.localPlayer.root = XRRig.transform;
    avatar.localPlayer.head = XRHead;
    avatar.localPlayer.leftHand = XRLeftHand;
    avatar.localPlayer.rightHand = XRRightHand;
#endif
  }
}