namespace BarberBossI.Communication.Responses;

public  class ResponseErrorJson
{
	public IEnumerable<string> ErrorMessages { get; set; }

	public ResponseErrorJson(string errorMessage)
	{
		ErrorMessages = [errorMessage];
	}

	public ResponseErrorJson(IEnumerable<string> errorMessages)
	{
		ErrorMessages = errorMessages;
	}
}
