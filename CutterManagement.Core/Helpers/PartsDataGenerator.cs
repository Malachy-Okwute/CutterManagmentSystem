namespace CutterManagement.Core
{
    public static class PartsDataGenerator
    {
        public static List<PartDataModel> GenerateDummyParts(Department department, PartKind kind, int amount)
        {
            List<PartDataModel> partItems = new List<PartDataModel>();

            for (int i = 1; i < amount; i++)
            {
                partItems.Add(new PartDataModel
                {
                    PartNumber = "123456789",
                    Model = "500",
                    PartToothCount = "11",
                    Kind = kind,
                    DateCreated = DateTime.Now,
                });
            }

            return partItems;
        }

    }
}
