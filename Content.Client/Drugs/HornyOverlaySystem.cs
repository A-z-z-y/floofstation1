using Content.Shared.Drugs;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Player;

namespace Content.Client.Drugs;

/// <summary>
///     System to handle aphrodisiac related overlays.
/// </summary>
public sealed class HornyOverlaySystem : EntitySystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IOverlayManager _overlayMan = default!;

    private HornyOverlay _overlay = default!;

    public static string HornyKey = "Horny";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HornyComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<HornyComponent, ComponentShutdown>(OnShutdown);

        SubscribeLocalEvent<HornyComponent, LocalPlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<HornyComponent, LocalPlayerDetachedEvent>(OnPlayerDetached);

        _overlay = new();
    }

    private void OnPlayerAttached(EntityUid uid, HornyComponent component, LocalPlayerAttachedEvent args)
    {
        _overlayMan.AddOverlay(_overlay);
    }

    private void OnPlayerDetached(EntityUid uid, HornyComponent component, LocalPlayerDetachedEvent args)
    {
        _overlay.Intoxication = 0;
        _overlay.TimeTicker = 0;
        _overlayMan.RemoveOverlay(_overlay);
    }

    private void OnInit(EntityUid uid, HornyComponent component, ComponentInit args)
    {
        if (_player.LocalEntity == uid)
            _overlayMan.AddOverlay(_overlay);
    }

    private void OnShutdown(EntityUid uid, HornyComponent component, ComponentShutdown args)
    {
        if (_player.LocalEntity == uid)
        {
            _overlay.Intoxication = 0;
            _overlay.TimeTicker = 0;
            _overlayMan.RemoveOverlay(_overlay);
        }
    }
}
