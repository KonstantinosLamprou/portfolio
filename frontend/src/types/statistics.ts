
export interface StatisticsResponse {
    totalViews: number;
    totalLikes: number;
}

export interface UpdateStatisticsRequest {
    viewToAdd: number;
    likeToAdd: number;
}