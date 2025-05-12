namespace DeepSpaceSaga.UI
{
    public partial class Form1 : Form
    {
        private static readonly ILog Logger = LogManager.GetLogger(GeneralSettings.WinFormLoggerRepository, typeof(Form1));

        private IWorkerService _worker;

        public Form1()
        {
            InitializeComponent();

            _worker = Program.ServiceProvider?.GetService<IWorkerService>() 
                ?? throw new InvalidOperationException("Failed to resolve IWorkerService");

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
            Logger.Debug("StartProcessing command");
        }

        private async void crlStopProcessing_Click(object sender, EventArgs e)
        {
            await _worker.StopProcessing();
            Logger.Debug("StopProcessing command");
        }

        private void crlResumeProcessing_Click(object sender, EventArgs e)
        {
            _worker.ResumeProcessing();
            Logger.Debug("ResumeProcessing command");
        }

        private void crlPauseProcessing_Click(object sender, EventArgs e)
        {
            _worker.PauseProcessing();
            Logger.Debug("PauseProcessing command");
        }
    }
}
