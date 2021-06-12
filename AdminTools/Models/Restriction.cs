using System.Collections.Generic;

namespace AdminTools.Models
{
    public class Restriction
    {
        public List<ushort>? Ids { get; set; }
        public string? BypassPermission { get; set; }
    }
}
