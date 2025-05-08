using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using Godot;

class UnitCreator : Creator 
{
    private Dictionary<string, JsonElement> data;
    public void LoadData(){
        string path = "res://unitData.json";
        if (FileAccess.FileExists(path)){
            using var json = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            string jsonString = json.GetAsText();
            json.Close();

            data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);
        }
        else {
            GD.Print("File doesn't exist");
        }
    }

    public override CardScene CreateCard(string cardId){

        var tempData = data[cardId];

        var cardScene = GD.Load<PackedScene>("res://cardScene.tscn");
        UnitCard unitCard = (UnitCard)cardScene.Instantiate();

        unitCard.name = tempData.GetProperty("name").GetString();
        unitCard.power = tempData.GetProperty("power").GetInt32();
        unitCard.row = tempData.GetProperty("row").GetInt32();
        unitCard.desc = tempData.GetProperty("desc").GetString();
        unitCard.texture = tempData.GetProperty("texture").GetString();

        unitCard.Initialize(unitCard.name, unitCard.power, unitCard.desc, unitCard.texture);
        
        return unitCard;
    }
}