using System.Diagnostics.CodeAnalysis;

namespace Gestor_Projetos_Tarefas.Domain.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class DomainException: Exception
    {
        public DomainException()
        {
            
        }

        public DomainException(string message)
        : base(message) { }

        public DomainException(string message, Exception inner)
       : base(message, inner) { }
    }

}
