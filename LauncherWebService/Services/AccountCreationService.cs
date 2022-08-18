using Grpc.Core;
using LauncherWebService.Database;

namespace LauncherWebService.Services;

public class AccountCreationService : AccountCreationManager.AccountCreationManagerBase
{
    public override async Task<CreateReply> RequestCreate(CreateRequest request, ServerCallContext context)
    {
        await using var db = new SwgEmuAccount.AccountContext();

        // If username already exists, return
        if (db.accounts is not null && db.accounts.Any(a => a.username == request.Username)) {
            return new CreateReply()
            {
                Status = "Username already exists!"
            };
        }

        var (password, salt) = SwgEmuAccountUtils.HashPassword(request.Password);

        db.accounts!.Add(new SwgEmuAccount.Account
        {
            username = request.Username.ToLower(),
            password = password,
            station_id = SwgEmuAccountUtils.GenerateStationId(),
            active = 1,
            admin_level = 0,
            salt = salt,
            email = request.Email,
            discord = request.DiscordId,
            subscribed = request.SubscribeToNewsletter
        });

        await db.SaveChangesAsync();

        return new CreateReply()
        {
            Status = "ok"
        };
    }
}
