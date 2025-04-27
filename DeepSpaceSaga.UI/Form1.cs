namespace DeepSpaceSaga.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Global.WorkerService.OnGetDataFromServer += WorkerService_OnGetDataFromServer;
        }

        private void WorkerService_OnGetDataFromServer(Common.Implementation.GameSessionDTO session)
        {
            var x = session.Turn;
        }
    }
}
