using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class MoneyManager : MonoBehaviour
{
    //reference to our money UI
    [SerializeField] private Text moneyUI;
    //a property to hold our current spending money
    public BigInteger Money { get; private set; }

    //static reference to our money manager
    public static MoneyManager instance;

    private void UpdateMoneyUI()
    {
        //set the money UI to be our current spending money
        moneyUI.text = string.Format("{0}", MoneyFormatter.FormatMoney(Money));
    }

    // Start is called before the first frame update
    void Start()
    {
        //get the Money value of our Money key from the player prefs and parse it into a big integer,
        //if we never saved this value before it will return 0
        Money = BigInteger.Parse(PlayerPrefs.GetString("Money", "0"));
        UpdateMoneyUI();
        //set the instance to be this copy of the money manager
        instance = this;
    }

    public bool Buy(BigInteger cost)
    {
        //by default assume the buy operation was not successfull
        bool isBuyOPSuccessfull = false;
        //if the cost of the thing we want to buy us less than or equals to our current spending money
        if (cost <= Money)
        {
            //subtract the cost from our money
            Money -= cost;
            //set the buy operation to true
            isBuyOPSuccessfull = true;
        }
        //update our money UI
        UpdateMoneyUI();
        //return the buy operation bool, if the buy operation failed it will be false
        return isBuyOPSuccessfull;
    }


    //A method to add money to our manager
    public void AddMoney(BigInteger profit)
    {
        //if the profit we want to add is greater than 0, this is for protection so we don't add negative values.
        if (profit > 0)
        {
            //add the profit to our money
            Money += profit;
            //update the money UI
            UpdateMoneyUI();
        }
    }


    private void SaveMoney()
    {
        //save our money as string
        PlayerPrefs.SetString("Money", Money.ToString());
        //make sure we save the player prefs because the game will close soon
        PlayerPrefs.Save();
    }

    //for mobile devices
    private void OnApplicationPause(bool pause)
    {
        //if the application is paused
        if (pause)
        {
            //save the money
            SaveMoney();
        }
    }

    //for the editor
    private void OnApplicationQuit()
    {
        //on application quit save the money
        SaveMoney();
    }

}
