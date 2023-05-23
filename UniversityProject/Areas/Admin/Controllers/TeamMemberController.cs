using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Metadata;
using UniversityProject.Areas.Admin.ViewModels;
using UniversityProject.DAL;
using UniversityProject.Models;

namespace UniversityProject.Areas.Admin.Controllers
{
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
    public class TeamMemberController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public TeamMemberController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = new TeamMemberIndexViewModel
            {

                teamMembers = await _appDbContext.teamMembers.ToListAsync()

            };
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamMembers teamMembers)
        {
            if (!ModelState.IsValid) return View(teamMembers);

            bool isExist = await _appDbContext.RelatedProducts
                                                   .AnyAsync(c => c.Title.ToLower().Trim() == teamMembers.Name.ToLower().Trim());

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu adda product artiq movcuddur");

                return View(teamMembers);
            }
            if (teamMembers.CreateDate == null)
            {
                teamMembers.CreateDate = DateTime.Today;
            }

            var teamMember = new TeamMembers
            {
                Name = teamMembers.Name,
                BookName = teamMembers.BookName,
                CreateDate = teamMembers.CreateDate,
                DeadLine = teamMembers.DeadLine,
                StudentCardId = teamMembers.StudentCardId,
                Profession = teamMembers.Profession

            };
            await _appDbContext.teamMembers.AddAsync(teamMember);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var teamMembers = await _appDbContext.teamMembers.FindAsync(id);
            if (teamMembers == null) return NotFound();

            return View(teamMembers);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, TeamMembers teamMembers)
        {
            if (!ModelState.IsValid) return View(teamMembers);

            if (id != teamMembers.Id) return BadRequest();
            var dbrelatedProducts = await _appDbContext.teamMembers.FindAsync(id);
            if (dbrelatedProducts == null) return NotFound();

            bool isExist = await _appDbContext.RelatedProducts
                .AnyAsync(rcw => rcw.Title.ToLower().Trim() == teamMembers.Name.ToLower().Trim() &&
                rcw.Id != teamMembers.Id);
            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu adda komponent movcuddur");
                return View(teamMembers);
            }
            dbrelatedProducts.Name = teamMembers.Name;
            dbrelatedProducts.StudentCardId = teamMembers.StudentCardId;
            dbrelatedProducts.BookName = teamMembers.BookName;

            dbrelatedProducts.CreateDate = teamMembers.CreateDate;
            dbrelatedProducts.DeadLine = teamMembers.DeadLine;
            dbrelatedProducts.Profession = teamMembers.Profession;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var relatedProducts = await _appDbContext.teamMembers.FindAsync(id);
            if (relatedProducts == null) return NotFound();

            return View(relatedProducts);

        }
        [HttpPost]
        public async Task<IActionResult> DeleteComponent(int id)
        {



            var dbrelatedProducts = await _appDbContext.teamMembers.FindAsync(id);
            if (dbrelatedProducts == null) return NotFound();
            

            _appDbContext.Remove(dbrelatedProducts);

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var relatedProducts = await _appDbContext.teamMembers.FindAsync(id);
            if (relatedProducts == null) return NotFound();

            return View(relatedProducts);

        }
    }
}
