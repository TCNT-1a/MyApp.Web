namespace MyApp.Web.Models.Account
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BirthYear { get; set; }
        public List<Hobby> Hobby { get; set; }
        public Place Place { get; set; }
        public Person()
        {
            Hobby = new List<Hobby>();
            Place = new Place();
        }
    } 
}
