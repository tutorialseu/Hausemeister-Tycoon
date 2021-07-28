using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{

    [SerializeField] string androidID = "4137731";

    //placements Ids
    [SerializeField] string doubleMoneyVideoPlacementId = "doubleMoneyVideo";
    [SerializeField] string skippableVideoPlacementId = "SkippableVideo";


    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);

        //initalize the ads using the android id
        Advertisement.Initialize(androidID);
    }

    public void ShowDoubleMoneyAd()
    {
        Advertisement.Show(doubleMoneyVideoPlacementId);
    }

    public void ShowSkippableAd()
    {
        //show skippable ad
        Advertisement.Show(skippableVideoPlacementId);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        //check if the player finished watching the ad 
        if (showResult == ShowResult.Finished)
        {
            //check if the ad that was watched is the double money video ad
            if (placementId == doubleMoneyVideoPlacementId)
            {
                //in that case
                //debug in the console to check if it's working
                Debug.Log("Double Money");
                //call the add money method of our money manager and add it's current amount of money to it. which means double money
                MoneyManager.instance.AddMoney(MoneyManager.instance.Money);
            }
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsReady(string placementId)
    {
        //throw new System.NotImplementedException();
    }
}
