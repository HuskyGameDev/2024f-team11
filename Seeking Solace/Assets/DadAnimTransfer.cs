using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadAnimTransfer : MonoBehaviour
{
    void ClimbUp()
    {
        Debug.Log("Anim Transfer");
        transform.parent.gameObject.SendMessage("tpFloor2");
    }
}
