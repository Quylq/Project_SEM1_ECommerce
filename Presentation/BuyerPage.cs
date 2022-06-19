using BL;
using Persistence;

namespace Presentation
{
    class BuyerPage
    {
        public void Program()
        {
            Console.WriteLine("===========================================");
            Console.WriteLine();
            Console.WriteLine("  ---  ECommerce  ---  ");
            Console.WriteLine();
            Console.WriteLine("  STUDENTS LIST MANAGEMENT");
            Console.WriteLine("===========================================");
            Console.WriteLine();
            Console.WriteLine("   1.  SEARCH PRODUCTS");
            Console.WriteLine("   2.  CATEGORY MANAGEMENT");
            Console.WriteLine("   3.  CART");
            Console.WriteLine("   4.  MY PURCHASES");
            Console.WriteLine("   5.  EXIT");
            Console.WriteLine("===========================================");
            try
            {
                Console.Write("#YOUR CHOICE: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        
                        Program();
                        break;
                    case 2:
                        
                        Program();
                        break;
                    case 3:
                        
                        Program();
                        break;
                    case 4:
                        break;
                    default:
                        Console.WriteLine("===========================================");
                        Console.WriteLine("Choice 1-4");
                        Program();
                        break;
                }
            }
            catch
            {
                Console.WriteLine("===========================================");
                Console.WriteLine("Choice 1-4");
                Program();
            }
        }
    }
}