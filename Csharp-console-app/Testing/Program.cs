namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string s = "qwe,,,ert,t,,,";

            string[] stringed = s.Split(',', StringSplitOptions.RemoveEmptyEntries);


            Console.WriteLine(stringed.Length);
        }
    }
}