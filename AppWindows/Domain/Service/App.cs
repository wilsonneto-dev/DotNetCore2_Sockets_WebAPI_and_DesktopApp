using AppWindows.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppWindows.Domain.Service
{
    class App
    {
        private readonly SocketService socketService;
        private readonly MainForm mainForm;
        public App(MainForm form)
        {
            mainForm = form;
            socketService = new SocketService();
            App.instance = this;
        }

        public void Run()
        {
            Task.Run(() => socketService.RunServer(6565));
            mainForm.Log("Servidor rodando...");
        }

        public void GetInput()
        {
            Input formInput = new Input();
            formInput.ShowDialog();
        }

        public void SendInput(String message)
        {
            this.socketService.Send(message);
        }
        
        public void Message(String texto)
        {
            Task.Run(() => MessageBox.Show(texto));
        }

        public static App instance;

    }
}
