using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSchematicEditor.Models
{
    /// <summary>
    /// Класс проекта
    /// </summary>
    public class ProjectData
    {
        public string ProjectName { get; set; } = "Новый проект";
        public List<BlockData> Blocks { get; set; } = new List<BlockData>();
    }
}
