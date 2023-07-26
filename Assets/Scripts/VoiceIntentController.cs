using Oculus.Voice;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public enum Displays
{
    Table = 0,
    Globe = 1,
    Hyperwall = 2
}

public class VoiceIntentController : MonoBehaviour
{
    public GameObject[] DisplayTeleportSpots;

    public GameObject player;

    public MapManager mapManager;

    [Header("Voice")]
    [SerializeField]
    private AppVoiceExperience appVoiceExperience;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI fullTranscriptText;

    [SerializeField]
    private TextMeshProUGUI partialTranscriptText;

    [SerializeField]
    private TextMeshProUGUI responseText;

    private bool appVoiceActive;

    public GameObject gazeTarget;

    public Material[] tempMats;
    public Material[] floodMats;
    public Material[] rainMats;

    private void Awake()
    {
        fullTranscriptText.text = partialTranscriptText.text = string.Empty;

        appVoiceExperience.TranscriptionEvents.OnFullTranscription.AddListener((transcript) =>
        {
            //fullTranscriptText.text = transcript;
            //ChatGPTClient.Instance.AskQuestion(transcript);
        });

        appVoiceExperience.TranscriptionEvents.OnPartialTranscription.AddListener((transcript) =>
        {
            partialTranscriptText.text = transcript;
        });

        appVoiceExperience.VoiceEvents.OnRequestCreated.AddListener((request) =>
        {
            appVoiceActive = true;
            Debug.Log("OnRequestCreated Activated");
        });

        appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(() =>
        {
            appVoiceActive = false;
            Debug.Log("OnRequestCompleted Deactivated");
        });

        appVoiceExperience.VoiceEvents.OnResponse.AddListener((response) =>
        {
            var respObj = response.AsObject;
            Debug.Log($"response: {respObj}");
            var intents = respObj["intents"];
            var respText = respObj["text"];
            Debug.Log($"intents: {intents}");
            if ((string)intents[0]["name"] == ("question")) //((intents.Count == 0 && respObj["text"].ToString() != "") || (string)intents[0]["name"] == ("question"))
            {
                Debug.Log($"Asking ChatGPT a question: {respObj["text"]}");
                ChatGPTClient.Instance.AskQuestion(respObj["text"]);
            } else
            {
                Debug.Log($"Processing Action: {respObj["text"]}");
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Keyboard.current.spaceKey.wasPressedThisFrame && !appVoiceActive)
        //{
        //    appVoiceExperience.Activate();
        //}

        //if (Input.GetButton("trigger"))
        //{
        //    Debug.Log("trigger pressed");
        //}
    }

    public void ActivateVoice()
    {
        appVoiceExperience.Activate();
    }

    private static void DisplayValues(string prefix, string[] info)
    {
        foreach (var i in info)
        {
            Debug.Log($"{prefix} {i}");
        }
    }

    public void MoveToDisplay(String[] info)
    {
        DisplayValues("MoveToDisplay: ", info);
        responseText.text = $"MoveToDisplay: {info[0]}";
        if (Enum.TryParse(info[0], true, out Displays disp))
        {
            int dispNum = (int)disp;
            GameObject desiredSpot = DisplayTeleportSpots[dispNum];
            player.transform.position = new Vector3(desiredSpot.transform.position.x, player.transform.position.y, desiredSpot.transform.position.z);
        }
    }

    public void LoadLocation(String[] info)
    {
        DisplayValues("LoadLocation: ", info);
        responseText.text = $"LoadLocation: {info[0]}";
        if (info.Length > 0 && float.TryParse(info[0], out float targetLat) && float.TryParse(info[1], out float targetLong))
        {
            mapManager.setLatLong(targetLat, targetLong);
        }
    }

    public void LoadData(String[] info)
    {
        DisplayValues("LoadData: ", info);
        responseText.text = $"LoadData: {info[0]}";
        if (gazeTarget)
        {
            if(gazeTarget.transform.parent.parent.name == "HyperWall")
            {
                switch (info[0])
                {
                    case "flood":
                        gazeTarget.GetComponent<Renderer>().material = floodMats[Random.Range(0, floodMats.Length - 1)];
                        break;
                    case "temperature":
                        gazeTarget.GetComponent<Renderer>().material = tempMats[Random.Range(0, tempMats.Length - 1)];
                        break;
                    case "rainfall":
                        gazeTarget.GetComponent<Renderer>().material = rainMats[Random.Range(0, rainMats.Length - 1)];
                        break;
                    default:
                        gazeTarget.GetComponent<Renderer>().material = floodMats[Random.Range(0, floodMats.Length - 1)];
                        break;
                }
            }
        }
    }

    public void ChangeZoom(String[] info)
    {
        DisplayValues("ChangeZoom: ", info);
        responseText.text = $"ChangeZoom: {info[0]}";
        if(info.Length > 0 && float.TryParse(info[0], out float targetZoom))
        {
            mapManager.setZoom(targetZoom);
        }
    }

    public void Help(String[] info)
    {
        DisplayValues("Help: ", info);
        responseText.text = $"Help: {info[0]}";
    }

    public void SetGazaeObject(GameObject targetObj)
    {
        gazeTarget = targetObj;
    }
}
