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

        private void WorkerService_OnGetDataFromServer(string state, GameSessionDTO session)
        {
            CrossThreadExtensions.PerformSafely(this, RefreshSessionInfo, state, session);
        }

        private void RefreshSessionInfo(string state, GameSessionDTO session)
        {
            crlSessionInfo.Text = state + " Turn: " + session.Turn;
        }

        private void crlStartProcessing_Click(object sender, EventArgs e)
        {
            _worker.StartProcessing();
        }

        private async void crlStopProcessing_Click(object sender, EventArgs e)
        {
            await _worker.StopProcessing();
        }

        private void crlResumeProcessing_Click(object sender, EventArgs e)
        {
            _worker.ResumeProcessing();
        }

        private void crlPauseProcessing_Click(object sender, EventArgs e)
        {
            _worker.PauseProcessing();
        }
    }
}
