#Automatic Build Converter
##by Emzi0767
##Version 2.1.0.0

##ABOUT

A small tool which automatically converts STO Academy builds into [/r/stobuilds](https://reddit.com/r/stobuilds) templates.

This tool helps alleviate the issue some people are having with STO Academy that makes them unable to load it, and removes the inconvenience that having to click through various tabs and icons to see complete build information. It also enables easier build maintenance.

Note that this is not a full substitute for filling out the template yourself, as the automatically converted template still lacks some information. It's there just to ease the pain of migration.

If you have any additional questions or suggestions, you can post them [on the issue tracker](https://github.com/Emzi0767/STO-Academy-Build-Tool/issues) (preferred) or in the [reddit thread](https://redd.it/5466ul).

##IMPORTANT NOTE

Please don't alter the output from the application. While certain parts of it may seem unimportant or irrelevant to you, they contain important information, not just for people who may try to help you, but also for me as the application's developer. In the event of there being problems with the output, I won't be able to diagnose the issue if the output has been altered. You can add a couple notes before the output, just don't modify the output itself.

Additionally, always remember to fill out as much data as possible, so that we can provide assistance based on your actual setup.

##SYSTEM REQUIREMENTS

* **Windows**:
   * Minimum required Windows version: Windows Vista Service Pack 2.
   * I will provide support only for the following Windows versions:
      * Windows 7 Service Pack 1 (32- and 64-bit).
      * Windows 8.1 Update (32- and 64-bit).
      * Windows 10 Anniversary Update (32- and 64-bit).
   * [Microsoft .NET Framework, version 4.5.2](https://www.microsoft.com/en-us/download/details.aspx?id=42643). This is installed by default on Windows 8.1 and 10.
* **GNU/Linux, Mac OS, and other \*NIX systems**:
   * Mono, version 4.x or newer
      * [Installing on GNU/Linux](http://www.mono-project.com/docs/getting-started/install/linux/).
      * [Installing on Mac OS](http://www.mono-project.com/docs/getting-started/install/mac/).
      * [Compiling from sources](http://www.mono-project.com/docs/compiling-mono/).
   * Minimum required Mono version is 3.2.x, however I will only provide support for 4.x

##HOW TO USE

1. Unzip the file to any directory.
2. Run abt.exe and wait for the program to load its data.
3. Enter the full build URL into the text box.
4. Press convert and wait for the program to convert the build.
5. Once conversion is complete, a file save prompt or clipboard content prompt will appear, depending on your choices. Save the file or confirm content replacement.
6. Open the saved file in a text editor, copy its contents, and paste them to your build post on [/r/stobuilds](https://reddit.com/r/stobuilds).

###RUNNING ON MAC OS

Since I don't own a Mac, and I can't package the application as a nice and nifty .app package, the only way to run the application is via terminal.

1. Open Terminal
2. Navigate to the directory where you extracted the converter (`cd path`).
   * For example, if it was extracted to a directory called abt inside your Downloads directory, you want to `cd Downloads/abt`.
3. Make the converter binary executable (`chmod +x abt2.exe`).
   * This is only required after extracting the converter.
4. Run it (`mono abt2.exe`).

Please note that first run may take a while, as Mono needs to cache your fonts and do some other first-run tasks.

##REPORTING BUGS

Bugs happen, no software is perfect. If you happen to cause the software to crash or otherwise behave in an unintended manner, make sure to let me know using via [the issue tracker](https://github.com/Emzi0767/STO-Academy-Build-Tool/issues) (preferred) or [reddit PM](https://www.reddit.com/message/compose/?to=eMZi0767&subject=ABT%20Bug%20Report&message=I%20experienced%20a%20crash%20with%20ABT.%20Attached%20below%20is%20the%20bug%20report.%0A%0A%3Cinsert%20link%20to%20.bug%20file%20here%3E). If the application crashes, it should generate a .bug file in the directory it is in. If it did, make sure to attach that file as well, it will help me diagnose the issue better. If possible, include the link you were trying to convert, and list of steps you took that caused the problem.