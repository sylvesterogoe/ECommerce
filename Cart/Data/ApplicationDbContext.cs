﻿using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<CartItem>()
        //    .HasKey(ci => new { ci.CartItemId });

        //modelBuilder.Entity<CartItem>()
        //    .HasOne(ci => ci.Cart)
        //    .WithMany(c => c.CartItems)
        //    .HasForeignKey(ci => ci.CartId);

        //modelBuilder.Entity<CartItem>()
        //    .HasOne(ci => ci.Item)
        //    .WithMany()
        //    .HasForeignKey(ci => ci.ItemId);
    }
}