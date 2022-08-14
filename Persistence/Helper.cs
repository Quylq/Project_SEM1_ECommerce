using Persistence;

namespace DAL
{
    public static class Helper
    {
        public static bool DeepEquals(this User user, User another)
        {
            if (ReferenceEquals(user, another))
            {
                return true;
            }
            if ((user == null) || (another == null))
            {
                return false;
            }  
            return user.UserID.Equals(another.UserID)
            && user.UserName.Equals(another.UserName)
            && user.Password.Equals(another.Password)
            && user.FullName.Equals(another.FullName)
            && user.Birthday.Equals(another.Birthday)
            && user.Email.Equals(another.Email)
            && user.Phone.Equals(another.Phone)
            && user.AddressID.Equals(another.AddressID)
            && user.Role.Equals(another.Role);
        }
        public static bool DeepEquals1(this object obj, object another)
        {
        
            if (ReferenceEquals(obj, another))
            {
                return true;
            } 
            else if ((obj == null) || (another == null))
            {
                return false;
            }
            else if (obj.GetType() != another.GetType())
            {
                return false;
            } 
            else
            {
                var result = true;
                foreach (var property in obj.GetType().GetProperties())
                {
                    var objValue = property.GetValue(obj);
                    var anotherValue = property.GetValue(another);
                    if (!objValue!.Equals(anotherValue))
                    {
                        result = false;
                    } 
                }
                return result;
            }
        }
        /* public static bool DeepEquals2(this object obj, object another)
        {
            if (ReferenceEquals(obj, another))
            {
                return true;
            } 
            else if ((obj == null) || (another == null))
            {
                return false;
            } 
            else if (obj.GetType() != another.GetType())
            {
                return false;
            }
            else if (!obj.GetType().IsClass)
            {
                return obj.Equals(another);
            } 
            else
            {
                var result = true;
                foreach (var property in obj.GetType().GetProperties())
                {
                    var objValue = property.GetValue(obj);
                    var anotherValue = property.GetValue(another);
                    if (!objValue.DeepEquals2(anotherValue))
                    {
                        result = false;
                        break;
                    }   
                }
                return result;
            }
        } */
        public static bool DeepEquals<T>(this IEnumerable<T> obj, IEnumerable<T> another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            
            bool result = true;
            //Duyệt từng phần tử trong 2 list đưa vào
            using (IEnumerator<T> enumerator1 = obj.GetEnumerator())
            using (IEnumerator<T> enumerator2 = another.GetEnumerator())
            {
            while (true)
            {
                bool hasNext1 = enumerator1.MoveNext();
                bool hasNext2 = enumerator2.MoveNext();
            
                //Nếu có 1 list hết, hoặc 2 phần tử khác nhau, thoát khoải vòng lặp
                if (hasNext1 != hasNext2 || !enumerator1.Current!.DeepEquals1(enumerator2.Current!))
                {
                    result = false;
                    break;
                }
            
                //Dừng vòng lặp khi 2 list đều hết
                if (!hasNext1) break;
            }
            }
            
            return result;
        }
    }
}