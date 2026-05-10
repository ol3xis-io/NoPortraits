using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System.Security.Cryptography.X509Certificates;

namespace NoPortraits;

[BepInPlugin("io.ol3xis.plugins.NoPortraits", "NoPortraits", "1.0.0.0")]
[BepInProcess("RiftOfTheNecroDancer")]
public class NoPortraitsMod : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    
    public static ConfigEntry<bool> configRemoveCandace;
    public static ConfigEntry<bool> configRemoveCounterpart;
    public static ConfigEntry<float> configWaitTime;
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        // Config logic
        configRemoveCandace = Config.Bind("General.Toggles", "RemoveCadence", true, "Remove Cadence in Rhythm Rifts");
        configRemoveCounterpart = Config.Bind("General.Toggles", "RemoveCounterpart", true, "Remove counterpart character (character on the right) in Rhythm Rifts");
        configWaitTime = Config.Bind("General", "WaitTime", 0.5f, "How many seconds to wait before removing the portraits; increase if the mod doesn't work sometimes");
        // Harmony
        var harmony = new Harmony("io.ol3xis.plugins.NoPortraits");
        harmony.PatchAll();
    }
}