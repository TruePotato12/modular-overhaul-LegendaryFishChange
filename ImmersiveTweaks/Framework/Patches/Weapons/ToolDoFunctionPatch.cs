﻿namespace DaLion.Stardew.Tweaks.Framework.Patches.Weapons;

#region using directives

using HarmonyLib;
using JetBrains.Annotations;
using StardewValley;
using StardewValley.Tools;

#endregion using directives

[UsedImplicitly]
internal class ToolDoFunctionPatch : BasePatch
{
    /// <summary>Construct an instance.</summary>
    internal ToolDoFunctionPatch()
    {
        Original = RequireMethod<Tool>(nameof(Tool.DoFunction));
    }

    #region harmony patches

    /// <summary>Adds stamina cost to weapon use.</summary>
    [HarmonyPostfix]
    private static void ToolDoFunctionPostfix(Tool __instance, Farmer who)
    {
        if (__instance is not MeleeWeapon weapon || weapon.isScythe() || !ModEntry.Config.WeaponsCostStamina) return;

        var multiplier = weapon.type.Value switch
        {
            MeleeWeapon.dagger => 0.5f,
            MeleeWeapon.club => 2f,
            _ => 1f,
        };

        who.Stamina -= (2 - who.CombatLevel * 0.1f) * multiplier;
    }

    #endregion harmony patches
}