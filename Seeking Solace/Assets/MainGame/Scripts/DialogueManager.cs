using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // If using TextMeshPro
using System.Collections;
using FMODUnity;

public class DialogueManager : MonoBehaviour
{
    int currentNightNum = 0;

    public static DialogueManager Instance;

    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel; 
    private Queue<string> dialogueQueue = new Queue<string>();
    public float typingSpeed = 0.05f;
    public EventReference typingSoundEvent;

    public static event Action StartingDialogueBegan;
    public static event Action StartingDialogueEnded;
    public static event Action<int> DialogueEnded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        PlayerAnimationHandler.AnimationStopped += OnNightChanged;
    }

    private void OnDestroy()
    {
        PlayerAnimationHandler.AnimationStopped -= OnNightChanged;
    }

    public void StartDialogue(string[] lines)
    {
        dialogueQueue.Clear();
        foreach (string line in lines)
        {
            dialogueQueue.Enqueue(line);
        }

        dialoguePanel.SetActive(true);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        StartCoroutine(DisplaySentencesWithDelay());
    }

    IEnumerator DisplaySentencesWithDelay()
    {
        while (dialogueQueue.Count != 0)
        {
            string sentence = dialogueQueue.Dequeue();
            yield return StartCoroutine(TypeText(sentence));
            yield return StartCoroutine(Delay()); 
        }

        EndDialogue();
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        DialogueEnded?.Invoke(currentNightNum);
        StartingDialogueEnded?.Invoke();
    }

    private void OnNightChanged(int nightNumber)
    {
        currentNightNum = nightNumber;
        switch (nightNumber)
        {
            case 1:
                StartDialogue(new string[] { "You woke up from a nightmare...", "Go find your BS and play it to unwind." });
                break;
            case 2:
                StartDialogue(new string[] { "The night grows darker...", "Something lurks in the shadows..." });
                break;
            case 3:
                StartDialogue(new string[] { "You hear whispers in the distance.", "Stay alert..." });
                break;
            case 4:
                StartDialogue(new string[] { "The air is thick with fear.", "Is this nightmare ever going to end?" });
                break;
            case 5:
                StartDialogue(new string[] { "One final test remains.", "Survive this night, and you might see the dawn." });
                break;
        }

        StartingDialogueBegan?.Invoke();
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.5f);
    }

    IEnumerator TypeText(string textToType)
    {
        dialogueText.text = "";
        foreach (char letter in textToType.ToCharArray())
        {
            dialogueText.text += letter;
            if (typingSoundEvent.Guid != Guid.Empty)
            {
                RuntimeManager.PlayOneShot(typingSoundEvent);
            }
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}