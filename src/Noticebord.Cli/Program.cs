﻿using Microsoft.Extensions.DependencyInjection;
using Noticebord.Cli.Commands;
using Noticebord.Cli.Infrastructure;
using Noticebord.Cli.Settings;
using Noticebord.Client;
using Spectre.Console.Cli;

var registrations = new ServiceCollection();
registrations.AddSingleton<IClient, NoticebordClient>();

var app = new CommandApp(new TypeRegistrar(registrations));

app.Configure(config =>
{
    config.AddBranch<NoticesSettings>("notices", notices =>
    {
        notices.AddCommand<ListNoticesCommand>("list")
            .WithDescription("List all notices")
            .WithExample(new[] { "notices", "list" });
        notices.AddCommand<ShowNoticeCommand>("show")
            .WithDescription("Show a single notice by its ID")
            .WithExample(new[] { "notices", "show", "1" });
    });

    config.AddCommand<LoginCommand>("login")
            .WithDescription("Log in to your account")
            .WithExample(new[] { "login", "--interactive" })
            .WithExample(new[] { "login", "--username", "user@mail.com", "--password", "password" });
});

return app.Run(args);




