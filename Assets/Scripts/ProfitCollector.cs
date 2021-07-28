using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfitCollector : MonoBehaviour
{
    //the interval at which we will collect profits
    [SerializeField] private float profitCollectionInterval = 5f;

    //reference to our dog avatar (raw image)
    [SerializeField] private GameObject dogAvatar;

    //a variable to count the time internally
    private float profitCollectionTimer = 0;

    //a list of the buildings to collect profits from
    private List<Building> buildings;

    //a variable to check if the manager is hired, 0 means false, 1 means true
    private int isManagerHired = 0;


    // Start is called before the first frame update
    void Start()
    {
        //check if the manager was bought. if not return 0 which means false
        isManagerHired = PlayerPrefs.GetInt("isManagerHired", 0);

        //if the manager us hired turn the dog avatar on
        dogAvatar.SetActive(isManagerHired == 1 ? true : false);
        //initialize the list of buildings
        buildings = new List<Building>();

        //find all the buildings components in the game and add them to our buildings list.
        buildings.AddRange(GameObject.FindObjectsOfType<Building>());

    }

    // Update is called once per frame
    void Update()
    {
        //check if the manager is hired
        if (isManagerHired == 1)
        {
            //add the time passed to our timer
            profitCollectionTimer += Time.deltaTime;


            if (profitCollectionTimer >= profitCollectionInterval)
            {
                //reset the timer
                profitCollectionTimer = 0;

                foreach(Building building in buildings)
                {
                    building.OnCollectProfitButton();
                }

            }

        }
    }

    public void HireManager()
    {
        //the manager was hired.
        //set the isManagerHired to 1, which means true.
        isManagerHired = 1;
        //enable the dog avatar
        dogAvatar.SetActive(true);
        //save it in the player prefs
        PlayerPrefs.SetInt("isManagerHired", isManagerHired);
        Debug.Log("HireManager called");
    }


}
