using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSchematicEditor.Models
{
    /// <summary>
    /// Класс блока
    /// </summary>
    public class BlockData
    {
        public string UId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsActive { get; set; }
    }
}
