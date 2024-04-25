using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;
using System.Media;
namespace Geri_sayum

{
    public partial class Form1 : Form
    {

        private System.Windows.Forms.Timer countdownTimer;
        private DateTime targetDateTime;
        private System.Timers.Timer coundtdowntimer; // System.Timers.Timer olarak değiştirin
        private System.Timers.Timer musicStopTimer; // 20 saniye sonra müziği durdurmak için zamanlayıcı
        public Form1()
        {
            InitializeComponent();
            // Tarih ve saat seçimi için DateTimePicker ayarlama
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Tarih ve saat formatı
            dateTimePicker1.ShowUpDown = true; // Saat seçimi için yukarı/aşağı düğmeleri

            // countdownTimer'ı başlatın ve olay işleyicisini bağlayın
            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Interval = 1000; // 1 saniye aralıklarla
            countdownTimer.Tick += OnCountdownTimerElapsed; // Olay işleyicisini bağla

            // 20 saniye sonra müziği durdurmak için zamanlayıcı
            musicStopTimer = new System.Timers.Timer();
            musicStopTimer.Interval = 20000; // 20 saniye
            musicStopTimer.Elapsed += StopMusic; // Zamanlayıcı olayı için işlemci ekleyin

            // NotifyIcon ayarları
            notifyIcon1.Text = "Geri Sayım Uygulaması";
            notifyIcon1.Icon = new Icon("C:\\Users\\ömer\\Downloads\\hourglass_23777.ico");
            notifyIcon1.Visible = true;

            // Bağlam menüsü
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Geri Sayımı Başlat", null, StartCountdown);
            contextMenu.Items.Add("Kapat", null, CloseApplication);
            notifyIcon1.ContextMenuStrip = contextMenu;

            // Diğer olaylar için ayarlamalar
            this.Resize += new EventHandler(Form1_Resize);
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }
        private void StartCountdown(object sender, EventArgs e)
        {
            if (countdownTimer != null)
            {
                targetDateTime = dateTimePicker1.Value; // Hedef tarih ve saati alın
                countdownTimer.Start(); // Geri sayımı başlat
            }
        }
       
        private void CloseApplication(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit(); // Uygulamayı kapat
        }
        private void OnCountdownTimerElapsed(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            TimeSpan timeRemaining = targetDateTime - now;

            if (timeRemaining.TotalSeconds <= 0)
            {
                countdownTimer.Stop();
                UpdateLabel("Süre Doldu!");

                PlaySound("C:\\Users\\ömer\\Downloads\\mp3indirdur-Tarkan-Son-Durak.wav");
            }
            else
            {
                UpdateLabel($"Gün: {timeRemaining.Days}, Saat: {timeRemaining.Hours}, Dakika: {timeRemaining.Minutes}, Saniye: {timeRemaining.Seconds}");
            }
        }

        private void PlaySound(string soundFilePath)
        {
            SoundPlayer player = new SoundPlayer(soundFilePath);
            player.Play(); // Asenkron olarak çal
            musicStopTimer.Start();
        }

        private void StopMusic(object sender, ElapsedEventArgs e)
        {
            musicStopTimer.Stop(); // Zamanlayıcıyı durdur
            // Müzik çalmayı durdurmak için SoundPlayer kullanarak müziği tekrar başlatın (bu, SoundPlayer'da tek yol)
            SoundPlayer player = new SoundPlayer();
            player.Stop(); // Ses çalmayı durdur
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (countdownTimer != null) // Null kontrolü
            {
                StartCountdown(sender, e); // Geri sayımı başlat
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            targetDateTime = dateTimePicker1.Value; // Seçilen tarihi güncelleyin
            DateTime now = DateTime.Now;
            TimeSpan timeRemaining = targetDateTime - now;

            if (timeRemaining.TotalSeconds <= 0)
            {
                countdownTimer.Stop();
                UpdateLabel("Süre Doldu!");
            }
            else
            {
                UpdateLabel($"Gün: {timeRemaining.Days}, Saat: {timeRemaining.Hours}, Dakika: {timeRemaining.Minutes}, Saniye: {timeRemaining.Seconds}");
            }

        }

        private void UpdateLabel(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => label1.Text = text));
            }
            else
            {
                label1.Text = text;
            }
        }
        private void label1_Click(string text)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide(); // Formu gizle
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Uygulama kapatılmak istendiğinde simge durumuna getirin
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide(); // Formu gizle
                e.Cancel = true; // Kapama olayını iptal et
            }

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Formu tekrar göstermek için kod
            this.Show();
            this.WindowState = FormWindowState.Normal; // Normal duruma getir
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

    }
}
