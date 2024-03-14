using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gestor_Projetos_Tarefas.Api.Utils
{
    public class ErrorHandlingUtils
    {
        public String ReturnModelStateMessages(ModelStateDictionary  ModelState)
        {

            string errorMessages = string.Join("; ", ModelState.Values.SelectMany(model => model.Errors).Select(error => error.ErrorMessage));
            return errorMessages;

        }
    }
}
