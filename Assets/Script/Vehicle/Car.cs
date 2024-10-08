using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Vehicle
{
    public override void Horn()
    {
        Debug.Log("자동차 경적");
    }
}
