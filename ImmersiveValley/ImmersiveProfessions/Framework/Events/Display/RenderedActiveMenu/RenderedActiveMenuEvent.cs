﻿namespace DaLion.Stardew.Professions.Framework.Events.Display;

#region using directives

using StardewModdingAPI.Events;

#endregion using directives

/// <summary>Wrapper for <see cref="IDisplayEvents.RenderedActiveMenu"/> allowing dynamic enabling / disabling.</summary>
internal abstract class RenderedActiveMenuEvent : BaseEvent
{
    /// <summary>
    ///     When a menu is open, raised after that menu is drawn to the sprite batch but before it's rendered to the
    ///     screen.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    public void OnRenderedActiveMenu(object sender, RenderedActiveMenuEventArgs e)
    {
        if (enabled.Value) OnRenderedActiveMenuImpl(sender, e);
    }

    /// <inheritdoc cref="OnRenderedActiveMenu" />
    protected abstract void OnRenderedActiveMenuImpl(object sender, RenderedActiveMenuEventArgs e);
}