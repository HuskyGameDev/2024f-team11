using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObject : MonoBehaviour
{
    public static event Action<GameObject> OnObjectInteraction;
    public static event Action<string> OnFalseInteraction;
    public static event Action<bool> EnteredRoom;
    public static event Action<string> EnteredRoomString;
    public static event Action<string> ExitedRoomString;
    public static event Action<bool> ExitedRoom;
    public static event Action OnHoverInteractable;
    public static event Action OnOutsideInteractable;
    Transform mainCamera;
    int LayerName;
    bool isPaused = false;
    bool inDialogue = true;
    bool isPlayable = false;

    public GameObject insideRoomTriggerObj;
    public GameObject outsideRoomTriggerObj;


    private void OnEnable()
    {
        GUIManager.OnPause += PauseFeedback;
        GUIManager.OnUnpause += ResumeFeedback;
        DialogueManager.StartingDialogueBegan += DialogueInProgress;
        DialogueManager.StartingDialogueEnded += DialogueEnded;
        BS.BSPickedUp += SetBSState;
        BS.BSDropped += SetBSState;
    }

    private void OnDisable()
    {
        GUIManager.OnPause -= PauseFeedback;
        GUIManager.OnUnpause -= ResumeFeedback;
        DialogueManager.StartingDialogueBegan -= DialogueInProgress;
        DialogueManager.StartingDialogueEnded -= DialogueEnded;
        BS.BSPickedUp -= SetBSState;
        BS.BSDropped -= SetBSState;
    }

    private void Awake()
    {
        LayerName = LayerMask.NameToLayer("Interactable");
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        if (!isPaused)
        {
            RaycastHit hit;

            // Cast a ray from the player's position to detect objects in front of them
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, 3f, LayerMask.GetMask("Interactable")))
            {
                if (hit.collider.gameObject.layer == LayerName)
                {
                    OnHoverInteractable?.Invoke();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        GameObject objHit;
                        if (hit.collider.transform.GetComponent<Wardrobe>())
                        {
                            objHit = hit.collider.gameObject;
                            OnObjectInteraction?.Invoke(objHit);
                        }

                        if (hit.collider.transform.parent)
                        {
                            if (hit.collider.transform.parent.GetComponent<Wardrobe>())
                            {
                                objHit = hit.collider.transform.parent.gameObject;
                                OnObjectInteraction?.Invoke(objHit);
                            }
                        }

                        if (hit.collider.transform.GetComponent<Drawer>() || hit.collider.transform.GetComponent<BS>())
                        {
                            objHit = hit.collider.gameObject;
                            OnObjectInteraction?.Invoke(objHit);
                        }

                        if (hit.collider.transform.GetComponent<Door>())
                        {
                            if (!inDialogue)
                            {
                                objHit = hit.collider.gameObject;
                                OnObjectInteraction?.Invoke(objHit);
                            }
                            else
                            {
                                OnFalseInteraction?.Invoke("Wait for dialogue to end.");
                            }
                        }
                    }
                }
                else
                {
                    OnOutsideInteractable?.Invoke();
                }
            }
            else
            {
                OnOutsideInteractable?.Invoke();
            }
        }
    }

    void PauseFeedback()
    {
        isPaused = true;
    }

    void ResumeFeedback()
    {
        isPaused = false;
    }

    public void DialogueInProgress() { inDialogue = true; }

    public void DialogueEnded() { inDialogue = false; }

    private void SetBSState(bool state)
    {
        isPlayable = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == insideRoomTriggerObj)
        {
            EnteredRoom?.Invoke(true);
            EnteredRoomString?.Invoke("Chase");
        }
        else if (other.gameObject == outsideRoomTriggerObj)
        {
            ExitedRoom?.Invoke(false);
            ExitedRoomString?.Invoke("Mini_Game");
        }

        if (other.gameObject == insideRoomTriggerObj && isPlayable)
        {
            EnteredRoom?.Invoke(true);
            GameManager.Instance.UpdateObjective(Objective.PLAY_BS);
        }
        else if (other.gameObject == outsideRoomTriggerObj && isPlayable)
        {
            ExitedRoom?.Invoke(false);
            GameManager.Instance.UpdateObjective(Objective.GO_TO_ROOM);
        }
        else if (other.gameObject == outsideRoomTriggerObj && !isPlayable)
        {
            ExitedRoom?.Invoke(false);
            GameManager.Instance.UpdateObjective(Objective.FIND_BS);
        }
    }
}
