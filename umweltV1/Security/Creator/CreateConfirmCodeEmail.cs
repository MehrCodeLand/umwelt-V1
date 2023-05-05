namespace umweltV1.Security.Creator
{
    public class CreateConfirmCodeEmail
    {
        public static string ConfirmCode()
        {
            var rnd = new Random();
            string ConfirmCode = rnd.Next(123, 99999).ToString();
            for(int i = 0; i < 5; ++i)
            {
                ConfirmCode += char.ConvertFromUtf32(rnd.Next(66, 80));
            }

            return ConfirmCode;
        }
    }
}
