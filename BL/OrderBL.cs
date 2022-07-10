using DAL;
using Persistence;

namespace BL;
public class OrderBL
{
    private OrderDAL orderDAL;

    public OrderBL()
    {
        orderDAL = new OrderDAL();
    }
    public List<Order> GetOrdersByStatusOfSeller(string _Status, User seller)
    {
        List<Order> orders = orderDAL.GetOrdersByStatusOfSeller(_Status, seller);
        return orders;
    }
    public List<User> GetUsersByStatusOfSeller(string _Status, User seller)
    {
        List<User> users = orderDAL.GetUsersByStatusOfSeller(_Status, seller);
        return users;
    }
    public void SaveOrder(Order order)
    {
        orderDAL.SaveOrder(order);
    }

    public List<Product> GetOrderDetails(Order order)
    {
        List<Product> products = new List<Product>();
        products = orderDAL.GetOrderDetails(order);

        return products;
    }
    public void UpdateStatus(Order order, string _Status)
    {
        orderDAL.UpdateStatus(order, _Status);
    }
}

