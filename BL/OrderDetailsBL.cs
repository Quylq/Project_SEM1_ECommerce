using DAL;
using Persistence;

namespace BL;
public class OrderDetailsBL
{
    private OrderDetailsDAL orderDetailsDAL;

    public OrderDetailsBL()
    {
        orderDetailsDAL = new OrderDetailsDAL();
    }
    public List<OrderDetails> GetOrderDetailsListByOrderID(int _OrderID)
    {
        List<OrderDetails> orders = orderDetailsDAL.GetOrderDetailsListByOrderID(_OrderID);

        return orders;
    }
    public void InsertOrderDetails(OrderDetails orderDetails)
    {
        orderDetailsDAL.InsertOrderDetails(orderDetails);
    }
    public void UpdateQuantityOfOrderDetails(OrderDetails orderDetails)
    {
        orderDetailsDAL.UpdateQuantityOfOrderDetails(orderDetails);
        orderDetailsDAL.DeleteOrderDetails();
    }
    public long GetTotalOrderDetails(OrderDetails orderDetails)
    {
        long total;
        ProductBL productBL = new ProductBL();
        Product product = productBL.GetProductByID(orderDetails.ProductID);
        total = (long)orderDetails.Quantity * product.Price;
        return total;
    }
    public List<OrderDetails> GetOrderDetailsListByUserIDAndStatus(int _UserID, string _Status)
    {
        List<OrderDetails> orderDetailsList = orderDetailsDAL.GetOrderDetailsListByUserIDAndStatus(_UserID, _Status);

        return orderDetailsList;
    }
}