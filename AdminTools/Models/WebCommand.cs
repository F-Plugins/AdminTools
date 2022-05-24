namespace AdminTools.Models
{
    public class WebCommand
    {
        public string? CommandName { get; set; }
        public string? RequestDescription { get; set; }
        public string? RequestURL { get; set; }
        public bool Enabled { get; set; }
    }
}
