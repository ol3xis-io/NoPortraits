using UnityEngine;
using System.Reflection;
using System.Collections;
using HarmonyLib;
using RhythmRift;
using NoPortraits;

public static class RemovePortraits
{
    public static void RemovePortrait(MonoBehaviour controller, string newId)
    {
        var type = controller.GetType();
        var idField = type.GetField("_heroPortraitId", BindingFlags.NonPublic | BindingFlags.Instance);
        idField?.SetValue(controller, newId);
        var parentField = type.GetField("_heroPortraitParent", BindingFlags.NonPublic | BindingFlags.Instance);
        Transform parent = parentField?.GetValue(controller) as Transform;
        var viewField = type.GetField("_heroPortraitViewInstance", BindingFlags.NonPublic | BindingFlags.Instance);
        var currentView = viewField?.GetValue(controller) as MonoBehaviour;

        if (currentView != null)
            Object.Destroy(currentView.gameObject);
        var loadMethod = type.GetMethod("LoadCharacterPortrait", BindingFlags.NonPublic | BindingFlags.Instance);

        IEnumerator routine = (IEnumerator)loadMethod.Invoke(controller, new object[]
        {
            newId,
            parent,
            true,
            null,
            null
        });
        controller.StartCoroutine(routine);
    }
    public static void RemoveCounterpartPortrait(MonoBehaviour controller, string newId)
    {
        var type = controller.GetType();
        var idField = type.GetField("_counterpartPortraitId", BindingFlags.NonPublic | BindingFlags.Instance);
        idField?.SetValue(controller, newId);
        var parentField = type.GetField("_counterpartPortraitParent", BindingFlags.NonPublic | BindingFlags.Instance);
        Transform parent = parentField?.GetValue(controller) as Transform;
        var viewField = type.GetField("_counterpartPortraitViewInstance", BindingFlags.NonPublic | BindingFlags.Instance);
        var currentView = viewField?.GetValue(controller) as MonoBehaviour;

        if (currentView != null)
            Object.Destroy(currentView.gameObject);
        var loadMethod = type.GetMethod("LoadCharacterPortrait", BindingFlags.NonPublic | BindingFlags.Instance);

        IEnumerator routine = (IEnumerator)loadMethod.Invoke(controller, new object[]
        {
            newId,
            parent,
            true,
            null,
            null
        });
        controller.StartCoroutine(routine);
    }
}
[HarmonyPatch(typeof(RRPortraitUiController), "Initialize")]
class PatchInitialize
{
    static void Postfix(RRPortraitUiController __instance)
    {
        __instance.StartCoroutine(DelayedChange(__instance));
    }

    static IEnumerator DelayedChange(RRPortraitUiController controller)
    {
        yield return new WaitForSeconds(0.4f);
        if (NoPortraitsMod.configRemoveCandace.Value)
        {
            RemovePortraits.RemovePortrait(controller, "I'M AN INVALID ID");
        }
        if (NoPortraitsMod.configRemoveCounterpart.Value)
        {
            RemovePortraits.RemoveCounterpartPortrait(controller, "I'M AN INVALID ID");
        }
    }
}