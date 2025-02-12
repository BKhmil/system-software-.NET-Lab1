using System;

namespace Lab1
{
    public partial class Form1 : Form
    {
        private static readonly string _directoryPath = AppDomain.CurrentDomain.BaseDirectory;

        public Form1()
        {
            InitializeComponent();
        }

        private void ReadTextFiles()
        {
            string[] files = Directory.GetFiles(_directoryPath, "*.txt");

            foreach (string file in files)
            {
                // page 12
                Thread th = new Thread(() =>
                {
                    string content = File.ReadAllText(file);
                    DisplayText(content);
                });
                th.IsBackground = true;
                th.Start();
            }
        }

        private void DisplayText(string text)
        {
            Label label = new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent
            };

            Random random = new Random();

            int x = random.Next(0, this.ClientSize.Width - 100);
            int y = random.Next(0, this.ClientSize.Height - 20);
            label.Location = new Point(x, y);

            Thread.Sleep(random.Next(2000, 6000));
            // page 78
            Invoke(new Action(() => this.Controls.Add(label)));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ReadTextFiles();
        }
    }
}
