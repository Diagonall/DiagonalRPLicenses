using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Linq;
using Rocket.API.Collections;
using Rocket.API;
using System;
using Rocket.Unturned;
using System.Collections;
using UnityEngine;

namespace Diagonal.RPLicenses
{
    public class RPLicenses : RocketPlugin<RPLicensesConfiguration>
    {
        public static RPLicenses Instance;
        
        #region Write
        public static void Write(string message)
        {
            Console.WriteLine(message);
        }
        public static void Write(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        #endregion

        #region Load/Unload
        protected override void Load()
        {
            Instance = this;

            ItemManager.onTakeItemRequested += onTakeItemRequested;
            VehicleManager.onEnterVehicleRequested += VehicleManager_onEnterVehicleRequested;
            U.Events.OnPlayerDisconnected += Events_OnPlayerDisconnected;
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;

            #region WriteLoad
            if (Configuration.Instance.VehicleLicense)
            {
                Write("Vehicle License: Enabled", ConsoleColor.Green);
                Write($"Vehicle License Permission: {Configuration.Instance.VehicleLicensePermission}", ConsoleColor.DarkGreen);
                Write($"Vehicle License ID: {Configuration.Instance.VehicleLicenseID}", ConsoleColor.DarkGreen);
                if (Configuration.Instance.DontLetInVehicle)
                {
                    Write("Kick On Enter: Enabled", ConsoleColor.Green);
                }
                else
                {
                    Write("Kick On Enter: Disabled", ConsoleColor.Red);
                }
            }
            else
            {
                Write("Vehicle License: Disabled", ConsoleColor.Red);
            }

            if (Configuration.Instance.WeaponLicense)
            {
                Write("Weapon License: Enabled", ConsoleColor.Green);
                Write($"Weapon License Permission: {Configuration.Instance.WeaponLicensePermission}", ConsoleColor.DarkGreen);
                Write($"Weapon License ID: {Configuration.Instance.WeaponLicenseID}", ConsoleColor.DarkGreen);

                if (Configuration.Instance.DontLetGetWeapon)
                {
                    Write("Drop On Take: Enabled", ConsoleColor.Green);
                }
                else
                {
                    Write("Drop On Take: Disabled", ConsoleColor.Red);
                }
            }
            else
            {
                Write("Vehicle License: Disabled", ConsoleColor.Red);
            }

            if (Configuration.Instance.DisableLicensesOnGroupOnline)
            {
                Write("Disable Licenses On Group Online: Enabled", ConsoleColor.Green);
                Write($"Disable Licenses Permission: {Configuration.Instance.DisableLicensesPermission}", ConsoleColor.DarkGreen);
                if (Configuration.Instance.LicensesMessage)
                {
                    Write("Licenses Message: Enabled", ConsoleColor.Green);
                }
                else
                {
                    Write("Licenses Message: Disabled", ConsoleColor.Red);
                }
            }
            else
            {
                Write("DisableLicenses On Group Online: Disabled", ConsoleColor.Red);
            }
            #endregion
        }

        protected override void Unload()
        {
            Instance = null;

            ItemManager.onTakeItemRequested += onTakeItemRequested;
            VehicleManager.onEnterVehicleRequested -= VehicleManager_onEnterVehicleRequested;
            U.Events.OnPlayerDisconnected -= Events_OnPlayerDisconnected;
            U.Events.OnPlayerConnected -= Events_OnPlayerConnected;
        }
        #endregion

        #region Weapon License

        private void onTakeItemRequested(Player Pplayer, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {
            UnturnedPlayer player = UnturnedPlayer.FromPlayer(Pplayer);
            LicensedWeapon weapon = Configuration.Instance.Licensed.FirstOrDefault(w => w.Id == itemData.item.id);

            if (weapon != null && Configuration.Instance.WeaponLicense)
            {
                if (player.HasPermission(Configuration.Instance.WeaponLicensePermission))
                {
                    return;
                }

                if (player.IsAdmin && Instance.Configuration.Instance.IgnoreAdmin)
                {
                    return;
                }

                for (byte page = 0; page < 8; page++)
                {
                    var count = player.Inventory.getItemCount(page);

                    for (byte index = 0; index < count; index++)
                    {
                        if (player.Inventory.getItem(page, index).item.id == Instance.Configuration.Instance.WeaponLicenseID)
                        {
                            return;
                        }
                    }
                }

                if (Configuration.Instance.DisableLicensesOnGroupOnline)
                {
                    foreach (var steamPlayer in Provider.clients)
                    {
                        UnturnedPlayer Players = UnturnedPlayer.FromSteamPlayer(steamPlayer);

                        if (Players.HasPermission(Configuration.Instance.DisableLicensesPermission))
                        {
                            return;
                        }
                    }
                }

                if (Configuration.Instance.DontLetGetWeapon)
                {
                    shouldAllow = false;
                    UnturnedChat.Say(player, Translate("no_weapon_license"), Color.red);
                }
                else
                {
                    UnturnedChat.Say(player, Translate("carrying_no_license"), Color.red);
                    UnturnedChat.Say(player, Translate("carrying_no_license"), Color.red);
                    UnturnedChat.Say(player, Translate("carrying_no_license"), Color.red);
                }
            }
        }
        #endregion

        #region Vehicle License
        private void VehicleManager_onEnterVehicleRequested(Player Pplayer, InteractableVehicle vehicle, ref bool shouldAllow)
        {
            UnturnedPlayer player = UnturnedPlayer.FromPlayer(Pplayer);

            if (Configuration.Instance.VehicleLicense)
            {
                if (player.IsAdmin && Instance.Configuration.Instance.IgnoreAdmin)
                {
                    return;
                }

                if (player.HasPermission(Instance.Configuration.Instance.VehicleLicensePermission))
                {
                    return;
                }

                if (vehicle.asset.id == 185)
                {
                    return;
                }

                for (byte page = 0; page < 8; page++)
                {
                    var count = player.Inventory.getItemCount(page);

                    for (byte index = 0; index < count; index++)
                    {
                        if (player.Inventory.getItem(page, index).item.id == Instance.Configuration.Instance.VehicleLicenseID)
                        {
                            return;
                        }
                    }
                }

                if (Configuration.Instance.DisableLicensesOnGroupOnline)
                {
                    foreach (var steamPlayer in Provider.clients)
                    {
                        UnturnedPlayer Players = UnturnedPlayer.FromSteamPlayer(steamPlayer);

                        if (Players.HasPermission(Configuration.Instance.DisableLicensesPermission))
                        {
                            return;
                        }
                    }
                }

                if (Configuration.Instance.DontLetInVehicle)
                {
                    shouldAllow = false;
                    UnturnedChat.Say(player, Translate("no_vehicle_license"), Color.red);
                }
                else
                {
                    UnturnedChat.Say(player, Translate("driving_no_license"), Color.red);
                    UnturnedChat.Say(player, Translate("driving_no_license"), Color.red);
                    UnturnedChat.Say(player, Translate("driving_no_license"), Color.red);
                }
            }
        }
        #endregion

        #region Player Connected
        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            if (Configuration.Instance.DisableLicensesOnGroupOnline)
            {
                var SteamID = player.CSteamID;
                foreach (var steamPlayer in Provider.clients)
                {

                    if (player.IsAdmin)
                    {
                        return;
                    }

                    if (steamPlayer.playerID.steamID == SteamID && player.HasPermission(Configuration.Instance.DisableLicensesPermission))
                    {
                        if (Configuration.Instance.LicensesMessage)
                        {
                            UnturnedChat.Say(Translate("licenses_off"), Color.red);
                        }
                        continue;
                    }

                    UnturnedPlayer Players = UnturnedPlayer.FromSteamPlayer(steamPlayer);

                    if (Players.HasPermission(Configuration.Instance.DisableLicensesPermission))
                    {
                        return;
                    }
                }
            }
        }
        #endregion

        #region Player Disconnected
        private void Events_OnPlayerDisconnected(UnturnedPlayer player)
        {
            if (Configuration.Instance.DisableLicensesOnGroupOnline)
            {
                var SteamID = player.CSteamID;
                foreach (var steamPlayer in Provider.clients)
                {
                    if (player.IsAdmin)
                    {
                        return;
                    }

                    if (steamPlayer.playerID.steamID == SteamID && player.HasPermission(Configuration.Instance.DisableLicensesPermission))
                    {
                        if (Configuration.Instance.LicensesMessage)
                        {
                            UnturnedChat.Say(Translate("licenses_on"), Color.green);
                        }
                        continue;
                    }

                    UnturnedPlayer Players = UnturnedPlayer.FromSteamPlayer(steamPlayer);

                    if (Players.HasPermission(Configuration.Instance.DisableLicensesPermission))
                    {
                        return;
                    }
                }
            }
        }
        #endregion

        #region Translate
        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                { "no_weapon_license", "You do not have a license to use this weapon!" },
                { "no_vehicle_license", "You do not have a license to drive this vehicle!" },
                { "driving_no_license", "You're driving without a license!" },
                { "carrying_no_license", "You're carrying a gun without a license!" },
                { "licenses_on", "Disabled Licenses!" },
                { "licenses_off", "Enabled Licenses!" }
            };
        #endregion
    }
}
