namespace CMS
{
    public enum Department
    {
        None = 0,
        Pinion,
        Gear
    }

    public enum MachineStatus
    {
        Running = 0,
        Idle,
        DownForMaintenance
    }

    public enum PartKind
    {
        Pinion = 0,
        Ring
    }

    public enum CutterCondition
    {
        NewCutter = 0,
        UsedCutter,
        DevelopmentCutter
    }

    public enum FrequencyCheckResult
    {
        Pass = 0,
        Failed
    }

    public enum CutterChangeInformation
    {
        ChangOver = 0,
        LineOnThe
    }
}
