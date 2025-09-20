namespace CutterManagement.Core
{
    /// <summary>
    /// User data model archive DTO
    /// </summary>
    /// <param name="FirstName"> User first name  </param>
    /// <param name="LastName"> User last name  </param>
    /// <param name="Shift"> User shift </param>
    /// <param name="IsActive"> True if user is an active user </param>
    /// <param name="IsArchived"> True if this user is no longer accessible / currently archived </param>
    public record UserDataArchiveDTO(string FirstName, string LastName, UserShift Shift, bool IsActive, bool IsArchived)
    {
        /// <summary>
        /// Machine and users navigation property collection
        /// </summary>
        public ICollection<MachineUserInteractions> MachineUserInteractions { get; set; } = new List<MachineUserInteractions>();
    }
}
