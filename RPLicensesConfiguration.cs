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
        public bool DisableLicensesOnGroupOnline;
        public bool LicensesMessage;
        public int WeaponLicenseID;
        public int VehicleLicenseID;
        public string WeaponLicensePermission;
        public string VehicleLicensePermission;
        public string DisableLicensesPermission;

        public void LoadDefaults()
        {
            Licensed = new List<LicensedWeapon>
            {
                new LicensedWeapon { Id = 122 },
                new LicensedWeapon { Id = 363 },
                new LicensedWeapon { Id = 297 }

            };
            
            LicensesMessage = true;
            DisableLicensesOnGroupOnline = true;
            DisableLicensesPermission = "license.disable";
            WeaponLicense = true;
            WeaponLicenseID = 42504;
            WeaponLicensePermission = "license.weapon";
            VehicleLicense = true;
            VehicleLicenseID = 42503;
            VehicleLicensePermission = "license.vehicle";
        }
    }
}
