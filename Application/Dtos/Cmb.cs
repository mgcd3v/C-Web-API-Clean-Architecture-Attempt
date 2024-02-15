using Application.Dtos.Common;

namespace Application.Dtos
{
    public class CmbRequest : Audit
    {
        public int Id { get; set; } = default;
        public string Field { get; set; } = string.Empty;
    }
    public class ReadCmbRequest : Page
    {
        public string? Field { get; set; } = null;
    }
    public class CreateCmbRequest : CmbRequest { }
    public class UpdateCmbRequest : CmbRequest { }

    public class CmbResponse
    {
        public int Id { get; set; } = default;
        public string Field { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool Active { get; set; } = default;
    }
    public class CreateCmbResponse : CmbResponse { }
    public class ReadCreateCmbResponse : CmbResponse { }
    public class UpdateCmbResponse : CmbResponse { }
    public class DeleteCmbResponse : CmbResponse { }
}
