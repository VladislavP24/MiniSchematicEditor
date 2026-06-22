using MiniSchematicEditor.Models;
using MiniSchematicEditor.Services.Interfaces;
using MiniSchematicEditor.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSchematicEditor.Tests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IJsonService> _jsonServiceMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IMessageService> _messageServiceMock;

        public MainWindowViewModelTests()
        {
            _jsonServiceMock = new Mock<IJsonService>();
            _fileServiceMock = new Mock<IFileService>();
            _messageServiceMock = new Mock<IMessageService>();
        }

        private MainWindowViewModel CreateViewModel()
        {
            var viewModel = new MainWindowViewModel();
            var bindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

            typeof(MainWindowViewModel).GetField("_jsonService", bindingFlags)?.SetValue(viewModel, _jsonServiceMock.Object);
            typeof(MainWindowViewModel).GetField("_fileService", bindingFlags)?.SetValue(viewModel, _fileServiceMock.Object);
            typeof(MainWindowViewModel).GetField("_messageService", bindingFlags)?.SetValue(viewModel, _messageServiceMock.Object);

            return viewModel;
        }

        [Fact]
        public void SaveProject_ShouldBlockSaving_WhenAnyBlockHasErrors()
        {
            // Arrange
            var vm = CreateViewModel();
            var bindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
            typeof(MainWindowViewModel).GetField("isProject", bindingFlags)?.SetValue(vm, true);

            var invalidBlock = new BlockModel { Name = "" };
            vm.Blocks.Add(invalidBlock);

            typeof(BlockModel).GetMethod("ValidateAllProperties", bindingFlags)?.Invoke(invalidBlock, null);

            // Act
            vm.SaveProject.Execute(null);

            // Assert
            _messageServiceMock.Verify(m => m.ShowError(It.IsAny<string>(), "Ошибка валидации"), Times.Once);
            _fileServiceMock.Verify(f => f.SaveFileDialog(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void OpenProject_ShouldShowErrorAndNotClearCanvas_WhenFileIsCorrupted()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.ProjectName = "Project 1";

            _fileServiceMock.Setup(f => f.OpenFileDialog(It.IsAny<string>())).Returns("C:\\corrupted.json");
            _jsonServiceMock.Setup(j => j.LoadProject(It.IsAny<string>())).Returns((ProjectData)null);

            // Act
            vm.OpenProject.Execute(null);

            // Assert
            _messageServiceMock.Verify(m => m.ShowError(It.IsAny<string>(), "Ошибка загрузки"), Times.Once);
            Assert.Equal("Project 1", vm.ProjectName);
        }

        [Fact]
        public void RemoveBlock_ShouldDeleteSelectedBlockAndClearSelection()
        {
            // Arrange
            var vm = CreateViewModel();
            var bindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
            typeof(MainWindowViewModel).GetField("isProject", bindingFlags)?.SetValue(vm, true);

            var block = new BlockModel { Name = "BlockToDelete" };
            vm.Blocks.Add(block);
            vm.SelectedBlock = block;

            // Act
            vm.RemoveBlock.Execute(null);

            // Assert
            Assert.Empty(vm.Blocks);
            Assert.Null(vm.SelectedBlock);
        }

        [Fact]
        public void OpenProject_ShouldPopulateBlocks_WhenFileIsValid()
        {
            // Arrange
            var vm = CreateViewModel();
            _fileServiceMock.Setup(f => f.OpenFileDialog(It.IsAny<string>())).Returns("C:\\valid.json");

            var mockData = new ProjectData
            {
                ProjectName = "LoadedProject",
                Blocks = new List<BlockData> { new BlockData { Name = "Block1", Type = "Square", X = 10, Y = 20 } }
            };
            _jsonServiceMock.Setup(j => j.LoadProject(It.IsAny<string>())).Returns(mockData);

            // Act
            vm.OpenProject.Execute(null);

            // Assert
            Assert.Equal("LoadedProject", vm.ProjectName);
            Assert.Single(vm.Blocks);
            Assert.Equal("Block1", vm.Blocks[0].Name);
            Assert.NotNull(vm.Blocks[0].IsNameUniqueChecker);
        }

        [Fact]
        public void OpenProject_ShouldDoNothing_WhenUserCancelsFileDialog()
        {
            // Arrange
            var vm = CreateViewModel();
            vm.ProjectName = "Project 2";
            _fileServiceMock.Setup(f => f.OpenFileDialog(It.IsAny<string>())).Returns((string)null!);

            // Act
            vm.OpenProject.Execute(null);

            // Assert
            Assert.Equal("Project 2", vm.ProjectName);
            _jsonServiceMock.Verify(j => j.LoadProject(It.IsAny<string>()), Times.Never);
        }
    }
}
