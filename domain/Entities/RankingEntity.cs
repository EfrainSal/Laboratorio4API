using System.ComponentModel.DataAnnotations;

namespace domain.Entities;

public sealed class RankingEntity
{
    public int Id { get; set; }
    [MaxLength(60)] public string PlayerName { get; set; } = string.Empty;
    public int Score { get; set; }
    public float Time { get; set; }
    public int CharacterId { get; set; }
    [MaxLength(64)] public string Date { get; set; } = string.Empty;
    public bool Uploaded { get; set; }
}
