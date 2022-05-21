using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

//made only to hit a node in move mode.
[CreateAssetMenu(menuName = "Events/Hit Command Channel")]
public class HitNodeChannelSO : ScriptableObject
{   
    //what if i just take a position and then search based on that? because wouldn't passing a reference be more performant? idk.
     public UnityAction<Vector3> onHit;
     public void RaiseEvent(Vector3 destination){
        if (onHit != null) //we always check if the unityaction is null
		{
			onHit.Invoke(destination);
		}
		else
		{
			Debug.LogWarning("A CommandHeardSO was requested, but nobody picked it up. " +
				"Check why there is no CommandHeard already loaded, " +
				"and make sure it's listening on this CommandHeardSO Event channel.");
		}
    }
}