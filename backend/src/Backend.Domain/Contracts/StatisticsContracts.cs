namespace Backend.Domain.Contracts;

public record StatisticsResponse(
    int TotalViews,
    int TotalLikes
);
public record UpdateStatisticsRequest(
    int? ViewToAdd,
    int? LikeToAdd
);