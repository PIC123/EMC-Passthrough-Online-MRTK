using DilmerGames.Core.Singletons;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTClient : Singleton<ChatGPTClient>
{
    [SerializeField]
    private ChatGTPSettings chatGTPSettings;

    private List<ChatGPTChatMessage> messages;

    [TextArea(3, 10)]
    [SerializeField]
    private string startingPrompt;

    public void Start()
    {
        messages = new List<ChatGPTChatMessage>{ new ChatGPTChatMessage { Role = "system", Content = startingPrompt } };
    }

    public IEnumerator Ask(string prompt, Action<ChatGPTResponse> callBack)
    {
        var url = chatGTPSettings.debug ? $"{chatGTPSettings.apiURL}?debug=true" : chatGTPSettings.apiURL;

        messages.Add(new ChatGPTChatMessage { Role = "user", Content = prompt });

        Debug.Log($"Messages so far: {string.Join<ChatGPTChatMessage>(", ", messages)}");

        using(UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            var requestParams = JsonConvert.SerializeObject(new ChatGPTRequest
            {
                Model = chatGTPSettings.apiModel,
                Messages = messages.ToArray()
            }); ;

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestParams);
            
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            request.disposeCertificateHandlerOnDispose = true;

            request.SetRequestHeader("Content-Type", "application/json");

            // required to authenticate against OpenAI
            request.SetRequestHeader("Authorization", $"Bearer {chatGTPSettings.apiKey}");
            request.SetRequestHeader("OpenAI-Organization", chatGTPSettings.apiOrganization);

            var requestStartDateTime = DateTime.Now;

            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError || 
                request.result == UnityWebRequest.Result.DataProcessingError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string responseInfo = request.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<ChatGPTResponse>(responseInfo);

                response.ResponseTotalTime = (DateTime.Now - requestStartDateTime).TotalMilliseconds;

                callBack(response);
            }
        }
    }

    public void AskQuestion(string gptPrompt)
    {
        StartCoroutine(Ask(gptPrompt, (response) =>
        {
            Debug.Log($"ChatGPT choices: {response.Choices}");
            var msg = response.Choices.FirstOrDefault().Message;
            messages.Add(msg);
            Debug.Log($"ChatGPT response: {msg.Content}");

            ChatGPTAssistant.Instance.ChatGPTAISpeak(msg.Content);
        }));
    }
}