using AutoServiceHelper.Infrastructure.Data.Constants;
using AutoServiceHelper.Infrastructure.Data.Models;

namespace AutoServiceHelper.Core.Models.Cars
{
    public class ViewIssueModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public TypeActivity Type { get; set; }

        public string SubmitetByUserId { get; set; }

        public DateTime SubmitionDate { get; set; }

        public string CarId { get; set; }
        public CarViewModel Car { get; set; }

        public int CarOdometer { get; set; }

        public bool IsFixed { get; set; }

        public IssueStatus Status { get; set; }

        public string Description { get; set; }


        //  public IList<Offer> Offers { get; set; } = new List<Offer>();
    }
}

