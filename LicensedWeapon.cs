using System.Xml.Serialization;

namespace Diagonal.RPLicenses
{
    public class LicensedWeapon
    {
        [XmlText]
        public ushort Id { get; set; }

        public LicensedWeapon() { }
    }
}
