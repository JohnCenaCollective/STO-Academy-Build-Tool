#STO Academy Converter Bot by Emzi0767, updated by JohnCenaPTF

##ABOUT

A reddit bot built on top of Emzi0767's [Academy Tool](https://github.com/JohnCenaCollective/STO-Academy-Build-Tool), designed to automatically convert STO Academy Link posts and comments into [/r/stobuilds](https://reddit.com/r/stobuilds) templates. It will automatically pull new posts and comments from /r/stobuilds, convert them, and reply with converted build.

This is designed for people who can't fill out the template themselves, or can't use the Build Tool. 

If you have any additional questions or suggestions, you can post them [on the issue tracker](https://github.com/JohnCenaCollective/STO-Academy-Build-Tool/issues).

##RUNNING THE BOT

###SETTING UP MONO

To run this bot, you need Mono, version 4.x or newer. You need to consult your GNU/Linux distribution repository to see which mono version is available in your distro's repository. If the version is insufficient, you might need to build it from sources.

To verify that mono is available, and usable, run `mono -V`. You should see something like this:

    Mono JIT compiler version 4.9.0 (yadda yadda text)
    More text

If you see something like this, and the version is 4.0.0 or newer, then your mono is usable.

###SETTING UP AN APPLICATION ON REDDIT

You need to go to [app settings on reddit](https://www.reddit.com/prefs/apps/), and on the bottom, create another app. Give it a name, set the type to installed app, set a redirect uri.

###SETTING UP THE ENVIRONMENT

In order to run the bot, you will first need to set up a directory structure for it. It should look like this:

    academy_bot/
      acb_v2.exe
      v2_lib/
        Emzi0767.Gaming.Sto.AcademyConverterBot.dll
        Emzi0767.Gaming.Sto.StoaLib.dll
        Emzi0767.Tools.MicroLogger.dll
        Html2Markdown.dll
        HtmlAgilityPack.dll
        Newtonsoft.Json.dll
      v2_settings/
        config.json
        settings.json
        oauth.json

You need to make sure that `acb_v2.exe` is executable.

###SETTING UP OAUTH

This part is difficult, and how you do it is entirely up to you. I set the redirect uri to a script on my local http server that completed OAuth authentication and created a `oauth.json` file for me.

###SETTING UP CONFIGS

Next up, you need to edit configs. First config to edit is `oauth.json`, which contains reddit OAuth settings. You need to edit `access_token` to contain your reddit OAuth token, and `refresh_token` to contain your reddit OAuth refresh token.

Next, `settings.json`. Edit `app_id` to match ID of the aplication you set up on reddit.

Last, `config.json`. It contains a variety of configuration options, although for now this is just Discord logging settings. It is pretty straightforward. `use_discord` defines whether to output data to discord, `discord_ip` and `discord_port` define where is the [Discord Logger](https://github.com/Emzi0767/Discord-Logger) running.

###SETTING UP A SCRIPT TO RUN THE BOT

This is necessary, because we'll be using cron. Put this in the script:

    #!/bin/bash
    
    cd <absolute_path_to_the_bot>
    mono acb_v2.exe

For the purpose of these instructions, let's assume you saved that as `runrobit.sh`.

###SETTING UP CRON

You need to edit crontab to match your needs. Do `crontab -e`, and append the following:

    # Environment Setup
    SHELL=/bin/bash
    PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin
    
    # Tasks
    */5 * * * * <absolute_path_to_the_bot>/runrobit.sh

Save and exit. This will make the bot run every 5 minutes.

###GRABBING A DRINK AND CELEBRATING VICTORY

If you did everything correctly, you can now grab a drink, and watch the bot do its job.

##REPORTING BUGS

Bugs happen, no software is perfect. If you happen to cause the software to crash or otherwise behave in an unintended manner, make sure to let us know using via [the issue tracker](https://github.com/JohnCenaCollective/STO-Academy-Build-Tool/issues). If possible, include the list of steps you took that caused the problem.