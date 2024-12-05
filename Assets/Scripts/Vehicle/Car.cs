using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구체적인 클래스 : 자동차
public class Car : Vehicle              //Vehicle 상속 
{
    public override void Horn()         //추상함수 실제 구현
    {
        Debug.Log("차동차 경적");
    }
}
