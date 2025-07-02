namespace BarberBossI.Exception.ExceptionsBase;

public abstract class FaturamentoException : SystemException
{
	protected FaturamentoException(string message) : base(message)
	{
	}

	public abstract int StatusCode { get; }
	public abstract List<string> GetErrors();
}
