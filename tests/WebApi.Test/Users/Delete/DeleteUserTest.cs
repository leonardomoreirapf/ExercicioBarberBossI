using FluentAssertions;
using System.Net;

namespace WebApi.Test.Users.Delete
{
	public class DeleteUserTest : BarberBossClassFixture
	{
		private const string METHOD = "api/User";

		private readonly string _token;

		public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
		{
			_token = webApplicationFactory.UserTeamMember.GetToken();
		}

		[Fact]
		public async Task Sucess()
		{
			var result = await DoDelete(requestUri: METHOD, token: _token);

			result.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
	}
}
