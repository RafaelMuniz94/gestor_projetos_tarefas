using Xunit;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Gestor_Projetos_Tarefas.Api.Utils;

namespace Gestor_Projetos_Tarefas.Test.Utils
{
    public class ErrorHandlingUtilsTests
    {
        [Fact]
        public void ReturnModelStateMessages_ValidModelState_ReturnsEmptyString()
        {
            // Arrange
            var utils = new ErrorHandlingUtils();
            var modelState = new ModelStateDictionary();

            // Act
            var result = utils.ReturnModelStateMessages(modelState);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void ReturnModelStateMessages_InvalidModelState_ReturnsErrorMessages()
        {
            // Arrange
            var utils = new ErrorHandlingUtils();
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("Field1", "Error message 1");
            modelState.AddModelError("Field2", "Error message 2");

            // Act
            var result = utils.ReturnModelStateMessages(modelState);

            // Assert
            Assert.Equal("Error message 1; Error message 2", result);
        }
    }
}
