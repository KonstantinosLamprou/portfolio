
namespace Backend.Domain.Entities; 

public class Statistics
{
    // Statistiken des Portfolios, z.B. Gesamtansichten, Likes, etc.
    public int Id { get; set; }
    public int TotalViews { get; set; }
    public int TotalLikes { get; set; }
}