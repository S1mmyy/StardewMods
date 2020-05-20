using Harmony;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using System;

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
            ObjectPatches.Initialize(helper, Monitor);
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
            // api is never set to anything, as .GetApi can't be used, since JA isn't loaded.
            // Figure out where/when I need to call Initialize() in Entry().
            api = helper.ModRegistry.GetApi<IApi>("spacechase0.JsonAssets");
            Monitor = monitor;
        }

        internal static bool PlacementAction_Prefix(StardewValley.Object __instance, GameLocation location, int x, int y, ref bool __result, Farmer who = null)
        {
            try
            {
                if (api != null)
                {
                    if ((location is MineShaft && Game1.CurrentMineLevel > 120) && __instance.ParentSheetIndex == api.GetBigCraftableId("Dwarven Drill"))
                    {
                        (location as MineShaft).createLadderDown(x, y, true);
                        __result = true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(PlacementAction_Prefix)}:\n{ex}", LogLevel.Error);
                return true;
            }
        }
    }
}
