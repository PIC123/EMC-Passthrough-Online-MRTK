using DilmerGames.Core.Singletons;
using Meta.WitAi.TTS.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatGPTAssistant : Singleton<ChatGPTAssistant>
{
    [SerializeField]
    private TTSSpeaker speaker;

    [SerializeField]
    private TextMeshProUGUI captionText;

    [SerializeField]
    private int maxCharacters = 280;

    [SerializeField]
    [TextArea(5, 10)]
    private string welcomeMessage;

    [SerializeField]
    [TextArea(5, 10)]
    private string transcribingInProgressMessage;

    private bool continueSpeak;

    private int linesProcessed = 0;

    [SerializeField]
    private Animator characterAnimator;

    private bool firstTime = true;

    public void SetCharacterAssistantLoadAnimation(bool playAnim)
    {
        characterAnimator.SetBool("PlayAnim_Anim", playAnim);
    }

    private void Awake()
    {
        captionText.text = string.Empty;

        speaker.Events.OnClipDataLoadBegin.AddListener((_) =>
        {
            captionText.text = "loading...";
        });

        speaker.Events.OnStartSpeaking.AddListener((speaker,caption) =>
        {
            continueSpeak = false;
            captionText.text = caption;
        });

        speaker.Events.OnFinishedSpeaking.AddListener((speaker, caption) =>
        {
            continueSpeak = true;
            linesProcessed++;
        });

        speaker.Events.OnCancelledSpeaking.AddListener((speaker, caption) =>
        {
            continueSpeak = false;
        });
    }

    //private void Start() => speaker.Speak(welcomeMessage);

    public void ChatGPTAISpeak(string text) => StartCoroutine(SpeakInChunks(text));


    private IEnumerator SpeakInChunks(string text)
    {
        List<string> captions = SplitIntoMultipleCaptions(text);
        captions.Insert(0, transcribingInProgressMessage);

        foreach(var cap in captions)
        {
            speaker.SpeakQueued(cap);
        }
        
        yield return null;
    }

    private List<string> SplitIntoMultipleCaptions(string input)
    {
        var lines = new List<string>();
        var words = input.Split(' ');

        var currentLine = string.Empty;
        foreach (var word in words)
        {
            if ((currentLine + " " + word).Length > maxCharacters)
            {
                // Current line is too long, start a new one
                lines.Add(currentLine.Trim());
                currentLine = string.Empty;
            }
            currentLine += " " + word;
        }

        // Add any remaining words to the last line
        if (!string.IsNullOrWhiteSpace(currentLine))
        {
            lines.Add(currentLine.Trim());
        }

        return lines;
    }

    public void SpeakWelcome()
    {
        if (firstTime)
        {
            speaker.Speak(welcomeMessage);
            firstTime = false;
        }
    }
}
