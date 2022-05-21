using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSM : StateMachineMB
{

    public MenuRaycastState MenuState { get; private set; }
    public EmptyRaycastState EmptyState { get; private set; }
    public MoveRaycastState MoveState { get; private set; }
    public SelectRaycastState SelectState { get; private set; }

    [SerializeField] private CommandHeardChannelSO _moveEvent = default;

    private Gaze gaze; //access to the gaze script on same object

    
    void Awake(){
        
    }

    //get whatever data you need to feed into the statemachine and pass it to the states.
    // Start is called before the first frame update
    void Start()
    {
        MenuState = new MenuRaycastState(this);
        EmptyState = new EmptyRaycastState(this);
        gaze = gameObject.GetComponent<Gaze>(); 
        MoveState = new MoveRaycastState(this, _moveEvent, gaze);
        SelectState = new SelectRaycastState(this);
        

        //we always start here.
        //ChangeState(MenuState);
        ChangeState(EmptyState);
    }
}
