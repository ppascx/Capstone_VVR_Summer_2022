using System.Collections;
using System;

//https://github.com/metalac190/UnityDesignPatterns/blob/master/Assets/Patterns/State/Reusable/IState.cs
public interface IState
{
    // automatically gets called in the State machine. Allows you to delay flow if desired
    void Enter();
    // allows simulation of Update() method without a MonoBehaviour attached
    //void Tick();
    // allows simulatin of FixedUpdate() method without a MonoBehaviour attached
    //void FixedTick();
    // automatically gets called in the State machine. Allows you to delay flow if desired

    //gets a command and decides what to do with it. if the state can handle it.
    void DoState(string command);

    //SwitchNodeState, to be used by the node content statemachine. allows for setting up when different content types are in the same graph
    void SwitchContentState(string contentNodeTypeName);
    
    //LoadOnContentState, to be used by the node content statemachine. loads the content data from the currentNode.
    void LoadOnContentState(string source);

    void Exit();
}
