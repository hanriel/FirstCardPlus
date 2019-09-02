using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using AutoUpdaterDotNET;
using MySql.Data.MySqlClient;

namespace FirstCardPlus.Forms
{
    public partial class f_Main : Form
    {
        readonly CookieContainer _cookieContainer;
        Date _dateStart;
        string[] _dateStartArray;
        Date _dateEnd;
        f_Processing _fProcessing;
        private MySqlConnection conn;

        string _token = "";

        public f_Main()
        {
            InitializeComponent();

            _cookieContainer = new CookieContainer();

            GetToken();
            SendLogin();
            GetPrivateSession();

            AutoUpdater.Start("http://new.sch24perm.ru/soft/fcp/updates/Versions.xml");
        }

        private void b_exit_Click(object sender, EventArgs e) => Application.Exit();

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string connStr = "server=10.8.0.1;user=root;Port=3305";

            conn = new MySqlConnection(connStr);
            conn.Open();

            listBox1.Items.Clear();
            Thread myThread = new Thread(GetWorkList);
            myThread.Start();
        }

        private void GetToken()
        {
            _token = Regex.Match(
                HttpWorker.Get("http://new-pk.first-card.ru/", _cookieContainer),
                "ue = \"(.*)\"", RegexOptions.IgnorePatternWhitespace
            ).Groups[1].Value;
        }

