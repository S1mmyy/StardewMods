using Harmony;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using System;
using StardewModdingAPI.Events;

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
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            ObjectPatches.Apply(this.ModManifest.UniqueID);
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            ObjectPatches.Initialize(Helper, Monitor);
        }
    }

    internal class ObjectPatches
    {
        private static IApi api;
        private static IMonitor Monitor;
        private static int MyCraftableID = -1;

        public static void Apply(string modId)
        {
            var harmony = HarmonyInstance.Create(modId);

            harmony.Patch(
            original: AccessTools.Method(typeof(StardewValley.Object), nameof(StardewValley.Object.placementAction)),
            prefix: new HarmonyMethod(typeof(ObjectPatches), nameof(ObjectPatches.PlacementAction_Prefix))
            );
        }

        public static void Initialize(IModHelper helper, IMonitor monitor)
        {
            api = helper.ModRegistry.GetApi<IApi>("spacechase0.JsonAssets");
            Monitor = monitor;

            if (api != null)
            {
                MyCraftableID = api.GetBigCraftableId("Dwarven Drill");
            }
        }

        internal static bool PlacementAction_Prefix(StardewValley.Object __instance, GameLocation location, int x, int y, ref bool __result, Farmer who = null)
        {
            try
            {
                if ((__instance.bigCraftable.Value && __instance.ParentSheetIndex == MyCraftableID) && (location is MineShaft shaft && Game1.CurrentMineLevel > 120))
                {
                    shaft.createLadderDown(x, y, true);
                    __result = true;
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
