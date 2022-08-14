using DAL;
using Persistence;

namespace BL;
public class BLHelper
{
    private ShopDAL shopDAL;
    private ProductDAL productDAL;
    private OrderDetailsDAL orderDetailsDAL;
    public BLHelper()
    {
        shopDAL = new ShopDAL();
        productDAL = new ProductDAL();
        orderDetailsDAL = new OrderDetailsDAL();
    }
    public int GetProductNumberOfCart(int _UserID)
    {
        return orderDetailsDAL.GetProductNumberOfCart(_UserID);
    }
    public Shop? GetShopByUserID(int _UserID)
    {
        return shopDAL.GetShopByUserID(_UserID);
    }
    public long GetTotalOrderDetails(OrderDetails orderDetails)
    {
        long total;
        Product product = productDAL.GetProductByID(orderDetails.ProductID);
        total = (long)orderDetails.Quantity * product.Price;
        return total;
    }
    public long GetTotalOrder(int _OrderID)
    {
        long total = 0;
        List<OrderDetails>? orderDetailsList = orderDetailsDAL.GetOrderDetailsListByOrderID(_OrderID);
        if (orderDetailsList != null)
        {
            foreach (OrderDetails orderDetails in orderDetailsList)
            {
                total += GetTotalOrderDetails(orderDetails);
            }
        }
        return total;
    }
    public long GetTotalCart(int _UserID)
    {
        long total = 0;
        List<OrderDetails>? orderDetailsList = orderDetailsDAL.GetOrderDetailsListByUserIDAndStatus(_UserID, "Shopping");
        if (orderDetailsList != null)
        {
            foreach (OrderDetails orderDetails in orderDetailsList)
            {
                total += GetTotalOrderDetails(orderDetails);
            }
        }
        return total;
    }
}