using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler.CustomCommands
{
    public class AddingExperienceDialogCommand : ICustomDialogCommand
    {
        public void Execute(ISessionContextService sessionContext, ICommand command)
        {
            command.Status = CommandStatus.Finalizing;
        }
    }
}
