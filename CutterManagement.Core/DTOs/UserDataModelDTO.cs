namespace CutterManagement.Core
{
    /// <summary>
    /// A basic user data model DTO
    /// </summary>
    /// <param name="FirstName"> User first name  </param>
    /// <param name="LastName"> User last name  </param>
    /// <param name="Shift"> User shift </param>
    /// <param name="IsArchived"> True if this user is no longer accessible / currently archived </param>
    public record UserDataModelDTO(string FirstName, string LastName, UserShift Shift, bool IsArchived) : IMessage
    {
        /// <summary>
        /// True if user is an active user
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Machine and users navigation property collection
        /// </summary>
        public ICollection<MachineUserInteractions> MachineUserInteractions { get; set; } = new List<MachineUserInteractions>();

        /// <summary>
        /// Information updates associated with this user
        /// </summary>
        public ICollection<InfoUpdateDataModel> InfoUpdateDataModel { get; set; } = new List<InfoUpdateDataModel>();
    }
}
