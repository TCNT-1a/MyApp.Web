namespace MyApp.Web.Models.Account
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Place Place { get; set; }
        public Person()
        {
            Place = new Place();
        }
    }
}
