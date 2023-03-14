using HtmlAgilityPack;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1(string radioId)
        {
            InitializeComponent();
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            Label song = new Label();
            song.Location = new Point(78, 10);
            song.Text = GetSong(radioId);
            Console.WriteLine(song.Text);
            song.Font = new Font("Tahoma", 8);
            song.ForeColor = Color.White;
            song.Width = 500;
            this.Controls.Add(song);

            Label text = new Label();
            text.Location = new Point(10, 10);
            text.Text = "current song:";
            text.Font = new Font("Tahoma", 8);
            text.ForeColor = Color.White;
            this.Controls.Add(text);

            StartTimer(song, radioId);
        }

        async void StartTimer(Label song, string radioId)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
            while (await timer.WaitForNextTickAsync())
            {
                song.Text = GetSong(radioId);
            }
        }

        string GetSong(string radioId)
        {
            var html = @"https://somafm.com/" + radioId + "/songhistory.html";

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);

            var node = htmlDoc.GetElementbyId("playinc").ChildNodes;
            var row = node.FindFirst("tr").NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
            var artist = row.FirstChild.NextSibling.NextSibling;
            var song_title = artist.NextSibling;

            return artist.InnerText.ToLower() + " - " + song_title.InnerText.ToLower();
        }
    }
}