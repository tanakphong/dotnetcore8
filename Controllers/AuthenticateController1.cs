using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotnetStockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticateController1 : ControllerBase
{
    // สร้าง Object ของ ApplicationDbContext
    private readonly ApplicationDbContext _context;

    // สร้าง Oject จัดการ Users
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    // ฟังก์ชันสร้าง Constructor สำหรับ initial ค่าของ ApplicationDbContext
    public AuthenticateController1(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration
    )
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    // ทดสอบเขียนฟังก์ชันการเชื่อมต่อ database
    [HttpGet("testconnectdb")]
    public void TestConnection()
    {
        if (_context.Database.CanConnect())
        {
            // ถ้าเชื่อมต่อได้จะแสดงข้อความ "Connected"
            Response.WriteAsync("Connected");
        }
        else
        {
            // ถ้าเชื่อมต่อไม่ได้จะแสดงข้อความ "Not Connected"
            Response.WriteAsync("Not Connected");
        }
    }

    // Register for User
    // Post api/authenticate/register-user
    [HttpPost]
    [Route("register-user")]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterModel model)
    {
        // เช็คว่า username ซ้ำหรือไม่
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "User already exists!"
                }
            );
        }

        // เช็คว่า email ซ้ำหรือไม่
        userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "Email already exists!"
                }
            );
        }

        // สร้าง User
        IdentityUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };

        // สร้าง User ในระบบ
        var result = await _userManager.CreateAsync(user, model.Password);

        // ถ้าสร้างไม่สำเร็จ
        if (!result.Succeeded)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "User creation failed! Please check user details and try again."
                }
            );
        }

        // กำหนด Roles Admin, Manager, User
        if (!await _roleManager.RoleExistsAsync(UserRolesModel.Admin))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Admin));
        }

        if (!await _roleManager.RoleExistsAsync(UserRolesModel.Manager))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Manager));
        }

        if (await _roleManager.RoleExistsAsync(UserRolesModel.User))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.User));
            await _userManager.AddToRoleAsync(user, UserRolesModel.User);
        }

        return Ok(new ResponseModel
        {
            Status = "Success",
            Message = "User registered successfully"
        });
    }



}