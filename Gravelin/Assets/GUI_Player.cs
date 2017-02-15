using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Player : MonoBehaviour
{

    private Text KilledPlayerText;
    private GameObject KillFeedPanel;
	// Use this for initialization
	void Start ()
	{
	    KilledPlayerText =
	        transform.FindChild("Camera").FindChild("Canvas").FindChild("Killed Something Text").GetComponent<Text>();
	    KilledPlayerText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowPlayerKilledText(string playerName)
    {
        
        StartCoroutine(ShowPlayerKilled(playerName,5));

    }

    private IEnumerator ShowPlayerKilled(string playerName, int seconds)
    {
        KilledPlayerText.text = "Killed" + playerName;
        KilledPlayerText.enabled = true;
        yield return new WaitForSeconds(seconds);
        KilledPlayerText.enabled = false;
    }

}
