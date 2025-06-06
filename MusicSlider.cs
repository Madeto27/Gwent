using Godot;
using System;

public partial class MusicSlider : HSlider
{
    [Export] public string busName = "Music";
    public override void _Ready()
    {
        int busIndex = AudioServer.GetBusIndex(busName);
        float currentDb = AudioServer.GetBusVolumeDb(busIndex);
        Value = Mathf.DbToLinear(currentDb);
        ValueChanged += OnSliderValueChanged;
    }

    private void OnSliderValueChanged(double value)
    {
        int busIndex = AudioServer.GetBusIndex(busName);
        float newVolumeDb = Mathf.LinearToDb((float)value);
        AudioServer.SetBusVolumeDb(busIndex, newVolumeDb);
    }
}
