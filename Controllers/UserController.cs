using AutoMapper;
using AutoMapper.Configuration;
using Project.Entities;
using Project.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Project.DAL;
using Microsoft.Extensions.Logging;
using SpeedyStayBLL.Repository;
using System.Collections.Generic;
using System.IO;
using Project.FileUploadService;
using Project.Models.DTO;

namespace Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        ApplicationDbContext _context { get; }

        private readonly IConfiguration configuration;

        IMapper _mapper;

        private readonly ILogger<UserController> _logger;
        public IMapper Mapper => _mapper;
        public IGenericRepository<Users> _userRepo { get; }

        private readonly IFileUploadService _uploadService;
        UserController(ApplicationDbContext faceBookDbContext,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<UserController> logger,
             IFileUploadService service)
        {
            this.configuration = configuration;
            _mapper = mapper;
            _context = faceBookDbContext;
            _logger = logger;
            _uploadService = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDTO Userinfo)
        {
            var MyAddedUser = _mapper.Map<Users>(Userinfo);
            await _userRepo.AddAsync(MyAddedUser);
            return Ok(MyAddedUser);
        }

        [HttpPost]  
        public async Task<IActionResult> UpdateUser(Users editedUser)
        {
            var user = await _context.Users.FindAsync(editedUser.UserId);
            if (user != null) 
            {
                user.Username = editedUser.Username;
                user.Password = editedUser.Password;
                user.Email = editedUser.Email;
            }
            await _context.Users.AddRangeAsync(user);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created,user.Username);
        }

        [HttpDelete]
         public async Task<IActionResult> DeleteUser(int id)
         {
            var user = _context.Users.FirstOrDefault(m=>m.UserId==id);
            _context.Users.Remove(user);
            return StatusCode(StatusCodes.Status200OK);
         }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var myList = _mapper.Map<List<Users>>(await _userRepo.GetAll());
            return StatusCode(StatusCodes.Status200OK, myList);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int Id)
        {
            var user = _userRepo.GetById(Id);
            if (user == null)
            {
                return BadRequest();
            }
            return StatusCode(StatusCodes.Status202Accepted, user);
        }
        [HttpPost]
        public async Task<IActionResult> LoginWithToken(Users user)
        {
            var User = await _context.Users.FindAsync(user.Email);
            if (user != null)
            {
                if (user.Password == User.Password)
                {
                    var token = new JWTHelper().Create((Microsoft.Extensions.Configuration.IConfiguration)configuration);
                    User.RefreshToken = token.RefreshToken;
                    var UsersAccessToken = token.AccessToken;
                    await _context.SaveChangesAsync();
                }
            }
            return Ok(user);
        }

        [HttpPost]
        public async void Post(IFormFile file)
        {
            Users user = new Users();
            user.FilePath = await _uploadService.UploadFileAsync(file);
            FileInfo fileInfo = new FileInfo(file.FileName);
            var filelenght = fileInfo.Length;
            var fileType = fileInfo.GetType();
            var fileName = fileInfo.Name;
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(string RefreshToken)
        {
            var User = _context.Users.Where(m => m.RefreshToken == RefreshToken);
            var Token = new JWTHelper().Create((Microsoft.Extensions.Configuration.IConfiguration)configuration);
            return Ok(Token);
        }
    }
}
