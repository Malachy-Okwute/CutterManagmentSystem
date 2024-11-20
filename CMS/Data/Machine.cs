namespace CMS
{
    public class Machine
    {
        public string UniqueID { get; set; } = string.Empty;
        public string UniqueSetID { get; set; } = string.Empty;
        public string Count { get; set; } = string.Empty;
        public Department MachineOwner { get; set; }
        public MachineStatus Status { get; set; }
        public FrequencyCheckResult FrequencyCheckResult { get; set; }
        public Part RunningPart { get; set; }
        public Cutter Cutter { get; set; }

        public Machine(Cutter cutter, Part runningPart)
        {
            Cutter = cutter;
            RunningPart = runningPart;
        }
    }
}
