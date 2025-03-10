using System.ComponentModel;

namespace CutterManagement.Core
{
    public enum Department
    {
        None = 0, Pinion, Ring
    }

    public enum MachineStatus
    {
        [Description("Select machine status")]
        None = 0, 
        [Description("Is running")]
        IsRunning,
        [Description("Warning")]
        Warning, 
        [Description("Is down for maintenance")]
        IsDownForMaintenance
    }

    public enum PartKind
    {
        [Description("Select type of part")]
        None = 0,

        [Description("Pinion")]
        Pinion,
        
        [Description("Ring")]
        Ring
    }

    public enum CutterCondition
    {
        New = 0, Used, Development
    }

    public enum FrequencyCheckResult
    {
        Passed = 0, Failed, Setup
    }

    public enum CutterChangeInformation
    {
        None = 0, Unknown, ChangOver, LineOnTheDrive, BrokenBlades
    }

    public enum UserRole 
    { 
        Basic, Admin, Dev 
    }

    public enum UserShift 
    {
        [Description("Select user shift")]
        None = 0,

        [Description("1st Shift")]
        First,

        [Description("2nd Shift")]
        Second,

        [Description("3rd Shift")]
        Third
    }
}
