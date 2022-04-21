using AutoServiceHelper.Core.Models.Offers;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IInformationServices
    {
        public Task<IEnumerable<ServiceViewModel>> GetServicesForOffer(string offerId);
    }
}
