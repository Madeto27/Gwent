using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        var cardScene = GD.Load<PackedScene>("res://Scenes/cardScene.tscn");
        UnitCard unitCard = (UnitCard)cardScene.Instantiate();

        unitCard.name = tempData.GetProperty("name").GetString();
        unitCard.power = tempData.GetProperty("power").GetInt32();
        unitCard.row = tempData.GetProperty("row").GetInt32();
        unitCard.desc = tempData.GetProperty("desc").GetString();
        unitCard.texture = tempData.GetProperty("texture").GetString();
        unitCard.amount = tempData.GetProperty("amount").GetInt32();
        unitCard.ability = CreateAbilityFromDesc(unitCard.desc);

        unitCard.Initialize(unitCard.name, unitCard.power, unitCard.desc, unitCard.row, unitCard.texture, unitCard.amount, unitCard.ability);
        
        return unitCard;
    }

    public List<string> GetCardIds()
    {
        return data.Keys.ToList();
    }

    public int GetCardAmount(string cardId){
        if (data.ContainsKey(cardId) && data[cardId].TryGetProperty("amount", out var amountProperty)){
            return amountProperty.GetInt32();
        }
        return 1;
    }

    private static readonly Dictionary<string, ICardAbility> AbilityMap = new()
    {
        { "Medic", new Medic() },
        { "TightBond", new TightBond() },
        { "Spy", new Spy() },
        { "MoraleBoost", new MoraleBoost() },
        { "BitingFrost", new BitingFrost()},
        { "TorrentialRain", new TorrentialRain()},
        { "ImpenetrableFog", new ImpenetrableFog()},
        { "ClearWeather", new ClearWeather()},
        { "None", null }
    };

    private ICardAbility CreateAbilityFromDesc(string desc)
    {
        if (AbilityMap.TryGetValue(desc, out var ability)){
            return ability;
        }
        return null;
    }
}