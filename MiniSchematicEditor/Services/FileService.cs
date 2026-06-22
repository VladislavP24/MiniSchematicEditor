using Microsoft.Win32;
using MiniSchematicEditor.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSchematicEditor.Services
{
    public class FileService : IFileService
    {
        public string? OpenFileDialog(string filter = "JSON файлы (*.json)|*.json")
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = filter
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public string? SaveFileDialog(string fileName, string filter = "JSON файлы (*.json)|*.json")
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = filter,
                FileName = fileName
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}
