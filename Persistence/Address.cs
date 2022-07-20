namespace Persistence
{
    public class Address
    {
        public int AddressID{get;set;}
        public string? City{get;set;}
        public string? District{get;set;}
        public string? Commune{get;set;} 
        public string? SpecificAddress{get;set;}
        public Address(int _AddressID, string _City, string _District, string _Commune, string _SpecificAddress)
        {
            AddressID = _AddressID;
            City = _City;
            District = _District;
            Commune = _Commune;
            SpecificAddress = _SpecificAddress;
        }
        public Address(string _City, string _District, string _Commune, string _SpecificAddress)
        {
            City = _City;
            District = _District;
            Commune = _Commune;
            SpecificAddress = _SpecificAddress;
        }
        public Address(){}
    }
}