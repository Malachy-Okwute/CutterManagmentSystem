namespace CutterManagement.Core
{
    /// <summary>
    /// Part data model DTO
    /// </summary>
    /// <param name="PartNumber"> Unique part number </param>
    /// <param name="PartToothCount"> The number of teeth this part has </param>
    /// <param name="Model"> The model of this part </param>
    /// <param name="SummaryNumber"> The summary this part is associated to </param>
    /// <param name="Kind"> The type of this part
    /// Ring or pinion </param>
    public record PartDataModelDTO(string PartNumber, string PartToothCount, string Model, string SummaryNumber, PartKind Kind) : IMessage;
}
