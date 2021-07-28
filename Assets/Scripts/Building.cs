using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class Building : MonoBehaviour
{

    //building id
    [SerializeField] private int id;

    //reference to our building visuals
    [SerializeField] private GameObject buildingVisuals;
    //reference to the buy button
    [SerializeField] private GameObject buyButton;
    //a string to fill the cost using the editor
    [SerializeField] private string costRepresentation;

    //reference to the collect profit button
    [SerializeField] private GameObject collectProfitButton;

    //reference to our collect profit button text
    private Text collectProfitButtonText;

    //a field that holds the current level of the building
    [SerializeField] private int buildingLvl = 1;
    //a field that holds the profit multiplier 
    [SerializeField] private int profitMultiplier = 1;
    //current building profit
    [SerializeField] BigInteger profit;

    //a field that holds the upgrade cost multiplier
    [SerializeField] private int upgradeCostMultiplier = 10;

    //Property to return the next upgrade cost
    private BigInteger NextUpgradeCost
    {
        get
        {
            //calculate the next upgrade cost
            return buildingLvl * upgradeCostMultiplier;
        }
    }

    //reference to the upgrade button
    [SerializeField] private GameObject upgradeButton;
    //reference to our upgrade button text 
    private Text upgradeButtonText;

    public BigInteger Cost
    {
        //get the cost of the building by converting the cost representation into a big integer
        get { return BigInteger.Parse(costRepresentation); }
        //when we set the cost store it in the cost representation by converting the value of the big integer to a string
        set { costRepresentation = value.ToString(); }
    }

    //a bool to store the state of our building state.
    private bool isUnlocked = false;


    //reference to our buy button text
    private Text buyButtonText;


    // Start is called before the first frame update
    void Start()
    {
        //get the text of of our buy button
        buyButtonText = buyButton.GetComponentInChildren<Text>();

        //set the buy button text to our cost
        buyButtonText.text = MoneyFormatter.FormatMoney(Cost);

        //set our button to be the opposite of our building state,
        //if the building is locked we need to display the buy button
        buyButton.SetActive(!isUnlocked);
        //set our building  visuals to our building state,
        //if the building is unlocked we need to show the building model
        buildingVisuals.SetActive(isUnlocked);

        //get a reference of the collect profit button text
        collectProfitButtonText = collectProfitButton.GetComponentInChildren<Text>();

        //set our collect button based on the state of the building,
        //if it's unlocked then our collect profit button should be enabled
        collectProfitButton.SetActive(isUnlocked);


        //get the reference to the upgrade button text
        upgradeButtonText = upgradeButton.GetComponentInChildren<Text>();
        //set our upgrade button based on the state of the building,if it's unlocked then our upgrade button should be enabled.
        upgradeButton.SetActive(isUnlocked);

        UpdateUpgradeUI();

        StartCoroutine(MakeProfit());
    }

    IEnumerator MakeProfit()
    {
        while (true)
        {
            //check if the building is unlocked
            if (isUnlocked)
            {
                //add the profit to our current profit
                profit += buildingLvl * profitMultiplier;
                //update the profit UI
                UpdateProfitUI();
            }

            yield return new WaitForSecondsRealtime(1f);

        }
    } 



    // profit per second = building level X profit multiplier.

    private void UpdateProfitUI()
    {
        //set our collect profit button text to our current profit
        collectProfitButtonText.text = MoneyFormatter.FormatMoney(profit);
    }


    private void UpdateUpgradeUI()
    {
        //update the upgrade button text
        upgradeButtonText.text = $"^\nLVL {buildingLvl}\n{MoneyFormatter.FormatMoney(NextUpgradeCost)}";
    }


    // The next upgrade cost = upgrade cost multiplier X building level.

    public void OnBuyButton()
    {
        //if our building is not unlcocked
        if (!isUnlocked)
        {
            //check if the buy operation was succesful, in other words if we have enough money to buy this building.
            if (MoneyManager.instance.Buy(Cost))
            {
                //change the building state to unlcocked
                isUnlocked = true;
                //enable the building visuals.
                buildingVisuals.SetActive(isUnlocked);
                //disable the buy button
                buyButton.SetActive(!isUnlocked);

                collectProfitButton.SetActive(isUnlocked);

                //enable the upgrade button
                upgradeButton.SetActive(isUnlocked);
            }
        }
    }

    public void OnCollectProfitButton()
    {
        MoneyManager.instance.AddMoney(profit);
        profit = 0;

        UpdateProfitUI();
    }

    public void OnUpgradeButton()
    {
        //if we have enough money
        if (MoneyManager.instance.Buy(NextUpgradeCost))
        {
            //increase the building lvl by 1 
            buildingLvl += 1;
            //upgrade the upgrade UI
            UpdateUpgradeUI();
        }
    }

    private void SaveBuildingData()
    {
        //create a building data object using the current values of our building
        BuildingData bd = new BuildingData(isUnlocked, buildingLvl, profit.ToString());
        //convert our building data to json
        string json = BuildingData.CreateJSONFromBuilding(bd);
        //save our building with the building +id as a key and the json string itself as the value
        PlayerPrefs.SetString("building" + id, json);
        //save
        PlayerPrefs.Save();
    }

    private void LoadBuildingData()
    {
        //try and load the building data, if we never saved this building before return an empty string
        string json = PlayerPrefs.GetString("building" + id, "");

        //our building data is null for now
        BuildingData bd = null;

        if (json.Equals(""))
        {
            //if yes then we need to create a new building with default values
            bd = new BuildingData(false, 1, "0");
        }
        else
        {
            //since we have a saved data for this building convert the json to a building  data object
            bd = BuildingData.CreateBuildingFromJSON(json);
        }

        isUnlocked = bd.IsUnlocked;
        buildingLvl = bd.BuildingLvl;
        profit = BigInteger.Parse(bd.Profit);
        

    }

    private void Awake()
    {
        LoadBuildingData();
    }


    //for mobile devices
    private void OnApplicationPause(bool pause)
    {
        //if the application is paused
        if (pause)
        {
            SaveBuildingData();
        }
    }

    //for the editor
    private void OnApplicationQuit()
    {
        //on application quit save the building data
        SaveBuildingData();
    }
}
