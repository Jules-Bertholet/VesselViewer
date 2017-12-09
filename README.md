# Vessel Viewer Continued

KSP Forum Link: https://forum.kerbalspaceprogram.com/index.php?/topic/146692-131-vessel-viewer-continued/

## Important!

This plugin is in maintenance mode, with the primary intent being to fix bugs and update for new versions of KSP.

## What is it?

From the original post on the forums:

> Ever fly an IVA mission, happened to hear an explosion and wished you knew if it was antenna no. 54 or your main engine that just blew up?

> Like to fly without the GUI but also a fan of knowing how much fuel there is in your tanks? Wondering just how hot is Deadly Reentry making those wings? Perhaps you'd like a more visual way of telling whenever your wings are stalling when running Ferram Aerospace?

> Ever wished you could activate engines and take science reports without having to go into external view, but too lazy or forgetful to setup action groups?

> I've got you covered.

## Installation requirements

To use this mod, you will need to install ModuleManager and either Toolbar or RasterPropMonitor (or both if you like!).  Copy the VesselView folder into the GameData folder on your KSP install, and it should work!

## Build instructions

You will need a full installation of Mono to build this plugin under Linux.  Windows folks, please report in with your build process!

1. Create a symlink to your KSP directory.  You will need RasterPropMonitor installed to build the plugin.

```shell
$ ln -s ~/.steam/steamapps/common/Kerbal\ Space\ Program .
```

2. Change directory to the `VesselView` directory and start the build.

```shell
$ cd VesselView
$ msbuild /t:Rebuild /p:Configuration=Release
```

3. Change directory back up and the plugin will be in `GameData\VesselView`!
