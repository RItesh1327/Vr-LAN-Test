using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR;
using TMPro;

public class BootManager : MonoBehaviour
{
    [SerializeField] bool m_forceXrInEditor = false;
    [SerializeField] bool m_ForceXR;
    [SerializeField] GameObject m_FirstPerson;
    [SerializeField] GameObject m_XrOrigin;
    [SerializeField] TextMeshPro m_WorldSpaceText;
    [SerializeField] bool GiveUpControl = false;

    private void Awake()
    {
        if (GiveUpControl)
            return;

        bool isVRActive = XRSettings.isDeviceActive || m_ForceXR;

        m_XrOrigin.SetActive(isVRActive);
        m_FirstPerson.SetActive(!isVRActive);
    }

    public void SetAvatar(bool isVR)
    {
        if (!GiveUpControl)
            return;

        Debug.Log($"Setting avatar: {(isVR ? "VR" : "FPS")}");
        m_XrOrigin.SetActive(isVR);
        m_FirstPerson.SetActive(!isVR);
    }
}
