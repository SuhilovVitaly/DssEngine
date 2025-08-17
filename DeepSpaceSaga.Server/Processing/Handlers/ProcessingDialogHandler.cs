using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpaceSaga.Server.Processing.Handlers
{
    public class ProcessingDialogHandler
    {
        public void Execute(ISessionContextService sessionContext)
        {
            var dialogExitData = new ConcurrentDictionary<string, ICommand>();

            foreach (var command in sessionContext.GameSession.Commands.Values)
            {
                if (command.Category == Common.Abstractions.Entities.Commands.CommandCategory.DialogExit)
                {
                    dialogExitData.TryAdd(command.Id.ToString(), command);
                }
            }

            lock (sessionContext)
            {
                foreach (var acknowledgedEvent in dialogExitData)
                {
                    sessionContext.GameSession.DialogsExits.TryAdd(acknowledgedEvent.Key, acknowledgedEvent.Key);
                }
            }
        }
    }
}