        void SendLogin()
        {
            var postData = "_token=" + _token + "&contract_user=0020001155&pass_user=24schoolnutr";
            var response = HttpWorker.Post("http://new-pk.first-card.ru/proc/login", _cookieContainer, postData);
            if (response[0] == 'Ч')
            {
                MessageBox.Show(response, "Отправка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetPrivateSession() => HttpWorker.Get("http://new-pk.first-card.ru/pc", _cookieContainer);

        void GetWorkList()
        {
            _fProcessing = new f_Processing();
            _fProcessing.Show();
            _fProcessing.SetTitle("Запрос статистики...");

            _fProcessing.SetPercent(50);
            _dateStart = dp_start.Value.ToDate();
            _dateEnd = dp_start.Value.ToDate();

            _dateStartArray = _dateStart.ToShortString().Split('.');

            string postData = $"_token={_token}&class_name=&month=0{_dateStart.Month}&year={_dateStart.Year}";
            string rawJson =
                HttpWorker.Post("http://new-pk.first-card.ru/reports/work-days", _cookieContainer, postData);


            dynamic json = null;
            try
            {
                _fProcessing.SetPercent(75);
                json = JsonConvert.DeserializeObject<dynamic>(rawJson)["return_data"]["data"]["report"];
            }
            catch (Exception)
            {
                _fProcessing.Close();
                MessageBox.Show("Ошибка получения данных, попробуёте ещё раз...");
                GetToken();
                SendLogin();
                GetPrivateSession();
                Thread.CurrentThread.Abort();
            }

            _fProcessing.SetTitle("Проверка процентности...");
            _fProcessing.SetPercent(0);

            for (int i = 0; i < 147; i++)
            {
                _fProcessing.SetPercent((sbyte) (0.68 * i));

                dynamic str = json[(1 + i) + "_data"];
                if (str != null)
                {
                    int linear = str["learnersCount"];
                    //MessageBox.Show(_dateStartArray[0].ToString());
                    int today = str["dayOfMonth_" + Int32.Parse(_dateStart.Day.ToString()).ToString()];

                    if (linear != today)
                    {
                        richTextBox1.Invoke(new Action(() =>
                        {
                            richTextBox1.Text +=
                                str["className"].ToString() + " - " + today + " из " + linear + "\n";
                        }));
                        GetFullList(str["className"].ToString(), _dateStart);
                    }
                }
            }

            _fProcessing.Close();
            conn.Close();
        }

        void GetFullList(string klass, Date date)
        {
            var postData = "_token=" + _token + "&class_name=" + klass +
                           $"&month=0{_dateStart.Month}&year={_dateStart.Year}";
            var rawJson = HttpWorker.Post("http://new-pk.first-card.ru/reports/for-ae", _cookieContainer,
                Encoding.UTF8.GetBytes(postData));

            dynamic json = JsonConvert.DeserializeObject<dynamic>(rawJson)["return_data"]["data"]["report"];
            var regex = new Regex("user_([0-9]*)");
            var mc = regex.Matches(Regex.Unescape(rawJson));

            string[] uids = new string[mc.Count];
            sbyte ins = 0;
            foreach (Match match in mc) uids[ins++] = match.Groups[1].Value;

            for (int i = 0; i < uids.Length; i++)
            {
                dynamic user = json["user_" + uids[i]][date.ToString("s")];
                if (user == null) continue;
                if ((bool) user.passages || (bool) user.pays) continue;

                richTextBox1.Invoke(new Action(() =>
                {
                    var sql = $"SELECT ID,CODEKEY FROM `tc-db-main`.personal where EXTID='{uids[i]}';";

                    var cmd = new MySqlCommand(sql, conn);
                    var rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        var sql2 =
                            $"INSERT INTO `tc-db-log`.logs (LOGTIME,FRAMETS,AREA,LOGDATA,EMPHINT,DEVHINT) VALUES (curtime(),0,0,UNHEX('FE060000020300000000{BitConverter.ToString((byte[]) rdr[1]).Replace("-", string.Empty)}FFFF'),{rdr[0].ToString()},23);";

                        rdr.Close();

                        MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                        cmd2.ExecuteNonQuery();
                    }

                    rdr.Close();

                    richTextBox1.Text += "- " + json["user_" + uids[i]]["0"]["2"] + " " + uids[i] + "\n";
                }));
            }
        }

        #region UI

        private void dp_start_ValueChanged(object sender, EventArgs e) => dp_end.MinDate = dp_start.Value;
        private void dp_end_ValueChanged(object sender, EventArgs e) => dp_start.MaxDate = dp_end.Value;

        private void mb_settings_Click(object sender, EventArgs e)
        {
            f_Settings form = new f_Settings();
            form.ShowDialog();
            form.Close();
        }

        #endregion UI
    }

    public static class HttpWorker
    {
        #region Get

        public static string Get(string uri) => Get(new Uri(uri), null);
        public static string Get(Uri uri) => Get(uri, null);
        public static string Get(string uri, CookieContainer cookieContainer) => Get(new Uri(uri), cookieContainer);

        public static string Get(Uri uri, CookieContainer cookieContainer)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
            request.CookieContainer = cookieContainer;
            return getResponce(request);
        }

        #endregion Get

        #region Post

        public static string Post(string uri) => Post(new Uri(uri), null, "");
        public static string Post(Uri uri) => Post(uri, null, "");

        public static string Post(string uri, CookieContainer cookieContainer)
            => Post(new Uri(uri), cookieContainer, "");

        public static string Post(Uri uri, CookieContainer cookieContainer)
            => Post(uri, cookieContainer, "");

        public static string Post(string uri, CookieContainer cookieContainer, string postData)
            => Post(new Uri(uri), cookieContainer, new ASCIIEncoding().GetBytes(postData));

        public static string Post(string uri, CookieContainer cookieContainer, byte[] postData)
            => Post(new Uri(uri), cookieContainer, postData);

        public static string Post(Uri uri, CookieContainer cookieContainer, string postData)
            => Post(uri, cookieContainer, new ASCIIEncoding().GetBytes(postData));

        public static string Post(Uri uri, CookieContainer cookieContainer, byte[] postData)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
            request.Method = "POST";
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(postData, 0, postData.Length);
            stream.Close();
            return getResponce(request);
        }

        #endregion Post

        private static string getResponce(HttpWebRequest request)
        {
            HttpWebResponse responce = (HttpWebResponse) request.GetResponse();
            using (Stream stream = responce.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                return reader.ReadToEnd();
        }
    }
}