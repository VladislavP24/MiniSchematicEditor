using MiniSchematicEditor.Models;
using MiniSchematicEditor.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSchematicEditor.Tests
{
    public class JsonServiceTests : IDisposable
    {
        private readonly string _tempFilePath;
        private readonly JsonService _jsonService;

        public JsonServiceTests()
        {
            _tempFilePath = Path.GetTempFileName();
            _jsonService = new JsonService();
        }

        [Fact]
        public void SaveAndLoad_ShouldPreserveCyrillicAndData()
        {
            // Arrange
            var originalData = new ProjectData
            {
                ProjectName = "Тестовый Проект",
                Blocks = new List<BlockData>
                {
                    new BlockData { Name = "Block 1", Type = "Square", X = 50, Y = 50 }
                }
            };

            // Act
            bool saveResult = _jsonService.SaveProject(_tempFilePath, originalData);
            var loadedData = _jsonService.LoadProject(_tempFilePath);

            // Assert
            Assert.True(saveResult);
            Assert.NotNull(loadedData);
            Assert.Equal("Тестовый Проект", loadedData.ProjectName);
            Assert.Equal("Block 1", loadedData.Blocks[0].Name);
        }

        [Fact]
        public void LoadProject_ShouldReturnNull_WhenJsonIsCorrupted()
        {
            // Arrange
            File.WriteAllText(_tempFilePath, "{ invalid json }");

            // Act
            var result = _jsonService.LoadProject(_tempFilePath);

            // Assert
            Assert.Null(result);
        }

        public void Dispose()
        {
            if (File.Exists(_tempFilePath))
                File.Delete(_tempFilePath); // Удаляем временный файл после теста
        }
    }
}
