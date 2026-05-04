namespace Bff.Domain.Models.Persistence;

public record OpportunityResponse(
    int Id, string Symbol, int Score, string Signal, string Reason,
    double? TargetPct, double? StopLossPct, double Price,
    bool Acted, bool IsApproved, string? ValidationReason, DateTime CreatedAt);

public record CreateOpportunityRequest(
    string Symbol, int Score, string Signal, string Reason, double Price,
    double? TargetPct = null, double? StopLossPct = null,
    bool IsApproved = true, string? ValidationReason = null);
