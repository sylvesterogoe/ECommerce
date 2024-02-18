using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerceTests.ServicesTests
{
    public class DBInteractionTests
    {
        [Fact]
        public async void AddUser_CreatesUserSuccessfully() 
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "TestECommerceDatabase")
           .Options;

            var user = new User
            {
                Id = 1,
                PhoneNumber = "1234567890",
            };

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.Users.Add(user);
            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options))
            {
                var userRepository = new UserRepository(context);

                //Act
                await userRepository.AddUserAsync(user);

                var createdUser = context.Users.Where(u => u.Id == user.Id).FirstOrDefault();

                //Assert
                Assert.Equal(createdUser?.Id, user?.Id);
                Assert.Equal(createdUser?.PhoneNumber, user?.PhoneNumber);
            }

        }

        [Fact]
        public void AddCart_AddsCartSuccessfully()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestECommerceDatabase")
            .Options;

            var cart = new Cart
            {
                Id = 2,
                UserId = 2,
                CartItems = new List<CartItem> { new CartItem { } }
            };

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.Carts.Add(cart);
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options))
            {
                var cartRepository = new CartRepository(context);

                //Act
                var retrievedCart = cartRepository.GetCartById(cart.Id);

                //Assert
                Assert.Equal(cart.Id, retrievedCart?.Id);
                Assert.Equal(cart.UserId, retrievedCart?.UserId);
            }            
        }

        [Fact]
        public void AddCartItemToCart_AddsCartItemSuccessfully()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestECommerceDatabase")
            .Options;

            var cartItem = new CartItem
            {
                Id = 3,
                Item = new Item
                {
                    Id = 3,
                    Name = "Table",
                    Price = 400
                },
                Quantity = 1,
                Time = new DateTime(2024,02,18)
            };

            var cart = new Cart
            {
                Id = 3,
                UserId = 3,
                CartItems = new List<CartItem> { cartItem }
            };

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.CartItems.Add(cartItem);
                context.Carts.Add(cart);
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options))
            {
                var cartRepository = new CartRepository(context);

                //Act
                var retrievedCart = cartRepository.GetCartById(cart.Id);

                //Assert
                Assert.Equal(retrievedCart?.CartItems.Where(ci => ci.Id == cartItem.Id).FirstOrDefault()?.Id, cartItem.Id);
                Assert.Equivalent(retrievedCart?.CartItems.Where(ci => ci.Id == cartItem.Id).FirstOrDefault()?.Item, cartItem.Item);
                Assert.Equal(retrievedCart?.CartItems.Where(ci => ci.Id == cartItem.Id).FirstOrDefault()?.Quantity, cartItem.Quantity);
                Assert.Equal(retrievedCart?.CartItems.Where(ci => ci.Id == cartItem.Id).FirstOrDefault()?.Time, cartItem.Time);
            }
        }

        [Fact]
        public async void AddSameCartItemToCart_IncreaseCartItemQuantitySuccessfully()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestECommerceDatabase")
            .Options;

            var cartItem1 = new CartItem
            {
                Id = 9,
                Item = new Item
                {
                    Id = 9,
                    Name = "Table",
                    Price = 400
                },
                Quantity = 1,
                Time = new DateTime(2024, 02, 18)
            };

            var cartItem2 = new CartItem
            {
                Id = 9,
                Item = new Item
                {
                    Id = 9,
                    Name = "Table",
                    Price = 400
                },
                Quantity = 2,
                Time = new DateTime(2024, 02, 19)
            };

            var cart = new Cart
            {
                Id = 9,
                UserId = 7,
                CartItems = new List<CartItem> { cartItem1, cartItem2 }
            };

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                var cartItemRepository = new CartItemRepository(context);
                await cartItemRepository.AddCartItem(cart, cartItem1);
                context.ChangeTracker.Clear();
            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options))
            {
                var cartItemRepository = new CartItemRepository(context);

                //Act
                var updatedCartItem = await cartItemRepository.UpdateCartItemQuantity(cartItem1, cartItem2.Quantity);

                //Assert
                Assert.Equal(3, updatedCartItem?.Quantity);
            }
        }

        [Fact]
        public async void RemoveCartItemFromCart_DeletesCartItemSuccessfully()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestECommerceDatabase")
            .Options;

            var cartItem = new CartItem
            {
                Id = 4,
                Item = new Item
                {
                    Id = 4,
                    Name = "Bed",
                    Price = 1500
                },
                Quantity = 1,
                Time = DateTime.Now
            };

            var cart = new Cart
            {
                Id = 4,
                UserId = 4,
                CartItems = new List<CartItem> { cartItem }
            };

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.CartItems.Add(cartItem);
                context.Carts.Add(cart);
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options))
            {
                var cartItemRepository = new CartItemRepository(context);

                //Act
                await cartItemRepository.DeleteCartItem(cartItem);
                var deletedCartItem = cartItemRepository.GetCartItem(cartItem.Id);

                //Assert
                Assert.Null(deletedCartItem);
            }
        }

        [Fact]
        public void GetCartItem_RetrievesCartItemSuccessfully()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestECommerceDatabase")
            .Options;

            var cartItem = new CartItem
            {
                Id = 5,
                Item = new Item
                {
                    Id = 5,
                    Name = "Food",
                    Price = 100
                },
                Quantity = 1,
                Time = DateTime.Now
            };

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.CartItems.Add(cartItem);
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options))
            {
                var cartItemRepository = new CartItemRepository(context);

                //Act
                var retrievedCartItem = cartItemRepository.GetCartItem(cartItem.Id);

                //Assert
                Assert.Equal(cartItem.Id, retrievedCartItem?.Id);
                Assert.Equivalent(cartItem.Item, retrievedCartItem?.Item);
                Assert.Equal(cartItem.Quantity, retrievedCartItem?.Quantity);
                Assert.Equal(cartItem.Time, retrievedCartItem?.Time);
            }
        }
    }
}
