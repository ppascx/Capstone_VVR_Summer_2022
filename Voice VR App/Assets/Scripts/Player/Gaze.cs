using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Gaze : MonoBehaviour
{
    [SerializeField] private HitNodeChannelSO _hitEvent = default;
    [SerializeField] private LayerMask interactableLayer;
    public float rayDistance;
    public float raySphereRadius;
    [SerializeField] private Camera cam;
    private bool interacting;
    private float holdtimer;

    //will hold the position of the last thing we hit...we don't care about the object itself.
    //lmao vector3 is NOT NULLABLE.
    private Transform selection;
    private Vector3 hitPosition;

    void Start(){
        cam = Camera.main;
        hitPosition = Vector3.zero;
        selection = null;
    }
    void Update(){
        CheckForInteractable();
    }

    void CheckForInteractable(){
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;

        bool hitSomething = Physics.SphereCast(ray, raySphereRadius, out hitInfo, rayDistance, interactableLayer);
        if(hitSomething){

            Debug.Log("UI HIT!");
            //if have no previous selection atm, if it is null...
            if(selection == null){
                //ASSIGN to the new thing we hit.
                selection = hitInfo.transform;     
                hitPosition = hitInfo.transform.position;
            }else{
                //if it is NOT the same interactable that we have already selected before....
                if(selection != hitInfo.transform){
                    //note that for this to work no 2 nodes can be in the same position...backend should take care of these rules.
                    //ASSIGN.
                    selection = hitInfo.transform;     
                    hitPosition = hitInfo.transform.position;
                }
                //else 
                    //nothing happens.
            }
        }else{
            //we reset selection to point to null.
            selection = null;     
            hitPosition = Vector3.zero;
        }
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, hitSomething ? Color.green : Color.red);
    }
    public bool Shoot(){
        //checks on raycast to move there.
        //if it hit something...we launch event for UI and for GraphController and return true.
        if(selection != null){
            _hitEvent.RaiseEvent(hitPosition);
        }else{
            return false;
        }
        return true;
    }

    

}
