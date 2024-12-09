namespace CutterManagement.Core
{
    public enum Department
    {
        None = 0, Pinion, Gear
    }

    public enum MachineStatus
    {
        IsRunning = 0, IsIdle, IsDownForMaintenance
    }

    public enum PartKind
    {
        Pinion = 0, Ring
    }

    public enum CutterCondition
    {
        NewCutter = 0, UsedCutter, DevelopmentCutter
    }

    public enum FrequencyCheckResult
    {
        PASSED = 0, FAILED, SETUP
    }

    public enum CutterChangeInformation
    {
        ChangOver = 0, LineOnTheDrive, BrokenBlades
    }
}
