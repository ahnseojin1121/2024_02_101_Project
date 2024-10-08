using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//추상 클래스 : 탈것
public abstract class Vehicle : MonoBehaviour
{
    public float speed = 10f;

    //가상 함수 : 이동
    public virtual void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    //
    public abstract void Horn();
    
}
