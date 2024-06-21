
using SharedLibrary.Models.DtoModels.MainModels;
using SharedLibrary.Responses.Abstract;

namespace SharedLibrary.Responses
{
	[Serializable]
	public class AuthenticationResponse : ResponseBase, IAuthenticationResponse
	{
		public string? Token { get; set; }

		public AuthenticationResponse()
		{ }

		public AuthenticationResponse(string? token = null, string? errorInfo = null)
		{
			Token = token;
			ErrorInfo = errorInfo;
		}
	}
}


