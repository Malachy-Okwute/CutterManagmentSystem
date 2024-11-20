namespace CMS
{
    public class Part
    {
        public string UniqueID { get; set; } = string.Empty;
        public string PartRatio { get; set; } = string.Empty;
        public PartModel Model { get; set; }
        public PartKind Kind { get; set; }

        public Part(PartModel model)
        {
            Model = model;
        }
    }
}
