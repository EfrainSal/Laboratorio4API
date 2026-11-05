using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Laboratorio4.API.Contracts;

public sealed class RankingRequest
{
    [Required, StringLength(60), JsonRequired] public string PlayerName { get; set; } = string.Empty;
    [Range(0, int.MaxValue), JsonRequired] public int Score { get; set; }
    [Range(0, float.MaxValue), JsonRequired] public float Time { get; set; }
    [Range(1, int.MaxValue), JsonRequired] public int CharacterId { get; set; }
    [Required, StringLength(64), JsonRequired] public string Date { get; set; } = string.Empty;
    [JsonRequired] public bool Uploaded { get; set; }
}
