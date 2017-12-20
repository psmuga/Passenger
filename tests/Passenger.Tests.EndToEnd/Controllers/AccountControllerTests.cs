using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Passenger.Api;
using Passenger.Infrastructure.Commands.Users;
using Passenger.Infrastructure.DTO;
using Xunit;
namespace Passenger.Tests.EndToEnd.Controllers
{
    public class AccountControllerTests : ControllerTestsBase
    {


        [Fact]
        public async Task given_valid_current_and_new_password_it_should_be_changed()
        {
            // Act
            var command = new ChangeUserPassword
            {
                CurrentPassword = "secret",
                NewPassword = "secret2"
            };
            var payload = GetPayload(command);
            var response = await Client.PutAsync("account/password",payload);

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NoContent);

 
        }
    }
}