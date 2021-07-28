using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

[Serializable]
public class BuildingData 
{
    //is the building unlocked
    [SerializeField] public bool IsUnlocked { get; set; }
    //building lvl
    [SerializeField] public int BuildingLvl { get; set; }
    //current building profit
    [SerializeField] public string Profit { get; set; }

    //simple constructor
    public BuildingData(bool isUnlocked, int buildingLvl, string profit)
    {
        this.IsUnlocked = isUnlocked;
        this.BuildingLvl = buildingLvl;
        this.Profit = profit;
    }

    public static string CreateJSONFromBuilding(BuildingData building)
    {
        //convert the the building data to json string
        return JsonConvert.SerializeObject(building);
    }

    public static BuildingData CreateBuildingFromJSON(string jsonString)
    {
        //convert the json string to building data object
        return JsonConvert.DeserializeObject<BuildingData>(jsonString);
    }
}
