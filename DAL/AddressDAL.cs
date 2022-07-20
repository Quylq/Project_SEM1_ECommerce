using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class AddressDAL
    {
        private string? query;
        private MySqlDataReader? reader;
        private Address GetAddressInfo(MySqlDataReader reader)
        {
            Address address = new Address();

            address.AddressID = reader.GetInt32("AddressID");
            address.City = reader.GetString("City");
            address.District = reader.GetString("District");
            address.Commune = reader.GetString("Commune");
            address.SpecificAddress = reader.GetString("Address");
            return address;
        }
        public Address GetAddressByID(int _AddressID)
        {
            query = $"select * from Address where AddressID = '{_AddressID}'";

            DbHelper.OpenConnection();
            reader = DbHelper.ExecQuery(query);

            Address address = new Address();
            if (reader.Read())
            {
                address = GetAddressInfo(reader);
            }
            DbHelper.CloseConnection();
            return address;
        }
    }
}
