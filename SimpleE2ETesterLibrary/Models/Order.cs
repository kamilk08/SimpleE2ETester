namespace SimpleE2ETesterLibrary.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Order(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static readonly Order Sequential = new Order(1, "Sequential");
        public static readonly Order Random = new Order(2, "Random");

    }
}