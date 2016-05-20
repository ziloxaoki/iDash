using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    interface ICommandHandler
    {
        void executeCommand(Command command);
    }
}
