using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Key : MonoBehaviour
{
    public KeyColor keyColor = KeyColor.red;

    public KeyColor GetColor()
    {
        return keyColor;
    }
}
