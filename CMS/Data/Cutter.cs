namespace CMS
{
    public class Cutter
    {
        public string UniqueID { get; set; } = string.Empty;
        public PartModel Model { get; set; } 
        public PartKind Kind { get; set; }
        public Department Department { get; set; }
        public CutterCondition Condition { get; set; }


        public Cutter(PartModel model)
        {
            Model = model;
        }
    }
}
