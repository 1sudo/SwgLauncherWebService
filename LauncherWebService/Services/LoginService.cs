using Grpc.Core;
using LauncherWebService.Database;

namespace LauncherWebService.Services;

public class LoginService : LoginManager.LoginManagerBase
{
    public override async Task? RequestLogin(LoginRequest request, IServerStreamWriter<LoginReply> responseStream, ServerCallContext context)
    {
        using var db = new SwgEmuAccount.AccountContext();

        try
        {
            var account = db.accounts!.First(a => a.username == request.Username);

            Console.WriteLine($"{account.username}, {account.password}, {account.salt}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }

        List<string> chars = new()
        {
            "Mr Potato Head",
            "Bob Dole",
            "Tinkerbell"
        };

        foreach (var ch in chars)
        {
            await responseStream.WriteAsync(new LoginReply
            {
                Status = "ok",
                Username = request.Username,
                Characters = { ch }
            });
        }
    }
}
