using AutoServiceHelper.Core.Models.Offers;

namespace AutoServiceHelper.Core.Contracts
{
    public interface IInformationServices
    {
        public Task<IEnumerable<ServiceViewModel>> GetServicesForOffer(string offerId);

        public Task<IEnumerable<PartsViewModel>> GetPartsForService(string serviceId);
        public Task<string> GetCarIdByOfferId(string offerId);
    }
}
