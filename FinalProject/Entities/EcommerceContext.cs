﻿using FinalProject.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Entities
{
    public class EcommerceContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Cart> carts { get; set; }

        public DbSet<CartProduct> cartProducts { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-M0HFSPB5;Database=Ecommerce;Trusted_Connection=True;Encrypt=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   //composite primary key
            modelBuilder.Entity<CartProduct>()
                .HasKey(cp => new { cp.ProductId, cp.CartId });

            //composite primary key
            modelBuilder.Entity<OrderItem>()
                .HasKey(cp => new { cp.OrderId, cp.ProductId });
        }
    }
}
