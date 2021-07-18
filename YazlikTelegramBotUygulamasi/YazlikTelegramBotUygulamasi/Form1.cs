using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;

namespace YazlikTelegramBotUygulamasi
{
    public partial class Form1 : Form
    {
        static TelegramBotClient Bot = new TelegramBotClient("65asdasd6021:AadasdasdpD9MoNzQRY_fgd");
        public Form1()
        {
            InitializeComponent();
            listBox1.Items.Add("(" + DateTime.Now.ToString("h:mm") + ") Program Açıldı..");
            label1.Text = "Düzenli olarak " + ((timer1.Interval/1000)/60)/60 + " saatte bir mesaj gönderilecek..";
            Bot.StartReceiving();
            Bot.OnMessage += Bot_OnMessage;

            listBox1.SelectedIndex = listBox1.Items.Count - 1;

            Bot.SendTextMessageAsync(187921443, "Program Açıldı");
        }

        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if(e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Yaşıyorum!! "+ e.Message.Chat.Username);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Bot.SendTextMessageAsync(187921443, "Sistem Çalışmaktadır..");
            listBox1.Items.Add("(" + DateTime.Now.ToString("h:mm") + ") Mesaj Gönderildi.");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
