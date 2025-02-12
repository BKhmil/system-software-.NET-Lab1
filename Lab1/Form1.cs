namespace Lab1
{
    public partial class Form1 : Form
    {
        // отримання шляху до директорії з файлами
        private static readonly string _directoryPath = AppDomain.CurrentDomain.BaseDirectory;

        public Form1()
        {
            InitializeComponent();
        }

        // Метод що зчитує всі текстові файли у зазначеній директорії та створює потоки для їх обробки.
        private void ReadTextFiles()
        {
            // Отримання усіх текстових файлів у директорії
            string[] files = Directory.GetFiles(_directoryPath, "*.txt");

            foreach (string file in files)
            {
                // Створення нового потоку для обробки кожного файлу
                Thread th = new Thread(() =>
                {
                    // Зчитування вмісту файлу
                    string content = File.ReadAllText(file);

                    // Відображення вмісту на формі
                    DisplayText(content);
                });

                // Встановлення потоку у фоновий режим,
                // це зроблено для того, щоб новостворені потоки завершувалися разом з головним потоком
                th.IsBackground = true;

                // Запуск потоку
                th.Start(); 
            }
        }

        // Метод, що відображає переданий текст у випадковому місці на формі.
        private void DisplayText(string text)
        {
            // Створення об'єкту Label для відображення тексту
            Label label = new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent
            };

            // Генерація випадкової позиції на формі
            // Кожен потік матиме свій власний об'єкт типу Random,
            // для отримання коректних значень з методу new Random.Next()
            // https://stackoverflow.com/questions/3049467/is-c-sharp-random-number-generator-thread-safe
            Random random = new Random();

            // під час генерації позиції беруться до увагу межі форми, для того щоб Label не виходив за межі
            int x = random.Next(0, this.ClientSize.Width - 100);
            int y = random.Next(0, this.ClientSize.Height - 20);
            label.Location = new Point(x, y);

            // Затримка перед додаванням мітки на форму
            Thread.Sleep(random.Next(2000, 6000));

            // Виконання оновлення UI через головний потік
            // Використовується метод Invoke, оскільки доступ до елементів форми можливий лише з головного потоку
            // Action - делегат, який не приймає параметрів і не повертає значення, оскільки він тут ідеально підходить, він і був використаний
            Invoke(new Action(() => this.Controls.Add(label)));
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.ReadTextFiles();
        }
    }
}
