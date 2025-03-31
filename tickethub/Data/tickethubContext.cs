using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketHubApi.Models;

namespace tickethub.Data
{
    public class tickethubContext : DbContext
    {
        public tickethubContext (DbContextOptions<tickethubContext> options)
            : base(options)
        {
        }

        public DbSet<TicketHubApi.Models.TicketPurchase> TicketPurchase { get; set; } = default!;
    }
}
