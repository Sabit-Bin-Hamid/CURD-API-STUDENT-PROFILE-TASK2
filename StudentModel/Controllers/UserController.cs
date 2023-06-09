﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using StudentModel.Models;
using StudentModel.Models.Data;
using Microsoft.EntityFrameworkCore;


namespace StudentModel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserApiDbContext _db;

        public UserController(UserApiDbContext db)
        {
            this._db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var getResult = await _db.Users.ToListAsync();
            return Ok(getResult);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserRequest addUser)
        {
            try
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = addUser.UserName,
                    Email = addUser.Email,
                    Password = addUser.Password,
                    UserFirstName = addUser.UserFirstName,
                    UserLastName = addUser.UserLastName,
                    Phone = addUser.Phone,
                    Address = addUser.Address,
                    Role = addUser.Role,
                };

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                return Ok(user);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            try
            {
                var user = await _db.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch { throw; }

        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetUserByName([FromRoute] string name)
        {
            try
            {
                var user = await _db.Users.Where(x => x.UserName.Contains(name)).ToListAsync();
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            } catch { throw; }
        }






        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, UpdateUserRequest update)
        {
            var user = _db.Users.Find(id);

            if (user != null) {
                user.Role = update.Role;
                user.Address = update.Address;
                user.Email = update.Email;
                user.Password = update.Password;
                user.UserLastName = update.UserLastName;
                user.Phone = update.Phone;
                user.UserFirstName = update.UserFirstName;
                user.UserName = update.UserName;


                await _db.SaveChangesAsync();
                return Ok(user);
            }
            return NotFound();

        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteUserById([FromRoute] Guid id)
        {
            try
            {
                var user = await _db.Users.FindAsync(id);
                if (user != null)
                {
                    _db.Remove(user);
                    await _db.SaveChangesAsync();
                    return Ok("User Delete SuccessFully! " + user);
                }

                return NotFound();
            }
            catch { throw; }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllUser()
        {
            try
            {
                var user = await _db.Users.ToListAsync();
                if (user != null)
                {
                    _db.Users.RemoveRange(user);
                    await _db.SaveChangesAsync();
                    return Ok(user);
                }
                return NotFound();
            }
            catch { throw; }
        }

        [HttpGet]
        [Route("name/{name}/phone/{phone}")]
        public async Task<IEnumerable<User>> GetUsersByNameAndPhone(string name,string phone) 
        {
            try
            {
                var query = _db.Users.AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(n => n.UserName.Contains(name));
                }

                if (!string.IsNullOrEmpty(phone))
                {
                    query = query.Where(p => p.Phone.Contains(phone));
                }
                return await query.ToListAsync();
            }
            catch { throw; }
        }

        [HttpGet]
        [Route("role/{role}")]
        public async Task<IActionResult> GetUserByRole([FromRoute] string role)
        {
            try
            {
                var query = _db.Users.AsQueryable();
                if (!string.IsNullOrEmpty(role))
                {
                    query = query.Where(r => r.Role == role);

                }
                return Ok(await query.ToListAsync());

            }
            catch { throw; }
        }
    }
}

