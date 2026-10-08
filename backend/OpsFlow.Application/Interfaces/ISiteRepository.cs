using OpsFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpsFlow.Application.Interfaces
{
    public interface ISiteRepository
    {

        Task<Site?> GetByIdAsync(int siteId);
        Task<List<Site>> GetAllAsync();
        Task AddAsync(Site site);
        Task SaveChangesAsync();
    }
}
