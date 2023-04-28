using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class HeaderViewModel
    {
        public string Username { get; set; } = string.Empty;

        public long UserId { get; set; } = 0;

        public string Avtar { get; set; } = String.Empty;

        public string Email { get; set; } = String.Empty;

        public string Role { get; set; } = String.Empty;

        public bool? Status { get; set; } = true;
    }
}
