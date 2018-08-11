using ChatRoom.DAL;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.API
{
    public static class DataSeeder
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
           .GetRequiredService<IServiceScopeFactory>()
           .CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ChatRoomContext>();

                context.Database.Migrate();
            }
        }
    }
}
