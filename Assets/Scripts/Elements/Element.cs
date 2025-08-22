using System;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    public Action<Element> Destroyed;
}