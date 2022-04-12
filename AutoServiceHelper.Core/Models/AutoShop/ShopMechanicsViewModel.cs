namespace AutoServiceHelper.Core.Models.AutoShop
{
    public class ShopMechanicsViewModel
    {
        public string Id { get; set; }

        public string Row { get; set; }

        public string Name { get; set; }

        public bool WorkForShop { get; set; } = false;

        public string City { get; set; }

        public string Email { get; set; } 

        public string PhoneNumber { get; set; }

        public List<string> Activitys { get; set; }
    }
}
