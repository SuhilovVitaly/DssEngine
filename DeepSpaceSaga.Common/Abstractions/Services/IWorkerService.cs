using DeepSpaceSaga.Common.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpaceSaga.Common.Abstractions.Services
{
    public interface IWorkerService
    {
        event Action<GameSessionDTO>? OnGetDataFromServer;
    }
}
