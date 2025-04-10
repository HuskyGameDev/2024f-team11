using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePause : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    bool paused;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        canvasGroup.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            if(paused) {     //Unpause
                paused = false;
                canvasGroup.alpha = 0.0f;
                Time.timeScale = 1.0f;
            } else {
                paused = true;
                canvasGroup.alpha = 0.5f;
                Time.timeScale = 0.0f;
            }
        }
    }
}
