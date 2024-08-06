using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.DATA.Configs
{
    public static class ContextManager
    {
        private static AsyncLocal<DataContext> _context = new AsyncLocal<DataContext>();

        public static DataContext GetContext()
        {
            if (_context.Value == null)
            {
                _context.Value = new DataContext();
            }

            return _context.Value;
        }

        public static void DisposeContext(DataContext context)
        {
            if (context != null)
            {
                context.Dispose();

                if (_context.Value == context)
                {
                    _context.Value = null;
                }
            }
        }
    }

}
