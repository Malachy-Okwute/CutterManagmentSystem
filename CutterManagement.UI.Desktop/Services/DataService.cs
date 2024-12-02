using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Serilog;
using System.CodeDom;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Windows.Shapes;

namespace CutterManagement.UI.Desktop
{
    public class DataService
    {
        private IMachineDataService _machineDataService;

        public DataService(IMachineDataService machineDataService)
        {
            _machineDataService = machineDataService;
            LoadDataFromExcelStyleSheet<MachineDataModel>("TEST.xlsx");
        }

        public void LoadDataFromExcelStyleSheet<T>(string path)
        {
            if (File.Exists(path))
            {
                // https://learn.microsoft.com/en-us/office/open-xml/spreadsheet/how-to-parse-and-read-a-large-spreadsheet?tabs=cs-2%2Ccs-3%2Ccs-4%2Ccs
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(path, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart ?? spreadsheetDocument.AddWorkbookPart();
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                    object? requestedDataType = CreateInstance<T>();

                    switch (requestedDataType)
                    {
                        case MachineDataModel:
                            MapMachineInfo((MachineDataModel)requestedDataType, sheetData);
                            break;

                        case PartDataModel:
                            break;
                    }
                }
            }
            else
            {
                Log.Logger.Warning($"{path} not found");
            }
        }

        public void MapMachineInfo(MachineDataModel machine, SheetData sheetData)
        {
            foreach (Row row in sheetData.Elements<Row>())
            {
                var test = int.Parse(row.RowIndex?.InnerText!);

                if (test <= 2)
                    continue;

                foreach (Cell column in row.Elements<Cell>())
                {
                    switch (column.CellReference?.InnerText)
                    {
                        case "A3":
                            machine.Id = column.InnerText;
                            machine.SetID = column.InnerText;
                            break;
                        case "B3":
                            machine.Count = column.InnerText;
                            break;
                        case "C3":
                            machine.Owner = column.InnerText.ToUpper() == "GEAR" ? Department.Gear : Department.Pinion;
                            break;
                    }
                }
            }
            machine.FrequencyCheckResult = FrequencyCheckResult.SETUP;
            machine.DateTime = DateTime.UtcNow;
            machine.Status = MachineStatus.IsIdle;
            _machineDataService.AddMachine(machine);
        }


        public T CreateInstance<T>()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
