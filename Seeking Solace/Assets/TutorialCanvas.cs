using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject player;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject sign;
    [SerializeField] private float range;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(player.transform.position.x - sign.transform.position.x) < range && Mathf.Abs(player.transform.position.z - sign.transform.position.z) < range) {
            canvasGroup.alpha = 1f;
        }
        else {
            canvasGroup.alpha = 0f;
        }
    }
}