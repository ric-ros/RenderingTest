namespace RenderingTest.Client.Models
{
    public class QuoteBatchModel
    {
        public string Reference { get; set; } = string.Empty;

        public ICollection<DesignerSectionModel> Charges { get; set; } = new List<DesignerSectionModel>();

        public ICollection<DesignerSectionModel> Routes { get; set; } = new List<DesignerSectionModel>();
    }
}
