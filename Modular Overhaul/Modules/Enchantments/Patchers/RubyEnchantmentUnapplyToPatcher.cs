﻿namespace DaLion.Overhaul.Modules.Enchantments.Patchers;

#region using directives

using System.Collections.Generic;
using DaLion.Overhaul.Modules.Slingshots.VirtualProperties;
using DaLion.Overhaul.Modules.Weapons;
using DaLion.Overhaul.Modules.Weapons.VirtualProperties;
using DaLion.Shared.Extensions;
using DaLion.Shared.Extensions.Stardew;
using DaLion.Shared.Harmony;
using HarmonyLib;
using StardewValley.Tools;

#endregion using directives

[UsedImplicitly]
internal sealed class RubyEnchantmentUnapplyToPatcher : HarmonyPatcher
{
    /// <summary>Initializes a new instance of the <see cref="RubyEnchantmentUnapplyToPatcher"/> class.</summary>
    internal RubyEnchantmentUnapplyToPatcher()
    {
        this.Target = this.RequireMethod<RubyEnchantment>("_UnapplyTo");
    }

    #region harmony patches

    /// <summary>Adjust Ruby enchant for randomized weapons.</summary>
    [HarmonyPrefix]
    private static bool RubyEnchantmentUnapplyToPrefix(RubyEnchantment __instance, Item item)
    {
        if (item is not MeleeWeapon weapon || !EnchantmentsModule.Config.RebalancedForges)
        {
            return true; // run original logic
        }

        var data = ModHelper.GameContent
            .Load<Dictionary<int, string>>("Data/weapons")[weapon.InitialParentTileIndex]
            .SplitWithoutAllocation('/');
        weapon.minDamage.Value -=
            (int)(weapon.Read(DataKeys.BaseMinDamage, int.Parse(data[2])) * __instance.GetLevel() * 0.1f);
        weapon.maxDamage.Value -=
            (int)(weapon.Read(DataKeys.BaseMaxDamage, int.Parse(data[3])) * __instance.GetLevel() * 0.1f);
        return false; // don't run original logic
    }

    /// <summary>Reset cached stats.</summary>
    [HarmonyPostfix]
    private static void RubyEnchantmentUnapplyPostfix(Item item)
    {
        switch (item)
        {
            case MeleeWeapon weapon when WeaponsModule.ShouldEnable:
                weapon.Invalidate();
                break;
            case Slingshot slingshot when SlingshotsModule.ShouldEnable:
                slingshot.Invalidate();
                break;
        }
    }

    #endregion harmony patches
}