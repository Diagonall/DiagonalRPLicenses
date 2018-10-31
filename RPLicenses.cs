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

            UnturnedPlayerEvents.OnPlayerInventoryAdded += OnInventoryUpdated;
            UnturnedPlayerEvents.OnPlayerUpdateStance += OnPlayerUpdateStance;

            #region WriteLoad
            if (Configuration.Instance.VehicleLicense)
            {
                Write("Vehicle License: Enabled", ConsoleColor.Green);
                Write($"Vehicle License Permission: {Configuration.Instance.VehicleLicensePermission}", ConsoleColor.DarkGreen);
                Write($"Vehicle License ID: {Configuration.Instance.VehicleLicenseID}", ConsoleColor.DarkGreen);
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
            }
            else
            {
                Write("Vehicle License: Disabled", ConsoleColor.Red);
            }
            #endregion
        }

        protected override void Unload()
        {
            Instance = null;

            UnturnedPlayerEvents.OnPlayerInventoryAdded -= OnInventoryUpdated;
            UnturnedPlayerEvents.OnPlayerUpdateStance -= OnPlayerUpdateStance;
        }
        #endregion

        #region Weapon License
        private void OnInventoryUpdated(UnturnedPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P)
        {
            LicensedWeapon weapon = Configuration.Instance.Licensed.FirstOrDefault(x => x.Id == P.item.id);

            if (weapon != null && Configuration.Instance.WeaponLicense)
            {
                if (player.HasPermission(Configuration.Instance.WeaponLicensePermission))
                {
                    return;
                }

                if (player.IsAdmin)
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

                player.Inventory.askDropItem(player.CSteamID, (byte)inventoryGroup, P.x, P.y);
                UnturnedChat.Say(player, Translate("no_weapon_license"));
            }
        }
        #endregion

        #region Vehicle License
        private void OnPlayerUpdateStance(UnturnedPlayer player, byte stance)
        {
            if (stance == 6 && Configuration.Instance.VehicleLicense)
            {

                if (player.IsAdmin)
                {
                    return;
                }

                if (player.HasPermission(Instance.Configuration.Instance.VehicleLicensePermission))
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

                player.CurrentVehicle.getExit(0, out var exitPoint, out var exitAngle);
                VehicleManager.sendExitVehicle(player.CurrentVehicle, 0, exitPoint, exitAngle, false);
                UnturnedChat.Say(player, Translate("no_vehicle_license"));
            }
        }

        #endregion

        #region Translate
        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                { "no_weapon_license", "You do not have a license to use this weapon!" },
                { "no_vehicle_license", "You do not have a license to drive this vehicle!" }
            };
        #endregion
    }
}
