namespace CutterManagement.UI.Desktop
{
    public interface IDataService
    {
        void LoadDataFromDB();
        void LoadDataFromExcelStyleSheet<T>(string path);
    }
}
