namespace umweltV1.Security.Creator
{
    public class CreateRandomId
    {
        public static int CreateId()
        {
            var rnd = new Random();
            return rnd.Next(1000, 9999);
        }
    }
}
