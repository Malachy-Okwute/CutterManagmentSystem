namespace CutterManagement.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class MachineUserInteractions : DataModelBase
    {
        public int? MachineDataModelId { get; set; }
        public MachineDataModel MachineDataModel { get; set; }
        public int? UserDataModelId { get; set; }
        public UserDataModel UserDataModel { get; set; }
    }
}
