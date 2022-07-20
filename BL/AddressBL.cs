using DAL;
using Persistence;

namespace BL;
public class AddressBL
{
    private AddressDAL addressDAL;

    public AddressBL()
    {
        addressDAL = new AddressDAL();
    }
    public Address GetAddressByID(int _AddressID)
    {
        Address address = addressDAL.GetAddressByID(_AddressID);

        return address;
    }
}