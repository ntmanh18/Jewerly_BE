using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.GemModel
{
    public class GemRequestModel
    {
        public string GemId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int Type { get; set; }

        public long Price { get; set; }

        public string? Desc { get; set; }
        public float? rate { get; set; }
    }
}
