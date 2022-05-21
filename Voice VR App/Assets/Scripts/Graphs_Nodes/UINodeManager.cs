using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

//this listens on two events. Move which happens when a player says that command. it will launch all neighboring nodes here.
//the other is for when a hit was detected from the raycast Gaze.cs script.
public class UINodeManager : MonoBehaviour
{
    [SerializeField] private CommandHeardChannelSO _moveEvent = default;
    [SerializeField] private HitNodeChannelSO _hitEvent = default;

    [SerializeField] private GameObject myPrefab;
    private GraphController graphController;
    private bool currentlyActive = false;
    private CapsuleCollider playerReferenceForColliderToggle;

    private Dictionary<Vector3, GameObject> prefabAccess = new Dictionary<Vector3, GameObject>();

    public float UserInterfaceRadius; //This Controls the distance between the player and the waypoint.
    public float UserInterfaceHeight; //The height at which waypoints will appear.
   

    void Awake()
    {
        RegisterChannel(_moveEvent, _hitEvent);
    }

    private void Start() {
        graphController = GameObject.FindWithTag("MainCamera").GetComponent<GraphController>();
        playerReferenceForColliderToggle = graphController.transform.GetComponent<CapsuleCollider>();
        playerReferenceForColliderToggle.radius = UserInterfaceRadius;
        playerReferenceForColliderToggle.enabled=false;

        //IList<GraphNode> myNodes = graphController.GetAllNodes();
        //GameObject currentPrefab;
        //foreach (GraphNode node in myNodes){
        //    currentPrefab = Instantiate(myPrefab, node.Position,Quaternion.identity);
        //    currentPrefab.SetActive(false);

            //adds to dictionary
        //    prefabAccess[node.Position] = currentPrefab;
            //this makes this object a child of the gameobject this script is attached to.
        //    currentPrefab.transform.parent = transform;
        //}
    }

    private void OnDestroy()
	{
		UnregisterChannel(_moveEvent, _hitEvent);
	}

    void OnEventHeard(){
        Debug.Log("I heard move event");
        ActivateDestinationsToggle();
    }

    private void RegisterChannel(CommandHeardChannelSO moveCueEventChannel, HitNodeChannelSO hitCueEventChannel)
	{
		moveCueEventChannel.onCommandHeard += OnEventHeard;
        hitCueEventChannel.onHit += OnHit;
	}
    private void UnregisterChannel(CommandHeardChannelSO moveCueEventChannel, HitNodeChannelSO hitCueEventChannel)
	{
		moveCueEventChannel.onCommandHeard -= OnEventHeard;
        hitCueEventChannel.onHit -= OnHit;
	}

    //this toggles all false or true
    private void ActivateDestinationsToggle(){
        currentlyActive = !currentlyActive;
        Debug.Log("Setting all Destinations to " + currentlyActive);
        IList<GraphNode> possibleDestinations = graphController.GetAllCurrentNodeNeighbors();
        //foreach(GraphNode node in possibleDestinations){
        //    prefabAccess[node.Position].SetActive(currentlyActive);
        //}

        if(currentlyActive){
            if(prefabAccess.Count == 0){
                Debug.Log("first time creating prefabs");
                //will show things if first time in this node.
                //activate player's capsule collider
                playerReferenceForColliderToggle.enabled = true;
                //create or make them all visible
                foreach(GraphNode node in possibleDestinations){
                    if (Physics.Linecast(node.Position, playerReferenceForColliderToggle.transform.position, out RaycastHit hit)){
                        Debug.Log("WAYPOINT HIT!");
                        GameObject tmp = Instantiate(myPrefab,new Vector3(Mathf.Round(hit.point.x * 10.0f) * 0.1f,Mathf.Round(hit.point.y * 10.0f) * 0.1f,Mathf.Round(hit.point.z * 10.0f) * 0.1f),Quaternion.identity);
                        tmp.transform.LookAt(playerReferenceForColliderToggle.transform);
                        //set text
                        tmp.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().SetText(node.Value.src);
                        //add to dictionary

                        tmp.GetComponent<WaypointData>().nodePosition = node.Position;
                        prefabAccess[tmp.transform.position] = tmp;
                        //make this the prefab a child of UINodeManager
                        tmp.transform.parent = transform;
                    }
                }
                playerReferenceForColliderToggle.enabled=false;
            }else{
                //foreach(GraphNode node in possibleDestinations){
                //    prefabAccess[node.Position].SetActive(currentlyActive);
                //}
                foreach (Vector3 key in prefabAccess.Keys){
                    prefabAccess[key].SetActive(currentlyActive);
                }

            }
        }else{
            //hide, for now destroy...make a factory object
            //clear dictionary???
            //

            //if(prefabAccess.Count != 0){
            //    foreach(GraphNode node in possibleDestinations){
            //        prefabAccess[node.Position].SetActive(currentlyActive);
            //    }
            //}

            foreach (Vector3 key in prefabAccess.Keys){
                    prefabAccess[key].SetActive(currentlyActive);
            }
            
        }


    }

    void OnHit(Vector3 selectedPosition){
        //some error checking needed

        Debug.Log("one of my objects has been selected! at "+ selectedPosition);
        ActivateDestinationsToggle();
        //prefabAccess[selectedPosition].SetActive(false); 
        //graphController.ChangeCurrentNode(selectedPosition);
        graphController.ChangeCurrentNode(prefabAccess[selectedPosition].GetComponent<WaypointData>().nodePosition);
        prefabAccess.Clear(); //clears all in dict...waits until next time.

        //give the prefab a component taht makes it animate on selection and call it here        
        //WAIT FOR ANIMATION TO BE FINISHED.THEN DEACTIVATE ALL.
        //deactivate itself.
        
    }
        


}
