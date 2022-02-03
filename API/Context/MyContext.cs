using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveEmployee> LeaveEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Employees)
                .WithOne(m => m.Manager);

            modelBuilder.Entity<Employee>()
                .HasMany(d => d.Departments)
                .WithOne(e => e.Manager);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Employees)
                .WithOne(d => d.Department);

            modelBuilder.Entity<Employee>()
                .HasMany(le => le.LeaveEmployees)
                .WithOne(e => e.Employee);

            modelBuilder.Entity<Leave>()
                .HasMany(le => le.LeaveEmployees)
                .WithOne(l => l.Leave);

            modelBuilder.Entity<Employee>()
                .HasOne(acc => acc.Account)
                .WithOne(e => e.Employee)
                .HasForeignKey<Account>(ac => ac.NIK);

            modelBuilder.Entity<Account>()
                .HasMany(ar => ar.AccountRoles)
                .WithOne(a => a.Account);

            modelBuilder.Entity<Role>()
                .HasMany(ar => ar.AccountRoles)
                .WithOne(r => r.Role);

            //modelBuilder.Entity<Leave>()
            //    .Property(l => l.Id)
            //    .ValueGeneratedNever();

            //modelBuilder.Entity<LeaveEmployee>()
            //    .Property(le => le.Id)
            //    .ValueGeneratedNever();

            //modelBuilder.Entity<AccountRole>()
            //    .Property(ar => ar.Id)
            //    .ValueGeneratedNever();
            
            //modelBuilder.Entity<Role>()
            //    .Property(r => r.Id)
            //    .ValueGeneratedNever();

        }
    }
}
