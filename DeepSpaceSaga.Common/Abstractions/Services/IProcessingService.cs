using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Common.Abstractions.Services
{
    public interface IProcessingService
    {
        GameSessionDto Process(ISessionContextService sessionContext);
    }
}
