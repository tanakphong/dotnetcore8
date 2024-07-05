using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
namespace DotnetStockAPI.Controllers;

// [Authorize]
// [Authorize(Roles = UserRolesModel.Admin + "," + UserRolesModel.Manager)]
[ApiController]
[Route("api/[controller]")]
[EnableCors("MultipleOrigins")]
public class CategoryController : ControllerBase
{
    // สร้าง Object ของ ApplicationDbContext
    private readonly ApplicationDbContext _context;
    // ฟังก์ชันสร้าง Constructor รับค่า ApplicationDbContext
    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // CRUD Category
    // ฟังก์ชันสำหรับการดึงข้อมูล Category ทั้งหมด
    [HttpGet]
    public ActionResult<category> GetCategories()
    {
        // LINQ stand for "Language Integrated Query"
        var categories = _context.categories.ToList();
        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(categories);
    }

    // ฟังก์ชันสำหรับการดึงข้อมูล Category ตาม ID
    // GET /api/Category/1
    [HttpGet("{id}")]
    public ActionResult<category> GetCategory(int id)
    {
        // LINQ สำหรับการดึงข้อมูลจากตาราง Categories ตาม ID
        var category = _context.categories.Find(id); // select * from category where id = 1
        // ถ้าไม่พบข้อมูล
        if(category == null)
        {
            return NotFound();
        }
        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(category);
    }

    // ฟังก์ชันสำหรับการเพิ่มข้อมูล Category
    // POST /api/Category
    [HttpPost]
    public ActionResult<category> AddCategory([FromBody] category category)
    {
       // เพิ่มข้อมูลลงในตาราง Categories
        _context.categories.Add(category); // insert into category values (...)
        _context.SaveChanges(); // commit
        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(category);
    }

       // ฟังก์ชันสำหรับการแก้ไขข้อมูล Category
    // PUT /api/Category/1
    [HttpPut("{id}")]
    public ActionResult<category> UpdateCategory(int id, [FromBody] category category)
    {
        // ค้นหาข้อมูล Category ตาม ID
        var cat = _context.categories.Find(id); // select * from category where id = 1
        // ถ้าไม่พบข้อมูลให้ return NotFound
        if(cat == null)
        {
            return NotFound();
        }
        // แก้ไขข้อมูล Category
        cat.categoryname = category.categoryname; // update category set categoryname = '...' where id = 1
        cat.categorystatus = category.categorystatus; // update category set categorystatus = '...' where id = 1
        // commit
        _context.SaveChanges();
        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(cat);
    }

    // ฟังก์ชันสำหรับการลบข้อมูล Category
    // DELETE /api/Category/1
    [HttpDelete("{id}")]
    public ActionResult<category> DeleteCategory(int id)
    {
        // ค้นหาข้อมูล Category ตาม ID
        var cat = _context.categories.Find(id); // select * from category where id = 1
        // ถ้าไม่พบข้อมูลให้ return NotFound
        if(cat == null)
        {
            return NotFound();
        }
        // ลบข้อมูล Category
        _context.categories.Remove(cat); // delete from category where id = 1
        _context.SaveChanges(); // commit
        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(cat);
    }
}