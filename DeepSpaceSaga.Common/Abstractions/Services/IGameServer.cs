using DeepSpaceSaga.Common.Implementation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpaceSaga.Common.Abstractions.Services
{
    public interface IGameServer
    {
        public GameSessionDTO TurnCalculation();
    }
}
