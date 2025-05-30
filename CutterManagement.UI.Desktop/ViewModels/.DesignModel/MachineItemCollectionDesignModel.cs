using CutterManagement.Core;
using CutterManagement.DataAccess;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Design model for <see cref="MachineItemCollectionControl"/>
    /// </summary>
    public class MachineItemCollectionDesignModel : MachineItemCollectionViewModel
    {
        /// <summary>
        /// A singleton instance of this design-model
        /// </summary>
        public static readonly MachineItemCollectionDesignModel Instance = new(null!);

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemCollectionDesignModel(IMachineService machineService) : base(machineService) { } 
    }
}
