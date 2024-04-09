using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintOtherName : MonoBehaviour
{
    public void PrintName(Collider other)
    {
        print(other.name);
    }
}
