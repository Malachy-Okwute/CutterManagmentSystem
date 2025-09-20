namespace CutterManagement.Core
{
    /// <summary>
    /// DTO Join table between machine and users 
    /// </summary>
    /// <param name="MachineDataModelId"> Machine foreign key </param>
    /// <param name="MachineDataModel"> Associated machine object </param>
    /// <param name="UserDataModelId"> User foreign key </param>
    /// <param name="UserDataModel"> Associated user object </param>
    public record MachineUserInteractionsDTO(int? MachineDataModelId, MachineDataModel MachineDataModel, int? UserDataModelId, UserDataModel UserDataModel)
    {
        /// <summary>
        /// Date and time of last entry
        /// </summary>
        public DateTime LastEntryDateTime { get; set; } = DateTime.Now;
    }
}
