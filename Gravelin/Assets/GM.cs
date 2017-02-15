using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{

    public List<GameObject> playerPrefabs;

    private bool[] _instantiatedPlayers;
    // Use this for initialization
    void Start()
    {
        //StartGame(2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame(int nrOfPlayers)
    {
        _instantiatedPlayers = new bool[nrOfPlayers];

        for (int index = 0; index <= nrOfPlayers - 1; index++)
        {
            var player = (GameObject)Instantiate(Resources.Load("Player"));
            player.GetComponent<Player>().playerNumber = index + 1;
            player.transform.position = new Vector3(0.9f * Random.value, 1, 0);
            var camera = player.transform.FindChild("Camera").GetComponent<Camera>();
            switch (index)
            {
                case 0:
                    camera.rect = nrOfPlayers > 3 ? new Rect(0, 0.5f, 0.5f, 0.5f) : new Rect(0, 0.5f, 1, 0.5f);
                    break;
                case 1:
                    camera.rect = nrOfPlayers > 3 ? new Rect(0.5f, 0.5f, 0.5f, 0.5f) : new Rect(0, 0, 0.5f, 0.5f);
                    if(nrOfPlayers == 2)
                        camera.rect = new Rect(0,0, 1, 0.5f);
                    break;
                case 2:
                    camera.rect = nrOfPlayers > 3 ? new Rect(0, 0, 0.5f, 0.5f) : new Rect(0.5f, 0, 0.5f, 0.5f);
                    break;
                case 3:
                    camera.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                    break;
            }

            camera.enabled = true;
        }
        gameObject.SetActive(false);
    }
}
