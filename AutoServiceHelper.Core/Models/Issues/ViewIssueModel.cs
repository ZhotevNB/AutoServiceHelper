using AutoServiceHelper.Core.Models.Cars;
using AutoServiceHelper.Infrastructure.Data.Constants;


namespace AutoServiceHelper.Core.Models.Issues
{
    public class ViewIssueModel
    {
        public Guid Id { get; set; }

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

