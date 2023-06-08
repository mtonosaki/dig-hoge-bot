// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using DigHogeBot.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DigHogeBot.Bots {
    public class EchoBot: ActivityHandler, IWebAnalyzerBot {
        private static readonly HttpClient httpClient = new();
        private HttpRequest latestRequest = null;
        private HttpResponse latestResponse = null;
        private HttpContext latestHttpContext = null;

        public HttpRequest Request {
            set => latestRequest = value;
        }
        public HttpResponse Response {
            set => latestResponse = value;
        }
        public HttpContext Context {
            set => latestHttpContext = value;
        }

        private async Task<string> makeStringAsync(object val) {
            var str = val?.ToString() ?? "(null)";
            if (val is Stream rs) {
                var sr = new StreamReader(rs);
                str = await sr.ReadToEndAsync();
            }
            if (val is IDictionary<string, object?> dic) {
                str = string.Join(", ", dic.Select(kv => $"{kv.Key}={kv.Value?.ToString()}"));
            }
            if (val is IQueryCollection col) {
                str = string.Join(", ", col.Select(kv => $"{kv.Key}=[{string.Join(", ", kv.Value.Select(async v => await makeStringAsync(v)))}]"));
            }
            if (val is IEnumerable<KeyValuePair<string, string>> kvs) {
                str = string.Join(", ", kvs.Select(kv => $"{kv.Key}={kv.Value}"));
            }
            if (val is MiddlewareSet ms) {
                str = string.Join(", ", ms.Select(middleWare => $"{middleWare}"));
            }
            if (string.IsNullOrEmpty(str)) {
                str = $"(なし)";
            }
            return str;
        }

        private async Task autoHandlingAsync(object target, ITurnContext<IMessageActivity> turnContext, string[] co, CancellationToken cancellationToken) {
            if (target == null) {
                var mes = $"{co[0]} が null だったので、情報収集ムリでした。";
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                return;
            }
            try {
                if (co.Length == 2) {
                    var type = target.GetType();
                    if (co[1] == "一覧" || co[1].Equals("list", StringComparison.OrdinalIgnoreCase)) {
                        var props = string.Join(", ", type.GetProperties().Select(pi => pi.Name));
                        var mes = string.IsNullOrEmpty(props)
                            ? $"{type.Name}型の {co[0]} には 表示できるプロパティは無いようですね。"
                            : $"{type.Name}型の {co[0]} には 次のプロパティがあるようです。" + props;
                        _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                        return;
                    }
                    var pi = type.GetProperty(co[1]);
                    if (pi != null) {
                        var val = pi.GetValue(target);
                        var str = await makeStringAsync(val);
                        var mes = $"{type.Name}型 {co[0]}内の {co[1]} は、{str} でした。";
                        _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                        return;
                    } else {
                        var mes = $"{type.Name}型 {co[0]}内の {co[1]} という値は見つかりませんでした。。。";
                        _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                        return;
                    }
                }
                if (co.Length > 2) {
                    var type = target.GetType();
                    var pi = type.GetProperty(co[1]);
                    if (pi != null) {
                        var val = pi.GetValue(target);
                        await autoHandlingAsync(val, turnContext, co.Skip(1).ToArray(), cancellationToken);
                        return;
                    } else {
                        var mes = $"なるほど、{type.Name}には、{co[1]} というプロパティは無いようですね。";
                        _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                        return;
                    }
                }
                var err = $"なるほど？ 子プロパティが知りたければ、 {co[0]} list と聞いてみてください";
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(err, err), cancellationToken);
            } catch {
                var mes = $"{string.Join(" ", co)}は どうやら表示させてくれないみたいです";
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
            }

        }

        private async Task answerHelp(string target, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) {
            string mes;
            if (string.IsNullOrEmpty(target)) {
                mes = "相談してくれてありがとう！ 次のコマンドが用意されていますのでやってみてくださいね。";
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                mes = "req, res, act, context, http, about, ip address";
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                mes = "また、req Headers のように スペースで区切って、子プロパティの中を見ることもできますよ。";
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                mes = "もしどんな子プロパティがあるかが知りたければ、req list とか、req Headers list のように一覧を要求してくださいね！";
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                return;
            }
            if (target.StartsWith("ip")) {
                mes = "Botを起点としたアウトバウンドのIPアドレスを調べます。使うAPIは http://httpbin.org/ip です。";
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                return;
            }
            mes = target.ToLower() switch {
                "controller" => "Controllerって、ほら、あれですよ。MVCのController。ググってみて。",
                "http" => "Controllerで取得できる HttpContext オブジェクトです。",
                "httpcontext" => "Controllerで取得できる HttpContext オブジェクトです。",
                "req" => "Controllerで取得できる HttpContext の Request オブジェクトです。",
                "res" => "Controllerで取得できる HttpContext の Response オブジェクトです。",
                "context" => "Botの Turn Context です。",
                "act" => "Botの Turn Context内にある Activity オブジェクトです。",
                "about" => "BotのContextオブジェクトから色々ピックアップします。",
                _ => $"{target}って、美味しそうですね。",
            };
            _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
        }

        private async Task<bool> NeedHelp(string[] co, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) {
            if (co.Length == 1 && co[0].EndsWith("って")) {
                await answerHelp(co[0][..^2], turnContext, cancellationToken);
                return true;
            }
            if (co.Length == 1 && (co[0].EndsWith("?") || co[0].EndsWith("？"))) {
                await answerHelp(co[0][..^1], turnContext, cancellationToken);
                return true;
            }
            if (co.Length == 2 && (co[1].StartsWith("って") || co[1] == "?" || co[1] == "？")) {
                await answerHelp(co[0], turnContext, cancellationToken);
                return true;
            }
            if ((co[0].Equals("help", StringComparison.OrdinalIgnoreCase) || co[0] == "?" || co[0] == "？") && co.Length <= 2) {
                await answerHelp(co.Length == 2 ? co[1] : string.Empty, turnContext, cancellationToken);
                return true;
            }
            return false;
        }
        private static async Task<bool> ConciderAbout(string[] co, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) {
            if (co.Length == 1 && co[0].Equals("about", StringComparison.OrdinalIgnoreCase)) {
                var messages = new List<string>{
                    $"Text={turnContext.Activity.Text}",
                    $"Id={turnContext.Activity.Id}",
                    $"ChannelId={turnContext.Activity.ChannelId}",
                    $"DeliveryMode={turnContext.Activity.DeliveryMode}",
                    $"Expiration={turnContext.Activity.Expiration?.ToString() ?? "null"}",
                    $"Locale={turnContext.Activity.Locale}",
                    $"LocalTimestamp={turnContext.Activity.LocalTimestamp?.ToString()}",
                    $"ReplyToId={turnContext.Activity.ReplyToId}",
                    $"Summary={turnContext.Activity.Summary}",
                    $"From.Name={turnContext.Activity.From.Name}",
                    $"From.Id={turnContext.Activity.From.Id}",
                    $"From.AadObjectId={turnContext.Activity.From.AadObjectId}",
                    $"From.Role={turnContext.Activity.From.Role}",
                    $"Conversation.Name={turnContext.Activity.Conversation?.Name}",
                    $"Conversation.Id={turnContext.Activity.Conversation?.Id}",
                    $"Conversation.TenantId={turnContext.Activity.Conversation?.TenantId}",
                    $"Conversation.AadObjectId={turnContext.Activity.Conversation?.AadObjectId}",
                };
                foreach (var message in messages.Where(_ => !cancellationToken.IsCancellationRequested)) {
                    _ = await turnContext.SendActivityAsync(MessageFactory.Text(message, message), cancellationToken);
                }
                return true;
            }
            return false;
        }
        private static async Task<bool> ConciderOutboundIp(string[] co, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) {
            var talk = turnContext.Activity.Text.ToLower();
            if (talk.Contains("ip") && (talk.Contains("address") || talk.Contains("アドレス"))) {
                var message = "BotからのアウトバウンドIPですか？  ええっと．．．";
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(message, message), cancellationToken);

                try {
                    var res = await httpClient.GetAsync("http://httpbin.org/ip", cancellationToken);
                    if (res != null && res.IsSuccessStatusCode) {
                        var json = await res.Content.ReadAsStringAsync(cancellationToken);
                        _ = await turnContext.SendActivityAsync(MessageFactory.Text(json, json), cancellationToken);
                        return true;
                    } else {
                        var mes = $"IP確認中にエラー出ちゃいました。APIからの戻り値は、{res?.StatusCode} でした。";
                        _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                    }
                } catch (Exception ex) {
                    var mes = $"IP確認中に例外出ちゃいました。これ、分かります？↓";
                    _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                    _ = await turnContext.SendActivityAsync(MessageFactory.Text(ex.Message, ex.Message), cancellationToken);
                }
            }
            return false;
        }
        private static async Task<bool> Guard(string[] co, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) {
            if (turnContext.Activity.Text.Length > 1024) {
                var mes = "うぇ～い、もっとシンプルに言って！";
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                return true;
            }
            if (co.Length > 32) {
                var mes = "え、え、もっと簡単に言うと？";
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                return true;
            }
            return false;
        }

        private static async Task<bool> PlayHoge(string[] co, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) {
            if (co.Length < 1) {
                return false;
            }

            var playList = new List<string> {
                "hoge", "fuga", "piyo", "poki", "foo", "bar", "baz",
            };
            if (playList.Contains(co[0].ToLower())) {
                var random = new Random();
                var mes = playList[random.Next(playList.Count)];
                _ = await turnContext.SendActivityAsync(MessageFactory.Text(mes, mes), cancellationToken);
                return true;
            }
            return false;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) {
            var tenantIds = Startup.ConfigCurrent.GetValue<string>("AcceptTenandIds");
            if (!string.IsNullOrEmpty(tenantIds)) {
                var ids = tenantIds.Split(',').Select(x => x.Trim());
                if(!ids.Contains(turnContext.Activity.Conversation.TenantId)) {
                    return;
                }
            }

            var co = turnContext.Activity.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (await Guard(co, turnContext, cancellationToken)) {
                return;
            }
            if (await NeedHelp(co, turnContext, cancellationToken)) {
                return;
            }
            if (await ConciderAbout(co, turnContext, cancellationToken)) {
                return;
            }
            if (await ConciderOutboundIp(co, turnContext, cancellationToken)) {
                return;
            }
            if (await PlayHoge(co, turnContext, cancellationToken)) {
                return;
            }

            var command = co[0].ToLower();
            if (command.StartsWith("req")) {
                await autoHandlingAsync(latestRequest, turnContext, co, cancellationToken);
                return;
            }
            if (command.StartsWith("res")) {
                await autoHandlingAsync(latestResponse, turnContext, co, cancellationToken);
                return;
            }
            if (command == "http") {
                await autoHandlingAsync(latestHttpContext, turnContext, co, cancellationToken);
                return;
            }
            if (command.StartsWith("act")) {
                await autoHandlingAsync(turnContext.Activity, turnContext, co, cancellationToken);
                return;
            }
            if (command == "context") {
                await autoHandlingAsync(turnContext, turnContext, co, cancellationToken);
                return;
            }

            var replyText = $"今、{turnContext.Activity.Text} って言いました？";
            _ = await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken) {
            var welcomeText = "こんにちは。DIG Hogeボットです！ help と入力したら説明させていただきますね";
            foreach (var member in membersAdded) {
                if (member.Id != turnContext.Activity.Recipient.Id) {
                    _ = await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
