using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어의 상태를 관리
public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState;                 //현재 플레이어의 상태를 나타내는 변수
    public PlayerController PlayerController;        //

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //
        TransitionToState(new IdleState(this));

    }

    // Update is called once per frame
    void Update()
    {
        //
        if(currentState != null)
        {
            currentState.Update();
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdate();
        }
    }

    public void TransitionToState(PlayerState newState)
    {
        //현재 상태와 새로운 상태가 같은 타입일 경우
        if (currentState?.GetType() == newState.GetType())
        {
            return;
        }

        //
        currentState?.Exit();

        //
        currentState = newState;

        //
        currentState.Enter();

        //
        Debug.Log($"상태 전환 되는 스테이트 : {newState.GetType().Name}");

    }
}
