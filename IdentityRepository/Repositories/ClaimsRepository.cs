using IdentityDomain.Entities;
using IdentityDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityRepository.Repositories
{
    internal class ClaimRepository : BaseRepository<Claim>, IClaimRepository
    {
        //passes singletone context to baserepository
        internal ClaimRepository(ApplicationDbContext context) : base(context) { }
    }
}
