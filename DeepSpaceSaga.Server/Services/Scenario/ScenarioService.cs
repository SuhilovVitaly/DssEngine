using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpaceSaga.Server.Services.Scenario
{
    public class ScenarioService : IScenarioService
    {
        private readonly IGenerationTool _generationTool;
        public ScenarioService(IGenerationTool generationTool) 
        {
            _generationTool = generationTool;
        }

        public GameSession GetScenario()
        {
            throw new NotImplementedException();
        }
    }
}
