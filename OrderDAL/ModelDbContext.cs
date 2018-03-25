using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderDAL
{
    public class ModelDbContext : DbContext
    {
        public ModelDbContext(DbContextOptions<ModelDbContext> options)
           : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=OrderManagement;Data Source=DESKTOP-19RK5VV\SQLEXPRESS");
            }

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
    }

    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string Address { get; set; }

        [StringLength(20, MinimumLength = 2)]
        public string City { get; set; }

        [StringLength(20, MinimumLength = 2)]
        public string State { get; set; }

        [StringLength(20, MinimumLength = 2)]
        public string Country { get; set; }

        [Required]
        [StringLength(450)]
        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        public List<Order> Orders { get; set; }
    }
    
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string ShippingAddress { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string City { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        [DisplayName("State / Province")]
        public string State { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Country { get; set; }

        [Required]
        [DefaultValue(0)]
        public int OrderStatus { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int OrderItemId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public decimal Price { get; set; }
    }

    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(5000, MinimumLength = 2)]
        public string Discription { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [DefaultValue("true")]
        public bool Active { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }

    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public List<Product> Products { get; set; }
    }
    
}
