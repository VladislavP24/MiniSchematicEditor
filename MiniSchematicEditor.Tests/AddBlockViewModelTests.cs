using MiniSchematicEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSchematicEditor.Tests
{
    public class AddBlockViewModelTests
    {
        [Fact]
        public void OkCommand_ShouldNotCloseWindow_WhenFormHasErrors()
        {
            // Arrange
            var existingNames = new List<string> { "Block1" };
            var viewModel = new AddBlockViewModel(existingNames);

            bool closeActionCalled = false;
            viewModel.CloseAction = (result) => closeActionCalled = true;

            // Act
            viewModel.Name = "";
            viewModel.Ok.Execute(null);

            // Assert
            Assert.True(viewModel.HasErrors);
            Assert.False(closeActionCalled);
        }

        [Fact]
        public void OkCommand_ShouldCloseWindowWithTrue_WhenFormIsValid()
        {
            // Arrange
            var viewModel = new AddBlockViewModel(new List<string>());

            bool? dialogResult = null;
            viewModel.CloseAction = (result) => dialogResult = result;

            // Act
            viewModel.Name = "NewUniqueBlock";
            viewModel.X = 100;
            viewModel.Y = 100;
            viewModel.Ok.Execute(null);

            // Assert
            Assert.False(viewModel.HasErrors);
            Assert.True(dialogResult);
        }
    }
}
