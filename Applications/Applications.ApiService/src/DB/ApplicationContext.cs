using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.ApiService.src.DB
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class Transaction
    {
        public int TransactionId { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public decimal Money { get; set; }
        public string Description { get; set; } = string.Empty;

    }

    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            try
            {
                Database.EnsureCreated();
            }
            catch
            {

            }
        }

        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Account> Accounts => Set<Account>();
    }
}
