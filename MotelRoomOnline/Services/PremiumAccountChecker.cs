using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MotelRoomOnline.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MotelRoomOnline.Services
{
    public class PremiumAccountChecker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PremiumAccountChecker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<DataContext>();
                    var expiredAccounts = _context.Accounts
                        .Where(acc => acc.ExpiryDate < DateTime.Now && acc.PremiumId > 0)
                        .ToList();

                    if (expiredAccounts.Any())
                    {
                        foreach (var account in expiredAccounts)
                        {
                            account.PremiumId = 0;
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                await Task.Delay(TimeSpan.FromHours(3), stoppingToken);
            }
        }
    }
}

