using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.GemModel
{
    public class CreateGemModel
    {
        public float rate { get; set; }
        public string Name { get; set; } = null!;

        public int Type { get; set; }

        public long Price { get; set; }

        public string? Desc { get; set; }
    }
}
