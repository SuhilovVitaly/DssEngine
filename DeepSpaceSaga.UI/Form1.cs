using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Implementation;
using DeepSpaceSaga.UI.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace DeepSpaceSaga.UI
{
    public partial class Form1 : Form
    {
        private IWorkerService _worker;

        public Form1()
        {
            InitializeComponent();

            _worker = Program.ServiceProvider.GetService<IWorkerService>();

            if (_worker is null) return;

            _worker.OnGetDataFromServer += WorkerService_OnGetDataFromServer;


        }

        private void WorkerService_OnGetDataFromServer(GameSessionDTO session)
        {
            CrossThreadExtensions.PerformSafely(this, RefreshSessionInfo, session);            
        }

        private void RefreshSessionInfo(GameSessionDTO session)
        {
            crlSessionInfo.Text = session.Turn.ToString();
        }

        private void crlStartProcessing_Click(object sender, EventArgs e)
        {
            _worker.StartProcessing();
        }

        private async void crlStopProcessing_Click(object sender, EventArgs e)
        {
            await _worker.StopProcessing();
        }
    }
}
