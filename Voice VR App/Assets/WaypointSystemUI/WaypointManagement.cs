using TMPro;
using UnityEngine;

public class WaypointManagement : MonoBehaviour
{
    public GameObject UIElementPrefab; //The UI Element that will be used.    
    public Transform player; //The entity to display the UI around.
    public GameObject currentNode; //The Current Node the player has active.
    public int UserInterfaceRadius; //This Controls the distance between the player and the waypoint.
    public int UserInterfaceHeight; //The height at which waypoints will appear.
    public GameObject[] neighbors; //This is a list of the currentNode's neighbors.

    private GameObject lastKnownNode = null; //Used to track if the current node has been switched.
    private GameObject[] createdUIElements; //This is a list of all created UI elements. Used for Garbage Disposal.
    private Vector3 spawnVector; //Temporary Variable Used for Spawning Waypoints.
    private CapsuleCollider waypointCollider; //The collider that is used for raycast collisions.


    // Start is called before the first frame update
    void Start()
    {
        if(currentNode == null)
            Debug.Log("WaypointManagement.cs | Error: currentNode variable is null.");

        if (player == null)
            Debug.Log("WaypointManagement.cs | Error: player variable is null.");

        if (UserInterfaceRadius <= 0)
            Debug.Log("WaypointManagement.cs | Error: UserInterfaceRadius variable is <= 0.");

        if (UserInterfaceHeight <= 0)
            Debug.Log("WaypointManagement.cs | Error: UserInterfaceHeight variable is <= 0.");

        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null && UserInterfaceRadius > 0) //If No Errors Have Occured
        {
            /* Checking to see if current node has been switched to update neighbor.
             * If the currentNode has changed, update all UI elements.
             * If not, do nothing.
             */
            if (lastKnownNode != currentNode)
            {
                lastKnownNode = currentNode;
                //TODO: Add some code here to update the neighbors[] node list from the current Node
                waypointCollider = player.GetComponent<CapsuleCollider>(); //Get the CapsuleCollider we use for raycast collisions
                waypointCollider.radius = UserInterfaceRadius; //Change radius of capsule collider to user input.

                //If UI Elements already exist, delete them.
                if (createdUIElements != null)
                {
                    for (int index = 0; index < createdUIElements.Length; index++)
                    {
                        Destroy(createdUIElements[index]); //Self Explanatory                      
                    }

                }
                createdUIElements = new GameObject[neighbors.Length];//Create List to store all waypoint prefabs

                //For each Node, do a ray cast from it's position to the player, collect the collision points, and spawn waypoint
                for (int index = 0; index < neighbors.Length; index++)
                {

                    //If Collision Was Found Between Node and Player
                    if (Physics.Linecast(neighbors[index].transform.position, player.position, out RaycastHit hit))
                    {
                        //Generation of waypoints happens here

                        Debug.Log("WaypointManagement.cs | Node can see player with raycast");                        
                        spawnVector = new Vector3(); //Initiate Spawn Position
                        spawnVector.x = hit.point.x; //Set X Pos of Waypoint
                        spawnVector.y = UserInterfaceHeight; //Set X Pos of Waypoint                   
                        spawnVector.z = hit.point.z; //Set X Pos of Waypoint

                        GameObject tmp = Instantiate(UIElementPrefab); //Spawn Waypoint
                        tmp.transform.position = spawnVector; //Set Position of Waypoint
                        tmp.transform.LookAt(player); //Rotate Waypoint To Face Player
                        GameObject child = tmp.transform.GetChild(0).gameObject; //Get the child component so that we can edit the text of waypoint
                        child.GetComponent<TextMeshPro>().SetText("Waypoint");//TODO: Replace this line of code for how you want the title's to appear
                        createdUIElements[index] = tmp; //Add waypoint to the current list of existing waypoints
                        
                        
                    }
                    else
                    {
                        Debug.Log("WaypointManagement.cs | Node cannot see player with raycast");
                    }                    
                }
            }            
        }
    }
}
