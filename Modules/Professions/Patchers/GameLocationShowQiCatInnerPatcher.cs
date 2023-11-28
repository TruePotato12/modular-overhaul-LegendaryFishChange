﻿namespace DaLion.Overhaul.Modules.Professions.Patchers;

#region using directives

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DaLion.Overhaul.Modules.Professions.Extensions;
using DaLion.Shared.Extensions.Reflection;
using DaLion.Shared.Harmony;
using HarmonyLib;

#endregion using directives

[UsedImplicitly]
internal sealed class GameLocationShowQiCatInnerPatcher : HarmonyPatcher
{
    /// <summary>Initializes a new instance of the <see cref="GameLocationShowQiCatInnerPatcher"/> class.</summary>
    internal GameLocationShowQiCatInnerPatcher()
    {
        this.Target = typeof(GameLocation).GetInnerMethodsContaining("<ShowQiCat>b__303_3").SingleOrDefault();
    }

    #region harmony patches

    /// <summary>Patch to display new perfection requirement.</summary>
    [HarmonyPrefix]
    // ReSharper disable once UnusedParameter.Local
    private static bool GameLocationShowQiCatInnerPrefix(ref float __result)
    {
        if (!ProfessionsModule.Config.EnablePrestige || !ProfessionsModule.Config.ExtendedPerfectionRequirement)
        {
            return true; // run original logic
        }

        if (ProfessionsModule.Config.EnableExtendedProgression)
        {
            // ReSharper disable once RedundantAssignment
            __result = Math.Min(
                Skill.ListVanilla
                    .Where(skill => skill.CurrentLevel >= 20)
                    .Sum(_ => 1f),
                5f);
        }
        else
        {
            // ReSharper disable once RedundantAssignment
            __result += Math.Min(
                Skill.ListVanilla
                    .Where(skill => Game1.player.HasAllProfessionsInSkill(skill))
                    .Sum(_ => 1f),
                5f);
        }

        return false; // don't run original logic
    }

    #endregion harmony patches
}
