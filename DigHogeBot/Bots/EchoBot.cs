// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DigHogeBot.Bots {
    public class EchoBot: ActivityHandler {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) {
            if (turnContext.Activity.Text.ToLower() == "about") {
                var info = $"name={turnContext.Activity.From.Name}, oid={turnContext.Activity.From.AadObjectId}, role={turnContext.Activity.From.Role}";
                await turnContext.SendActivityAsync(MessageFactory.Text(info, info), cancellationToken);
                var info2 = $"ChannelId={turnContext.Activity.ChannelId} Speak={turnContext.Activity.Speak}";
                await turnContext.SendActivityAsync(MessageFactory.Text(info2, info2), cancellationToken);
            } else {
                var replyText = $"今、{turnContext.Activity.Text} って言いました？";
                await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken) {
            var welcomeText = "こんにちは。DIG Hogeボットです！";
            foreach (var member in membersAdded) {
                if (member.Id != turnContext.Activity.Recipient.Id) {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
