using CutterManagement.Core;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="PartItemControl"/>
    /// </summary>
    public class PartItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Id number of this part item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique part number
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Part summary number
        /// </summary>
        public string SummaryNumber { get; set; }

        /// <summary>
        /// Part model number
        /// </summary>
        public string ModelNumber { get; set; }

        /// <summary>
        /// Number of teeth on part
        /// </summary>
        public string ToothCount { get; set; }

        /// <summary>
        /// True if part item is in edit mode
        /// </summary>
        public bool IsEditMode { get; set; }

        /// <summary>
        /// The kind of part (Gear / Pinion)
        /// </summary>
        public PartKind Kind { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler PartItemSelectedEvent;

        /// <summary>
        /// Command to enter into part edit mode
        /// </summary>
        public ICommand EnterEditModeCommand { get; set; }

        /// <summary>
        /// Command to reset edit mode
        /// </summary>
        public ICommand ResetEditModeCommand { get; set; }

        public PartItemViewModel()
        {
            EnterEditModeCommand = new RelayCommand(EnterEditMode);
            ResetEditModeCommand = new RelayCommand(OnPartItemSelected);
        }

        private void EnterEditMode()
        {
            OnPartItemSelected();

            IsEditMode = true;
        }

        private void OnPartItemSelected()
        {
            PartItemSelectedEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
