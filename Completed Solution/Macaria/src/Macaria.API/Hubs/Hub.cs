﻿using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Macaria.API.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class Hub: Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IMacariaContext _context;

        public Hub(IMacariaContext context) => _context = context;

        public async Task Send(string message) {
            
            var user = await GetUser(Context.User.Identity.Name);

            await Clients.OthersInGroup($"{user.Tenant.TenantId}").SendAsync("Send", message);
        }

        public override async Task OnConnectedAsync()
        {
            var user = await GetUser(Context.User.Identity.Name);

            await Groups.AddAsync(user.Username, $"{user.Tenant.TenantId}");

            await base.OnConnectedAsync();
        }

        public async Task<User> GetUser(string username)
            => await _context.Users.Include(x => x.Tenant)
                .FirstAsync(x => x.Username == username);   
    }
}