using FIrstDiscordBotC_.DATA.Models;
using FIrstDiscordBotC_.DATA.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.DATA
{
    public class DataContext:DbContext
    {
        private const string CONNECTION_NAME="Default";
        public DataContext():base(CONNECTION_NAME)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Server> Servers { get; set; }
    }
}
