using MiniSchematicEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSchematicEditor.Services.Interfaces
{
    public interface IJsonService
    {
        bool SaveProject(string filePath, ProjectData data);
        ProjectData? LoadProject(string filePath);
    }
}
