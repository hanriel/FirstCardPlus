using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace FirstCardPlus.Forms
{
    public partial class f_Main : Form
    {
        CookieContainer cookieContainer;
        Date date_start;
        Date date_end;
        f_Processing f_Processing;

        Microsoft.Office.Interop.Excel.Application xlApp;
        Worksheet xlWorkSheet;
        Workbook xlWorkBook;

        string token = "";

        public f_Main()
        {
            InitializeComponent();

            cookieContainer = new CookieContainer();

            getToken();
            sendLogin();
            getPrivateSession();
        }

        private void b_exit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Thread myThread = new Thread(getWorkList);
            myThread.Start();
        }

        private void getToken()
        {
            token = Regex.Match(
                HTTP_Worker.Get("http://new-pk.first-card.ru/", cookieContainer),
                "ue = \"(.*)\"", RegexOptions.IgnorePatternWhitespace
                ).Groups[1].Value;
        }

        void sendLogin()
        {
            string postData = "_token=" + token + "&contract_user=0020001155&pass_user=24schoolnutr";
            string responce = HTTP_Worker.Post("http://new-pk.first-card.ru/proc/login", cookieContainer, postData);
            if (responce.StartsWith("Ч"))
            {
                MessageBox.Show(responce,"Отправка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void getPrivateSession() =>
            //string url = "http://new-pk.first-card.ru/pc";
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.CookieContainer = cookieContainer;
            //HttpWebResponse responce = (HttpWebResponse)request.GetResponse();
            //responce.Close();
            HTTP_Worker.Get("http://new-pk.first-card.ru/pc", cookieContainer);

        void getWorkList()
        {
            f_Processing = new f_Processing();
            f_Processing.Show();
            f_Processing.SetTitle("Запрос статистики...");

            

            f_Processing.SetPercent(50);
            date_start = dp_start.Value.ToDate();
            date_end = dp_start.Value.ToDate();

            string postData = "_token=" + token + $"&class_name=&month={date_start.Month}&year="+date_start.Year;
            string rawJson = HTTP_Worker.Post("http://new-pk.first-card.ru/reports/work-days", cookieContainer, postData);
            
            dynamic json = null;
            try
            {
                f_Processing.SetPercent(75);
                json = JsonConvert.DeserializeObject<dynamic>(rawJson)["return_data"]["data"]["report"];
            }
            catch (Exception)
            {
                f_Processing.Close();
                MessageBox.Show("Ошибка получения данных, попробуёте ещё раз...");
                getToken();
                sendLogin();
                getPrivateSession();
                Thread.CurrentThread.Abort();
            }

            f_Processing.SetTitle("Проверка процентности...");
            f_Processing.SetPercent(0);

            if (checkBox1.Checked)
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();

                if (xlApp == null)
                {
                    MessageBox.Show("Excel is not properly installed!!");
                    return;
                }
                xlWorkBook = xlApp.Workbooks.Add(1);
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
            }
            
            for (int i = 0; i < 147; i++)
            {
                f_Processing.SetPercent((sbyte)(0.68 * i));

                dynamic str = json[(1 + i) + "_data"];

                if (str != null)
                {
                    //while (startdate <= enddate)
                    //{
                        
                        int linear = str["learnersCount"];
                        int today = str["dayOfMonth_" + date_start.Day];
                        if (today == 0) continue;
                        if(linear != today)
                        {
                            listBox1.Invoke(new System.Action(() => {
                                listBox1.Items.Add(date_start.ToString("s") + " " + str["className"].ToString() + " - " + today + " из " + linear);
                            }));

                            if (checkBox1.Checked && xlWorkSheet != null)
                            {
                                xlWorkSheet.Cells[listBox1.Items.Count, 1] = date_start.ToString("s");
                                xlWorkSheet.Cells[listBox1.Items.Count, 2] = str["className"].ToString() + " - " + today + " из " + linear;
                            }

                            getFullList(str["className"].ToString(), date_start);
                        }
                        //startdate = startdate.AddDays(1);
                    //}
                    //startdate = dateTimePicker1.Value.ToDate();
                }
            }

            if (checkBox1.Checked && xlWorkSheet != null)
            {
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\OLP.xlsx"))
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\OLP.xlsx");

                xlWorkBook.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\OLP.xlsx");
                xlWorkBook.Close(true, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\OLP.xlsx");
                xlApp.Quit();
            
                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
            }

            f_Processing.Close();
        }

        void getFullList(string klass, Date date)
        {
            string postData = "_token=" + token + "&class_name="+ klass + $"&month={date_start.Month}&year={date_start.Year}";
            string rawJson = HTTP_Worker.Post("http://new-pk.first-card.ru/reports/for-ae", cookieContainer, Encoding.UTF8.GetBytes(postData));

            dynamic json = JsonConvert.DeserializeObject<dynamic>(rawJson)["return_data"]["data"]["report"];
            Regex regex = new Regex("user_([0-9]*)");
            MatchCollection mc = regex.Matches(Regex.Unescape(rawJson));

            string[] uids = new string[mc.Count];
            sbyte ins = 0;
            foreach (Match match in mc) uids[ins++] = match.Groups[1].Value;
            
            double ing = 100 / uids.Length;
            for (int i = 0; i < uids.Length; i++)
            {
                dynamic user = json["user_" + uids[i]][date.ToString("s")];
                if (user == null) continue;
                if ((bool)user.passages || (bool)user.pays) continue;
                else
                {
                    listBox1.Invoke(new System.Action(() => { listBox1.Items.Add("--- " + json["user_" + uids[i]]["0"]["2"]); }));
                    if (checkBox1.Checked && xlWorkSheet != null)
                    {
                        xlWorkSheet.Cells[listBox1.Items.Count, 1] = i.ToString();
                        xlWorkSheet.Cells[listBox1.Items.Count, 2] = json["user_" + uids[i]]["0"]["2"];
                    }
                }
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

    class HTTP_Worker
    {
        #region Get
        public static string Get(string uri) => Get(new Uri(uri), null);
        public static string Get(Uri uri) => Get(uri, null);

        public static string Get(string uri, CookieContainer cookieContainer) => Get(new Uri(uri), cookieContainer);
        public static string Get(Uri uri, CookieContainer cookieContainer)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
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
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
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
            HttpWebResponse responce = (HttpWebResponse)request.GetResponse();
            using (Stream stream = responce.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    return reader.ReadToEnd();
        }

    }
}
