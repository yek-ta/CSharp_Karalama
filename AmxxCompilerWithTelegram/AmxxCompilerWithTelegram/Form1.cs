using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Telegram.Bot;
using System.Diagnostics;

namespace AmxxCompilerWithTelegram
{
    
    public partial class Form1 : Form
    {
        bool derleyebilir;
        string kuruludizin = Application.StartupPath;
        static TelegramBotClient Bot = new TelegramBotClient("18asdasdasdaqqweqwjDOihtHUtA");
        public Form1()
        {
            derleyebilir = true;
            InitializeComponent();
            Bot.StartReceiving();
            Bot.OnMessage += Bot_OnMessage;
            

            Bot.SendTextMessageAsync(187921443, "Program Açıldı");
            //Directory.CreateDirectory("D:\\telegramcompiler");
            label1.Text = kuruludizin;

        }
        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                //Bot.SendTextMessageAsync(e.Message.Chat.Id, "Yaşıyorum!! " + e.Message.Chat.Username);
                
            }
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                if (derleyebilir)
                {
                    derleyebilir = false;
                    //Bot.SendTextMessageAsync(e.Message.Chat.Id, "dosya geldi!! " + e.Message.Document);

                    Telegram.Bot.Types.File file = await Bot.GetFileAsync(e.Message.Document.FileId);

                    string fileName = file.FileId + "." + file.FilePath.Split('.').Last();

                    using (FileStream saveFileStream = File.Open(fileName, FileMode.Create))
                    {
                        await Bot.DownloadFileAsync(file.FilePath, saveFileStream);
                    }
                    await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Dosya Sunucuya Ulaştırıldı..");
                    
                    //File.Copy( + fileName, @"D:\telegramcompiler");
                    Process.Start(kuruludizin + @"\compile.exe");

                    System.Threading.Thread.Sleep(2000);
                    System.IO.File.Move(fileName, @"silinmeli\"+fileName);
                    string yenisim= fileName.Replace("sma", "amxx");
                    yenisim = @"compiled\" + yenisim;
                    if (!File.Exists(yenisim))
                    {
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "Dosya ile ilgili sıkıntı var, derleyemedim " + e.Message.Chat.Username);
                    }
                    else
                    {
                        using (var sendFileStream = File.Open(yenisim, FileMode.Open))
                        {
                            await Bot.SendDocumentAsync(e.Message.Chat.Id, new Telegram.Bot.Types.InputFiles.InputOnlineFile(sendFileStream, yenisim));
                        }
                    }
                }
                else
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Derleyicimin kapatılması için bekleyiniz " + e.Message.Chat.Username);
                }
                


            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            derleyebilir = true;


            string processName = "compile"; // Kapatmak İstediğimiz Program
            Process[] processes = Process.GetProcesses();// Tüm Çalışan Programlar
            foreach (Process process in processes)
            {
                if (process.ProcessName == processName)
                {
                    process.Kill();
                }

            }
            label1.Text = "Derleyici Kapatıldı";
        }
    }
}
