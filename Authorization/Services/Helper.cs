using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authorization.Models;

namespace Authorization.Services
{
    internal class Helper
    {
        private static construction_organizationEntities _context;

        public static construction_organizationEntities GetContext()
        {
            if (_context == null)
            {
                _context = new construction_organizationEntities();
            }
            return _context;
        }
    }
}
