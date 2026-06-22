using MiniSchematicEditor.Models;
using MiniSchematicEditor.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MiniSchematicEditor.Services
{
    public class JsonService : IJsonService
    {
        private readonly JsonSerializerOptions _options;

        public JsonService()
        {
            _options = new JsonSerializerOptions 
            { 
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };
        }

        public ProjectData? LoadProject(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                string jsonString = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(jsonString))
                    return null;

                return JsonSerializer.Deserialize<ProjectData>(jsonString);
            }
            catch (JsonException)
            {
                return null;
            }
            catch (Exception)
            { 
                return null; 
            }
        }

        public bool SaveProject(string filePath, ProjectData data)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(data, _options);
                File.WriteAllText(filePath, jsonString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
