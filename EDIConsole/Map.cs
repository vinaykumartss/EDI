using EDIConsole.Segments;
using EdiEngine.Common.Definitions;
using EdiEngine.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EDIConsole.Maps
{
    public class M_940 : MapLoop
    {
        public M_940() : base(null)
        {
            Content.AddRange(new MapBaseEntity[] {
                new W05() { ReqDes = RequirementDesignator.Mandatory, MaxOccurs = 1 },
            });
        }
    }
}
