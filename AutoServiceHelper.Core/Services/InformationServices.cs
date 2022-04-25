using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Models.Offers;
using AutoServiceHelper.Infrastructure.Data.Common;
using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceHelper.Core.Services
{
    
    public class InformationServices:IInformationServices
    {
        private readonly IRepository repo;
        private readonly UserManager<IdentityUser> userManager;


        public InformationServices(IRepository _repo,
            UserManager<IdentityUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;

        }
        public async Task<IEnumerable<ServiceViewModel>> GetServicesForOffer(string offerId)
        {

            var result = await repo.All<ShopService>()
                .Where(x => x.OfferId.ToString() == offerId)
                .Select(x => new ServiceViewModel()
                {
                    Name = x.Name,
                    NeededHourOfWork = x.NeededHourOfWork,
                    Type = x.Type,
                    Price = x.Price,
                    ServiceId = x.Id,
                    offerId = x.OfferId

                })
                .ToListAsync();

            foreach (var service in result)
            {
                service.Price += await repo.All<Part>()
                    .Where(x => x.ShopServiceId == service.ServiceId)
                    .Select(x => x.Price)
                    .SumAsync();
            }

            return result;
        }

        public async Task<IEnumerable<PartsViewModel>> GetPartsForService(string serviceId)
        {
            return await repo.All<Part>()
                .Where(x => x.ShopServiceId.ToString() == serviceId)
                .Select(x => new PartsViewModel
                {
                    Name = x.Name,
                    Price = x.Price,
                    Id = x.Id.ToString(),
                    Number = x.Number,
                    QuantitiNeeded = x.QuantitiNeeded

                }).ToListAsync();
        }

        public async Task<string>GetIssueIdByOfferId(string offerId)
        {
            var issueId = await repo.All<Offer>()
                .Where(x => x.Id.ToString() == offerId)
                .Select(x => x.IssueId)
                .FirstOrDefaultAsync();

            
            return issueId.ToString();
        }
    }
}
