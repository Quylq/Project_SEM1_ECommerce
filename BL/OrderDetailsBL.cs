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
    public void UpdateProductNumberOfOrderDetails(OrderDetails orderDetails)
    {
        orderDetailsDAL.UpdateProductNumberOfOrderDetails(orderDetails);
    }
    public int GetTotalOrderDetails(OrderDetails orderDetails)
    {
        int total;
        ProductBL productBL = new ProductBL();
        Product product = productBL.GetProductByID(orderDetails.ProductID);
        total = orderDetails.ProductNumber * product.Price;
        return total;
    }
}