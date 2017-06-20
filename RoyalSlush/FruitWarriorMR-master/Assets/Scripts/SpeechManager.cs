using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	public BlendButton blenderButton; // Blender button start blender
	public BlenderTopButton TopButton;

    // Use this for initialization
    void Start()
    {
        keywords.Add("Start Game", () =>
        {
            // Call the OnStart method on every descendant object.
            this.BroadcastMessage("OnStart");
        });

		/*
		 * FOOD WORDS
		 */

        keywords.Add("Watermelon", () =>
        {
            // Call the OnWatermelon method on every descendant object.
            this.BroadcastMessage("OnWatermelon");
        });

        keywords.Add("Grapes", () =>
        {
            // Call the OnGrapes method on every descendant object.
            this.BroadcastMessage("OnGrapes");
        });

		keywords.Add("Strawberry", () =>
		{
			// Call the OnGrapes method on every descendant object.
			this.BroadcastMessage("OnGrapes");
		});


		/*
		 * CONTROL WORDS
		 */
		keywords.Add("Blend", () =>
		{
			StartBlender();
		});

		keywords.Add("Start", () =>
		{
			StartBlender();
		});

		keywords.Add("Stop", () =>
		{
			StartBlender();
		});

		keywords.Add("Open", () =>
		{
				OpenBlenderTop();
		});

		// Open blender?
		// STOP
		// Blend
		// START


        keywords.Add("Restart Game", () =>
        {
            // Call the OnReset method on every descendant object.
            this.BroadcastMessage("OnRestart");
        });
        keywords.Add("Main Menu", () =>
        {
            // Call the OnMenu method on every descendant object.
            this.BroadcastMessage("OnMenu");
        });

        keywords.Add("Show Grid", () =>
        {
            // Call the ShowGrid method on every descendant object.
            this.BroadcastMessage("ShowGrid");
        });
        keywords.Add("Hide Grid", () =>
        {
            // Call the HideGrid method on every descendant object.
            this.BroadcastMessage("HideGrid");
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

	void OpenBlenderTop(){

		TopButton.BroadcastMessage("OnBlend");
	}

	void StartBlender(){
		// Call the OnGrapes method on every descendant object.
		blenderButton.BroadcastMessage("OnBlend");
	}

	void StopBlender(){
		// Call the OnGrapes method on every descendant object.
		blenderButton.BroadcastMessage("OnBlend");
	}

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}