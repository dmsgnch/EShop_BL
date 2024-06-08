
using EShop_BL.Models.MainModels;
using SharedLibrary.Responses.Abstract;

namespace SharedLibrary.Responses
{
	[Serializable]
	public class AuthenticationResponse : IInformable, IAuthenticationResponse
	{
		public User? User { get; set; }
		public string? Token { get; set; }
		
		public string[]? Info { get; set; }

		public AuthenticationResponse()
		{ }

		public AuthenticationResponse(User? user = null, string? token = null, string[]? info = null)
		{
			User = user;
			Token = token;
			Info = info;
		}
	}
}


