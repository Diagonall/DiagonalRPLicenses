using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Diagonal.RPLicenses
{
    public class RPLicensesConfiguration : IRocketPluginConfiguration
    {
        [XmlArrayItem(ElementName = "WeaponID")]
        public List<LicensedWeapon> Licensed;

        public bool WeaponLicense;
        public bool VehicleLicense;
        public int WeaponLicenseID;
        public int VehicleLicenseID;
        public string WeaponLicensePermission;
        public string VehicleLicensePermission;

        public void LoadDefaults()
        {
            Licensed = new List<LicensedWeapon>
            {
                new LicensedWeapon { Id = 122 },
                new LicensedWeapon { Id = 363 },
                new LicensedWeapon { Id = 297 }

            };

            WeaponLicensePermission = "license.weapon";
            VehicleLicensePermission = "license.vehicle";
            WeaponLicense = true;
            VehicleLicense = true;
            WeaponLicenseID = 42504;
            VehicleLicenseID = 42503;
        }
    }
}
