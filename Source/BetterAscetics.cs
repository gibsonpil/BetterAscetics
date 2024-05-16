using Verse;
using HarmonyLib;
using System.Reflection;

namespace BetterAscetics 
{
    [StaticConstructorOnStartup]
    public static class BetterAscetics 
    {
        static BetterAscetics() 
        {
            // Load patches.
            Harmony harmony = new Harmony("com.gibsonpil.betterascetics");
            harmony.PatchAll();

            Log.Message("[BetterAscetics] Loaded!");
        }
    }
}
