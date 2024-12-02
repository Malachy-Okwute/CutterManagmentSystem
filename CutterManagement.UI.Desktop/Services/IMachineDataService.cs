
namespace CutterManagement.UI.Desktop
{
    public interface IMachineDataService
    {
        string AddMachine(MachineDataModel machine);
        string AddPart(PartDataModel part);
        HashSet<MachineDataModel> GetMachines();
        HashSet<PartDataModel> GetParts();
        string RemoveMachine(MachineDataModel machine);
        string RemovePart(PartDataModel part);
        string UpdateMachineInfo(MachineDataModel machine);
        string UpdatePartInfo(PartDataModel part);
    }
}