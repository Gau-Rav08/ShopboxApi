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
            await _user.CreateUser(user);
            JObject data = new(new JProperty("success", "true"));

            return Ok(data);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            
            var user = await _user.GetUserByEmail(loginDto.Email);
            if (user != null)
            {
                JObject data = new(new JProperty("success", "true"), new JProperty("user", new JObject(
                new JProperty("id", user.Id),
                new JProperty("name", user.Name),
                new JProperty("email", user.Email),
                new JProperty("password", user.Password))));
                return Ok(data);
            }
            else
            {
                JObject data = new(new JProperty("success", "false"), new JProperty("message", "Incorrect Email or Password."));
                return Ok(data);
            }
                        
        }

        [HttpPost("product")]
        public async Task<Product> GetProduct(GetProdById prodDto)
        {
            var prod = await _product.GetProductById(prodDto.Id);
            return prod;
        }
    }
}
