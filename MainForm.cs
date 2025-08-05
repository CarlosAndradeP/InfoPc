using System;
using System.Windows.Forms;

namespace InfoPC
{
    public class MainForm : Form
    {
        public MainForm()
        {
            this.Text = "InfoPC - Informações do Computador";
            this.Width = 600;
            this.Height = 500;

            var infoBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Consolas", 10),
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(infoBox);

            var btnAtualizar = new Button
            {
                Text = "Atualizar",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnAtualizar.Click += (s, e) => infoBox.Text = InfoCollector.GetInfo();
            this.Controls.Add(btnAtualizar);

            infoBox.Text = InfoCollector.GetInfo();
        }
    }
}
