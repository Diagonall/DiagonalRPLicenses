using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Diagonal.RPLicenses
{
    public class RPLicensesConfiguration : IRocketPluginConfiguration
    {
        [XmlArrayItem(ElementName = "WeaponID")]
        public List<LicensedWeapon> Licensed;

        public bool IgnoreAdmin;
        public bool LicensesMessage;
        public bool DisableLicensesOnGroupOnline;
        public string DisableLicensesPermission;
        public bool WeaponLicense;
        public bool DontLetGetWeapon;
        public int WeaponLicenseID;
        public string WeaponLicensePermission;
        public bool VehicleLicense;
        public bool DontLetInVehicle;
        public int VehicleLicenseID;
        public string VehicleLicensePermission;

        public void LoadDefaults()
        {
            Licensed = new List<LicensedWeapon>
            {
                new LicensedWeapon { Id = 122 },
                new LicensedWeapon { Id = 363 },
                new LicensedWeapon { Id = 297 }
            };

            IgnoreAdmin = true;
            LicensesMessage = true;
            DisableLicensesOnGroupOnline = true;
            DisableLicensesPermission = "license.disable";
            WeaponLicense = true;
            DontLetGetWeapon = true;
            WeaponLicenseID = 42504;
            WeaponLicensePermission = "license.weapon";
            VehicleLicense = true;
            DontLetInVehicle = true;
            VehicleLicenseID = 42503;
            VehicleLicensePermission = "license.vehicle";
        }
    }
}
