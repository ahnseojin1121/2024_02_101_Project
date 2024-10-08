using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bicycle : Vehicle
{
    public override void Move()
    {
        base.Move();
        transform.Rotate(0, Mathf.Sin(Time.time) * 10 * Time.deltaTime, 0);
    }

    public override void Horn()
    {
        Debug.Log("자전거 경적");
    }
}
