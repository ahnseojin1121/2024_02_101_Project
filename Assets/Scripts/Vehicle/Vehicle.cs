using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상 클래스 : 탈것 
public abstract class Vehicle : MonoBehaviour
{
    public float speed = 10f;                   //이동 속도 변수 선언

    // 가상 함수 : 이동
    public virtual void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);      //앞으로 해당 속도만큼 움직인다. 
    }

    // 추상 함수 : 경적 
    public abstract void Horn();                                            //경적 함수는 선언만 한다. 
 
}
