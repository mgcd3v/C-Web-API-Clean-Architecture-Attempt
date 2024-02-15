using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    [Keyless]
    public class Cmb
    {
        public int? Id { get; set; } = default;
        public string? Field { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;
        public string? Value { get; set; } = string.Empty;
        public bool? Active { get; set; } = default;
    }

    public class ExtendedCmb : Cmb
    {
    }
}
