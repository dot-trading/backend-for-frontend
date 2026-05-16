namespace TradingProject.Bff.Client.Models.Enums;

/// <summary>
/// Discriminates the type of notification published by the orchestrator (or any producer)
/// and consumed by the telegram-bot via the BFF.
///
/// Defined with <see cref="System.FlagsAttribute"/> so that a single message can advertise
/// multiple concerns and the consumer can build a composite message.
/// </summary>
[Flags]
public enum NotificationType
{
    /// <summary>No notification type (default / unset).</summary>
    None = 0,

    /// <summary>A service has started (e.g. OrchestratorServiceWorker).</summary>
    ServiceStart = 1,

    /// <summary>Market stress alert triggered by the orchestrator.</summary>
    MarketStress = 2,

    // ── Reserved for future types ────────────────────────────────────
    // Report      = 4,
    // RawAlert    = 8,
    // PositionExit = 16,
}
