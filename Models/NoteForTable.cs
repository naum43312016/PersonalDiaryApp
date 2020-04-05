using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalDiaryApp.Models
{
    public class NoteForTable
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }

    }
}
