using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetEnvImg : MonoBehaviour
{
    private GlobeManager globeManager;
    public Material domeMat;
    public Material[] domeMats;
    private string imgURL;

    private Renderer m_Renderer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetTexture("https://earthmissioncontrolsa.blob.core.windows.net/360-env-images/honolulu-360-0.jpg"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Debug.Log("Sphere enabled");
        //StartCoroutine(SetTexture());
        //m_Renderer = gameObject.GetComponent<Renderer>();
    }

    public void SetMaterial(int locIdx)
    {
        GetComponent<Renderer>().material = domeMats[locIdx];
        Debug.Log("new mat set");
    }

    public IEnumerator SetTexture(string imgURL)
    {
        //imgURL = GameObject.Find("Globe").GetComponent<GlobeManager>().selectedMarker.imgURL;
        //m_Renderer = gameObject.GetComponent<Renderer>();
        Debug.Log("setting new texture");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imgURL);
        Debug.Log($"Downloading texture from {imgURL}");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            domeMat.mainTexture = myTexture;
            Debug.Log("Setting texture");
        }
    }
}
