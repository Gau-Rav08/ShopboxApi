using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ShopboxApi.Dtos;
using ShopboxApi.Models;
using ShopboxApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopboxApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly UserRepository _user;
        private readonly ProductRepository _product;

        public ApiController()
        {
            _user = new UserRepository();
            _product = new ProductRepository();
        }

        [HttpPost("userData")]
        public async Task<User> UserData(GetUserByIdDto userDto)
        {
            var user = await _user.GetUserById(userDto.Id);
            return user;
        }

        [HttpPost("userSave")]
        public async Task<User> UserSave(GetUserDataDto userDto)
        {
            User userData = new()
            {
                Id = userDto.Id,
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                Phone = userDto.Phone,
                Address = userDto.Address
            };
            var user = await _user.UpdateUser(userData);
            return user;
        }

        [HttpGet("newproducts")]
        public async Task<IEnumerable<Product>> NewProducts()
        {
            var products = await _product.GetNewProduct();
            return products;
        }

        [HttpPost("signup")]
        public async Task<ActionResult> SignUp(SignupDto signupDto)
        {
            User user = new()
            {
                Name = signupDto.Name,
                Email = signupDto.Email,
                Password = signupDto.Password
            };

            ForLogin data = new();
            data.Success = await _user.CreateUser(user);
            if (!data.Success) { data.Message = "Account with that email is already exists"; }
            return Ok(data);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {

            var user = await _user.GetUserWhenLogin(loginDto.Email, loginDto.Password);
            ForLogin data = new();
            if (user != null)
            {               
                data.User = user;
                data.Success = true;
                return Ok(data);
            }
            else
            {
                data.Success = false;
                data.Message = "Incorrect Email or Password.";
                return Ok(data);
            }
                        
        }

        [HttpPost("product")]
        public async Task<Product> GetProduct(GetProdById prodDto)
        {
            var prod = await _product.GetProductById(prodDto.Id);
            return prod;
        }

        [HttpPost("filter")]
        public async Task<IEnumerable<Product>> FilterProducts(GetProdByBrandDto prod)
        {
            var products = await _product.GetProductFromBrand(prod.Brand);
            return products;
        }

        [HttpPost("addToCart")]
        public async Task AddCartProduct(GetUserProdDto userData)
        {
            var user = await _user.GetUserById(userData.UserId);
            var cart = user.Cart;
            var exists = false;
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].ProductId == userData.ProdId)
                {
                    cart[i].Quantity++;
                    exists = true;
                    user.Cart = cart;
                    await _user.FindAndUpdateCart(user);
                }
            }
            if(!exists)
            {
                user.Cart = cart;
                await _user.FindAndUpdateCart(user);
            }
        }

        [HttpPost("removeFromCart")]
        public async Task RemoveCartProduct(GetUserProdDto userData)
        {
            var user = await _user.GetUserById(userData.UserId);
            var cart = user.Cart;
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].ProductId == userData.ProdId)
                {
                    cart[i].Quantity--;
                    user.Cart = cart;
                    await _user.FindAndUpdateCart(user);
                }
            }
            
        }

        [HttpPost("cart")]
        public async Task<ForCart> GetCart(GetUserByIdDto userData)
        {
            var user = await _user.GetUserById(userData.Id);
            var cart = user.Cart;
            List<ForCartList> cartItems = new();
            long  total = 0;
            for (var i = 0; i < cart.Count; i++)
            {
                var item = await _product.GetProductById(cart[i].ProductId);
                var subtotal = item.Price * cart[i].Quantity;
                ForCartList toAdd = new()
                {
                    Items = item, SubTot = subtotal, Quantity = cart[i].Quantity
                };
                cartItems.Add(toAdd);
                total += subtotal;  
            }
            ForCart data = new() { Items = cartItems, Tot = total };

            return data;
        }




    }

    public class ForLogin
    {
        public bool Success { get; set; }
        public User User { get; set; }

        public string Message { get; set; }
    }

    public class ForCart
    {
        public List<ForCartList> Items { get; set; }
        public long Tot { get; set; }

    }

    public class ForCartList
    {
        public Product Items { get; set; }
        public int Quantity { get; set; }
        public long SubTot { get; set; }

    }
}
