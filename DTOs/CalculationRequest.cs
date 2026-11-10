using System.ComponentModel.DataAnnotations;

namespace ChannelApp.DTOs;

public record CalculationRequest(
    [Required] int Number1,
    [Required] int Number2
);