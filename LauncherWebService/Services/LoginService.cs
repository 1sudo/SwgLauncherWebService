using Grpc.Core;
using LauncherWebService.Database;
using Microsoft.EntityFrameworkCore;

namespace LauncherWebService.Services;

public class LoginService : LoginManager.LoginManagerBase
{
    public override async Task? RequestLogin(LoginRequest request, IServerStreamWriter<LoginReply> responseStream, ServerCallContext context)
    {
        await using var db = new SwgEmuAccount.AccountContext();

        var account = await db.accounts!.FirstOrDefaultAsync(a => 
            a.username != null && a.username == request.Username);

        if (account?.username is null && account?.salt is not null) return;

        var hashedPassword = SwgEmuAccountUtils.ComputeHash(request.Password, account?.salt!);

        if (account?.password == hashedPassword)
        {
            var characters = db.characters!.Where(c => c.account_id == account.account_id).ToList();

            // Stream characters
            // Status and user will be sent multiple times, unfortunate side effect. No big deal.
            foreach (var characterName in characters.Select(character => $"{character.firstname} {character.surname}".Trim()))
            {
                await responseStream.WriteAsync(new LoginReply
                {
                    Status = "ok",
                    Username = request.Username,
                    Characters = { characterName }
                });
            }
        }
        else
        {
            await responseStream.WriteAsync(new LoginReply
            {
                Status = "Invalid Login Credentials. Please try again.",
                Username = request.Username,
                Characters = { }
            });
        }
    }
}
