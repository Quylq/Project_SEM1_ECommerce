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

    public void SaveOrderDetails(OrderDetails orderDetails)
    {
        orderDetailsDAL.SaveOrderDetails(orderDetails);
    }

}

