using Grpc.Core;
using LauncherWebService.Database;
using Microsoft.EntityFrameworkCore;

namespace LauncherWebService.Services;

public class LoginService : LoginManager.LoginManagerBase
{
    public override async Task? RequestLogin(LoginRequest request, IServerStreamWriter<LoginReply> responseStream, ServerCallContext context)
    {
        using var db = new SwgEmuAccount.AccountContext();

        var account = await db.accounts!.FirstOrDefaultAsync(a => a.username != null && a.username == request.Username);

        if (account is null) return;

        Console.WriteLine($"{account.username}, {account.password}, {account.salt}");

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
