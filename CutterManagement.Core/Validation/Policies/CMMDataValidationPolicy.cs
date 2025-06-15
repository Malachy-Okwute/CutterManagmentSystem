using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.Core
{
    public class CMMDataValidationPolicy : DataValidationBase<CMMDataModel>
    {
        public override ValidationResult Validate(CMMDataModel data)
        {
            ValidationResult result = CreateValidationInstance();

            //if(data.)

            return result;
        }
    }
}
