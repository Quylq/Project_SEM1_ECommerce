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
    public List<Order> GetOrdersByStatusAndUserID(string _Status, int _UserID)
    {
        List<Order> orders = orderDAL.GetOrdersByStatusAndUserID(_Status, _UserID);
        return orders;
    }
    public List<Order> GetOrdersByStatusAndShopID(string _Status, int _ShopID)
    {
        List<Order> orders = orderDAL.GetOrdersByStatusAndShopID(_Status, _ShopID);
        return orders;
    }
    public void UpdateStatusOfOrder(int _OrderID, string _Status)
    {
        orderDAL.UpdateStatusOfOrder(_OrderID, _Status);
    }
    public void InsertOrder(Order order)
    {
        orderDAL.InsertOrder(order);
    }
    public int OrderIDMax()
    {
        int _OrderID = orderDAL.OrderIDMax();

        return _OrderID;
    }
    public Order GetOrderByID(int _OrderID)
    {
        Order order = orderDAL.GetOrderByID(_OrderID);
        return order;
    }
    public int GetTotalOrder(int _OrderID)
    {
        int total = 0;
        OrderDetailsBL orderDetailsBL = new OrderDetailsBL();
        List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByOrderID(_OrderID);
        foreach (OrderDetails orderDetails in orderDetailsList)
        {
            total += orderDetailsBL.GetTotalOrderDetails(orderDetails);
        }
        return total;
    }
    public int GetTotalCart(int _UserID)
    {
        int total = 0;
        OrderDetailsBL orderDetailsBL = new OrderDetailsBL();
        List<OrderDetails> orderDetailsList = orderDetailsBL.GetOrderDetailsListByUserIDAndStatus(_UserID, "Shopping");
        foreach (OrderDetails orderDetails in orderDetailsList)
        {
            total += orderDetailsBL.GetTotalOrderDetails(orderDetails);
        }
        return total;
    }
    public List<Order> GetOrdersByUserID(int _UserID)
    {
        List<Order> orders = orderDAL.GetOrdersByUserID(_UserID);

        return orders;
    }
}

