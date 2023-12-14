﻿namespace DaLion.Overhaul.Modules.Professions.Patchers.Fishing;

#region using directives

using DaLion.Overhaul.Modules.Professions.Extensions;
using DaLion.Shared.Extensions.Stardew;
using DaLion.Shared.Harmony;
using HarmonyLib;
using StardewValley.Buildings;
using StardewValley.GameData.FishPond;

#endregion using directives

[UsedImplicitly]
internal sealed class FishPondUpdateMaximumOccupancyPatcher : HarmonyPatcher
{
    /// <summary>Initializes a new instance of the <see cref="FishPondUpdateMaximumOccupancyPatcher"/> class.</summary>
    internal FishPondUpdateMaximumOccupancyPatcher()
    {
        this.Target = this.RequireMethod<FishPond>(nameof(FishPond.UpdateMaximumOccupancy));
        this.Postfix!.before = new[] { OverhaulModule.Ponds.Name };
    }

    #region harmony patches

    /// <summary>Patch for Aquarist increased max fish pond capacity.</summary>
    [HarmonyPostfix]
    [HarmonyBefore("DaLion.Overhaul.Modules.Ponds")]
    private static void FishPondUpdateMaximumOccupancyPostfix(
        FishPond __instance, FishPondData? ____fishPondData)
    {
        if (____fishPondData is null || !__instance.HasUnlockedFinalPopulationGate())
        {
            return;
        }

        var owner = __instance.GetOwner();
        if (!owner.HasProfessionOrLax(VanillaProfession.Aquarist))
        {
            return;
        }

        var occupancy = __instance.maxOccupants.Value + 2;
        if (owner.HasProfessionOrLax(VanillaProfession.Aquarist, true))
        {
            occupancy += 2;
        }

        if (__instance.HasLegendaryFish())
        {
            occupancy /= 2;
        }

        __instance.maxOccupants.Set(occupancy);
    }

    #endregion harmony patches
}
