using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace FirstCardPlus.Forms
{
    public partial class f_Settings : Form
    {

        CookieContainer cookieContainer;
        f_Processing f_Processing;

        string token = "";

        string classes = "";

        public f_Settings()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Loading classes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            getToken();
            sendLogin();
            getPrivateSession();

            f_Processing = new f_Processing();
            f_Processing.Show();
            f_Processing.SetTitle("Запрос статистики...");



            f_Processing.SetPercent(50);
            Date date_start = Date.Today;

            string postData = "_token=" + token + $"&class_name=&month={date_start.Month}&year=" + date_start.Year;
            string rawJson = HttpWorker.Post("http://new-pk.first-card.ru/reports/work-days", cookieContainer, postData);

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

            //if (checkBox1.Checked)
            //{
            //    xlApp = new Microsoft.Office.Interop.Excel.Application();

            //    if (xlApp == null)
            //    {
            //        MessageBox.Show("Excel is not properly installed!!");
            //        return;
            //    }
            //    xlWorkBook = xlApp.Workbooks.Add(1);
            //    xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
            //}


            List<string> klas = new List<string>();

            for (int i = 0; i < 147; i++)
            {
                f_Processing.SetPercent((sbyte)(0.68 * i));

                dynamic str = json[(1 + i) + "_data"];

                if (str != null)
                {
                    //while (startdate <= enddate)
                    //{

                    klas.Add(str["className"].ToString());
                    //startdate = startdate.AddDays(1);
                    //}
                    //startdate = dateTimePicker1.Value.ToDate();
                }
            }

            MessageBox.Show(klas.ToString());

            //if (checkBox1.Checked && xlWorkSheet != null)
            //{
            //    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\OLP.xlsx"))
            //        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\OLP.xlsx");

            //    xlWorkBook.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\OLP.xlsx");
            //    xlWorkBook.Close(true, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\OLP.xlsx");
            //    xlApp.Quit();

            //    Marshal.ReleaseComObject(xlWorkSheet);
            //    Marshal.ReleaseComObject(xlWorkBook);
            //    Marshal.ReleaseComObject(xlApp);
            //}

            f_Processing.Close();
        }


        private void getToken()
        {
            token = Regex.Match(
                HttpWorker.Get("http://new-pk.first-card.ru/", cookieContainer),
                "ue = \"(.*)\"", RegexOptions.IgnorePatternWhitespace
                ).Groups[1].Value;
        }

        void sendLogin()
        {
            string postData = "_token=" + token + "&contract_user=0020001155&pass_user=24schoolnutr";
            string responce = HttpWorker.Post("http://new-pk.first-card.ru/proc/login", cookieContainer, postData);
            if (responce.StartsWith("Ч"))
            {
                MessageBox.Show(responce, "Отправка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void getPrivateSession() =>
            //string url = "http://new-pk.first-card.ru/pc";
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.CookieContainer = cookieContainer;
            //HttpWebResponse responce = (HttpWebResponse)request.GetResponse();
            //responce.Close();
            HttpWorker.Get("http://new-pk.first-card.ru/pc", cookieContainer);


        /// <summary>
        /// Config classes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

        }


        
        private void f_Settings_Load(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.class_list.Length != 0)
            {
                b_sm_edit.Enabled = true;
            }
        }
    }
}
