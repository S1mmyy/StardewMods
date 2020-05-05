using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using Harmony;

namespace Skull_Cavern_Drill
{
    public interface IApi
    {
        int GetBigCraftableId(string name);
    }

    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var harmony = HarmonyInstance.Create(this.ModManifest.UniqueID);

            harmony.Patch(
            original: AccessTools.Method(typeof(StardewValley.Object), nameof(StardewValley.Object.placementAction)),
            prefix: new HarmonyMethod(typeof(ObjectPatches), nameof(ObjectPatches.PlacementAction_Prefix))
            );
        }
    }

    internal class ObjectPatches
    {
        private static IApi api;
        private static IMonitor Monitor;

        public static void Initialize(IModHelper helper, IMonitor monitor)
        {
            api = helper.ModRegistry.GetApi<IApi>("spacechase0.JsonAssets");
            Monitor = monitor;
        }

        internal static bool PlacementAction_Prefix(StardewValley.Object __instance, GameLocation location, int x, int y, Farmer who = null)
        {
            try
            {
                if (__instance.ParentSheetIndex == api.GetBigCraftableId("Dwarven Drill") && location is MineShaft && Game1.CurrentMineLevel > 120)
                {
                    (location as MineShaft).createLadderDown(x, y, true);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(PlacementAction_Prefix)}:\n{ex}", LogLevel.Error);
                return false;
            }
        }
    }
}
