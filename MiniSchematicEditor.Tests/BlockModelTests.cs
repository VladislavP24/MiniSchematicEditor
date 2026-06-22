using MiniSchematicEditor.Models;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSchematicEditor.Tests
{
    public class BlockModelTests
    {
        [Fact]
        public void Name_ShouldHaveError_WhenEmpty()
        {
            // Arrange
            var block = new BlockModel { Name = "ValidName" };

            // Act
            block.Name = "";

            // Assert
            Assert.True(block.HasErrors);
            var errors = block.GetErrors(nameof(block.Name));
            Assert.NotEmpty(errors);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(1501)]
        public void X_ShouldHaveError_WhenOutOfRange(double invalidX)
        {
            // Arrange
            var block = new BlockModel();

            // Act
            block.X = invalidX;

            // Assert
            Assert.True(block.HasErrors);
        }

        [Fact]
        public void Name_ShouldHaveError_WhenNotUnique()
        {
            // Arrange
            var block = new BlockModel { UId = "1" };
            block.IsNameUniqueChecker = (id, name) => false;

            // Act
            block.Name = "Duplicate";

            // Assert
            Assert.True(block.HasErrors);
        }
    }
}
