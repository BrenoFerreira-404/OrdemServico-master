namespace Domain.Exceptions;

public sealed class AcessoTenantNegadoException : DomainException
{
    public AcessoTenantNegadoException()
        : base("Acesso negado: recurso pertence a outro tenant.") { }

    public AcessoTenantNegadoException(string message) : base(message) { }
}
