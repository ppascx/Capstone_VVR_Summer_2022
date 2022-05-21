using System.Collections;
using UnityEngine.Events;
using UnityEngine;
//for any command event that doesn't need an argument, used on Move
//we add to the event
[CreateAssetMenu(menuName = "Events/Command Heard Cue Channel")]
public class CommandHeardChannelSO : ScriptableObject
{
    public UnityAction onCommandHeard;
    public void RaiseEvent(){
        if (onCommandHeard != null) //we always check if the unityaction is null
		{
			onCommandHeard.Invoke();
		}
		else
		{
			Debug.LogWarning("A CommandHeardSO was requested, but nobody picked it up. " +
				"Check why there is no CommandHeard already loaded, " +
				"and make sure it's listening on this CommandHeardSO Event channel.");
		}
    }
}
