using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalDiaryApp.Data;
using PersonalDiaryApp.Models;

namespace PersonalDiaryApp.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public NoteController(ApplicationDbContext db,UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(string text)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Note note = new Note();
            note.Text = text;
            note.UserId = userId;
            note.Date = DateTime.Now;
            _db.Note.Add(note);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index","App");
        }

        public IActionResult Read(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var note = _db.Note.FirstOrDefault(i => i.Id == id);
            if (note == null)
            {
                return RedirectToAction("Index", "App");
            }
            if (!note.UserId.Equals(userId))
            {
                return RedirectToAction("Index", "App");
            }
            return View(note);
        }
        public IActionResult Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var note = _db.Note.FirstOrDefault(i => i.Id == id);
            if (note == null)
            {
                return RedirectToAction("Index", "App");
            }
            if (!note.UserId.Equals(userId))
            {
                return RedirectToAction("Index", "App");
            }
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditNote(Note EditNote)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var note = _db.Note.FirstOrDefault(i => i.Id == EditNote.Id);
            if (note == null)
            {
                return RedirectToAction("Index", "App");
            }
            if (!note.UserId.Equals(userId))
            {
                return RedirectToAction("Index", "App");
            }
            note.Text = EditNote.Text;
            _db.SaveChanges();
            return RedirectToAction("Index", "App");
        }
        public IActionResult Remove(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var note = _db.Note.FirstOrDefault(i => i.Id == id);
            if (note == null)
            {
                return RedirectToAction("Index", "App");
            }
            if (!note.UserId.Equals(userId))
            {
                return RedirectToAction("Index", "App");
            }
            _db.Note.Remove(note);
            _db.SaveChanges();
            return RedirectToAction("Index", "App");
        }
    }
}