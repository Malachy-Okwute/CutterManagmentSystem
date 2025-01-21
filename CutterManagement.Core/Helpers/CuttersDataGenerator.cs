using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.Core
{
    public static class CuttersDataGenerator
    {
        public static List<CutterDataModel> GenerateDummyCutters(Department department, PartKind kind, int amount)
        {
            List<CutterDataModel> cutterItems = new List<CutterDataModel>();

            for (int i = 1; i < amount; i++)
            {
                cutterItems.Add(new CutterDataModel
                {
                    CutterNumber = $"P0000{i}-12345",
                    Owner = department,
                    Model = "500",
                    Kind = kind,
                    Condition = CutterCondition.NewCutter,
                    Count = 0,
                    LastUsedDate = DateTime.Now,
                    DateCreated = DateTime.Now,
                });
            }

            return cutterItems;
        }

    }
}
