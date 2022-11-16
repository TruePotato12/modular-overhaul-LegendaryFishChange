﻿namespace DaLion.Ligo.Modules.Professions.Patches.Farming;

#region using directives

using System.Reflection;
using DaLion.Ligo.Modules.Professions.Extensions;
using DaLion.Shared.Extensions.Stardew;
using HarmonyLib;
using Shared.Harmony;

#endregion using directives

[UsedImplicitly]
internal sealed class FarmAnimalGetSellPricePatcher : HarmonyPatcher
{
    /// <summary>Initializes a new instance of the <see cref="FarmAnimalGetSellPricePatcher"/> class.</summary>
    internal FarmAnimalGetSellPricePatcher()
    {
        this.Target = this.RequireMethod<FarmAnimal>(nameof(FarmAnimal.getSellPrice));
    }

    #region harmony patches

    /// <summary>Patch to adjust Breeder animal sell price.</summary>
    [HarmonyPrefix]
    private static bool FarmAnimalGetSellPricePrefix(FarmAnimal __instance, ref int __result)
    {
        double adjustedFriendship;
        try
        {
            if (!__instance.GetOwner().HasProfession(Profession.Breeder))
            {
                return true; // run original logic
            }

            adjustedFriendship = __instance.GetProducerAdjustedFriendship();
        }
        catch (Exception ex)
        {
            Log.E($"Failed in {MethodBase.GetCurrentMethod()?.Name}:\n{ex}");
            return true; // default to original logic
        }

        __result = (int)(__instance.price.Value * adjustedFriendship);
        return false; // don't run original logic
    }

    #endregion harmony patches
}