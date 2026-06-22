using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSchematicEditor.Services.Interfaces
{
    public interface IFileService
    {
        string? SaveFileDialog(string fileName, string filter = "JSON файлы (*.json)|*.json");
        string? OpenFileDialog(string filter = "JSON файлы (*.json)|*.json");
    }
}
