using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalDiaryApp.Data;
using PersonalDiaryApp.Models;

namespace PersonalDiaryApp.Controllers
{
    [Authorize]
    public class AppController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public AppController(UserManager<IdentityUser> userManager,ApplicationDbContext db)
        {
            _userManager = userManager;
            _db= db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            int notePerPage = 10;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            List<Note> notesFromDb = await _db.Note.Where(n => n.UserId.Equals(user.Id))
                .OrderByDescending(d => d.Date).Skip(notePerPage*page-notePerPage).ToListAsync();
            List<NoteForTable> list = new List<NoteForTable>();
            for(int i = 0; i < notesFromDb.Count; i++)
            {
                Note n = notesFromDb[i];
                NoteForTable nt = new NoteForTable
                {
                    Id = n.Id,
                    Date = n.Date.ToString("dd/MM/yyyy"),
                    Text = n.Text,
                    UserId = n.UserId
                };
                list.Add(nt);
            }
            return View(list);
        }
    }
}