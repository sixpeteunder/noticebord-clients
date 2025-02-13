﻿using System;
using System.IO;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Noticebord.Cli.Commands.Auth;
using Noticebord.Cli.Commands.Notices;
using Noticebord.Cli.Infrastructure;
using Noticebord.Cli.Settings.Notices;
using Noticebord.Client;
using Spectre.Console.Cli;

// const string? baseUrl = "http://localhost:8000/api";
const string? baseUrl = default;

string? token = default;

var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
var path = Path.Combine(appDataPath, "noticebord", "token.txt");

if (File.Exists(path)) token = await File.ReadAllTextAsync(path);

var services = new ServiceCollection();
services.AddSingleton<IClient>(new NoticebordClient(token , baseUrl, "Noticebord.Cli"));

var app = new CommandApp(new TypeRegistrar(services));

app.Configure(config =>
{
    config.AddBranch<NoticesSettings>("notices", notices =>
    {
        notices.AddCommand<CreateNoticeCommand>("create")
            .WithDescription("Create a new notice")
            .WithExample(new[] { "notices", "create" });
        notices.AddCommand<ListNoticesCommand>("list")
            .WithDescription("List all notices")
            .WithExample(new[] { "notices", "list" });
        notices.AddCommand<ShowNoticeCommand>("show")
            .WithDescription("Show a single notice by its ID")
            .WithExample(new[] { "notices", "show", "1" });
        notices.AddCommand<UpdateNoticeCommand>("update")
            .WithDescription("Update a single notice by its ID")
            .WithExample(new[] { "notices", "update", "1" });
        notices.AddCommand<DeleteNoticeCommand>("delete")
            .WithDescription("Delete a single notice by its ID")
            .WithExample(new[] { "notices", "delete", "1" });
    });

    config.AddCommand<LoginCommand>("login")
            .WithDescription("Log in to your account")
            .WithExample(new[] { "login" });

    config.AddCommand<LogoutCommand>("logout")
            .WithDescription("Log out of your account")
            .WithExample(new[] { "logout" });
});

return app.Run(args);