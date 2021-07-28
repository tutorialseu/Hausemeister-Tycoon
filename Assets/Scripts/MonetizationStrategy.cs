using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonetizationStrategy : MonoBehaviour
{
    //reference to the double money panel
    [SerializeField] private GameObject DoubleMoneyPanel;
    //the interval at which we will show the double money panel
    [SerializeField] private float doubleMoneyAdInterval = 60f;
    //the interval at which we will show the skippable ads
    [SerializeField] private float skippableVideoAdInterval = 150f;

    IEnumerator DoubleMoneyAdRoutine()
    {
        //first disable the double money panel
        DoubleMoneyPanel.SetActive(false);

        //this routine will never stop.
        while (true)
        {
            //wait for some time, here it's the doubleMoneyAdInterval
            yield return new WaitForSeconds(doubleMoneyAdInterval);
            //check if the panel is not enabled
            if (!DoubleMoneyPanel.activeSelf)
            {
                //show the panel
                DoubleMoneyPanel.SetActive(true);
            }
        }
    }

    IEnumerator SkippableVideoAdRoutine()
    {

        //this routine will never stop.
        while (true)
        {   
            if(PlayerPrefs.GetInt("adsRemoved", 0) == 1)
            {
                yield break;
            }

            //wait for some time, here it's the skippableVideoAdInterval
            yield return new WaitForSeconds(skippableVideoAdInterval);

            GetComponent<AdsManager>().ShowSkippableAd();
            
        }
    }


        // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoubleMoneyAdRoutine());
        StartCoroutine(SkippableVideoAdRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
