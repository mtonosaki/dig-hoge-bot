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
                var messages = new List<string>{
                    $"From.Name={turnContext.Activity.From.Name}",
                    $"From.Id={turnContext.Activity.From.Id}",
                    $"From.AadObjectId={turnContext.Activity.From.AadObjectId}",
                    $"From.Role={turnContext.Activity.From.Role}",
                    $"Text={turnContext.Activity.Text}",
                    $"Id={turnContext.Activity.Id}",
                    $"Expiration={turnContext.Activity.Expiration?.ToString() ?? "null"}",
                    $"ChannelId={turnContext.Activity.ChannelId}",
                    $"Conversation.Name={turnContext.Activity.Conversation?.Name}",
                    $"Conversation.Id={turnContext.Activity.Conversation?.Id}",
                    $"Conversation.TenantId={turnContext.Activity.Conversation?.TenantId}",
                    $"Conversation.AadObjectId={turnContext.Activity.Conversation?.AadObjectId}",
                };
                foreach(var message in messages) {
                    await turnContext.SendActivityAsync(MessageFactory.Text(message, message), cancellationToken);
                }
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
