using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;

namespace _1C_Shop
{
    public partial class checking_day : Form
    {
        SqlDataAdapter adapter;
        SqlConnection con;
        DataSet data_set;
        SqlCommand sql_cmd;
        SqlDataReader ans;

        DateTime dt = TIMEE.StartOfWeek(DateTime.Now, DayOfWeek.Monday);  // Start of week.
        DateTime dt1 = TIMEE.EndOfWeek(DateTime.Now, DayOfWeek.Sunday);  // End of week.

        [Table(Name = "History")]
        public class History
        {
            [Column(IsPrimaryKey = true, IsDbGenerated = true)]
            public int Id { get; set; }
            [Column(Name = "date_time")]
            public DateTime date_time { get; set; }
            [Column(Name = "product_and_price")]
            public string product_and_price { get; set; }
            [Column(Name = "total_paid")]
            public double total_paid { get; set; }
            [Column(Name = "was_given")]
            public double was_given { get; set; }
            [Column(Name = "change")]
            public double change { get; set; }
            [Column(Name = "payment_method")]
            public int payment_method { get; set; }
            [Column(Name = "by_user")]
            public string by_user { get; set; }
            [Column(Name = "date")]
            public DateTime date { get; set; }
        }

        static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Kreal\source\repos\1C Shop\1C Shop\Database.mdf;Integrated Security=True";

        public checking_day()
        {
            InitializeComponent();
            sql_cmd = new SqlCommand();
            data_set = new DataSet();
            con = new SqlConnection(connectionString);
        }

        private void checking_day_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "databaseDataSet2.History". При необходимости она может быть перемещена или удалена.
            //this.historyTableAdapter2.Fill(this.databaseDataSet2.History);
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cmd = @"select * from History as H where H.Date = CONVERT(DATETIME,'"+ DateTime.Today +"',104)";
            sql_cmd.Connection = con;
            sql_cmd.CommandText = cmd;
            //
            adapter = new SqlDataAdapter(cmd, con);
            data_set = new DataSet();
            adapter.Fill(data_set);
            dataGridView1.DataSource = data_set.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string cmd = @"select * from History";
            sql_cmd.Connection = con;
            sql_cmd.CommandText = cmd;
            //
            adapter = new SqlDataAdapter(cmd, con);
            data_set = new DataSet();
            adapter.Fill(data_set);
            dataGridView1.DataSource = data_set.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dt + " " + dt1);  // For a checking the dates.
            string cmd = @"select * from History as H where H.Date >= CONVERT(DATETIME,'"+ dt +"',104) AND H.Date <= CONVERT(DATETIME,'"+ dt1 +"',104)";
            sql_cmd.Connection = con;
            sql_cmd.CommandText = cmd;
            //
            adapter = new SqlDataAdapter(cmd, con);
            data_set = new DataSet();
            adapter.Fill(data_set);
            dataGridView1.DataSource = data_set.Tables[0];
            chart1.Series[0].XValueType = ChartValueType.Date;
            //
            //
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            List<string> column = new List<string>();
            for (DateTime date_point = dt; date_point <= dt1; date_point = date_point.AddDays(1))
            {
                DateTime x = date_point;
                double y = 0;  // Sum for Y.
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    column.Add(dataGridView1[7, i].Value.ToString());
                    if (x == Convert.ToDateTime(dataGridView1.Rows[i].Cells[1].Value)) //УБРАТЬ МИНУТЫ!
                    {
                        y += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);  
                    }
                }
                chart1.Series[0].Points.AddXY(x, y);
                y = 0;
            }
            column.Distinct().ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp =
                        new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
            //Книга.
            ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
            //Таблица.
            ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                ExcelApp.Cells[2, i + 1] = Convert.ToString(dataGridView1.Columns[i].HeaderText);
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    ExcelApp.Cells[i + 3, j + 1] = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                }
            }
            //Вызываем приложение Excel.
            ExcelApp.Visible = true;
            ExcelApp.UserControl = true;
            button5.Enabled = true;
        }

        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "1C Shop";

        private static UserCredential GetCredentials()
        {
            UserCredential credential;

            using (var stream = new FileStream("drive_id.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                // Console.WriteLine("Credential file saved to: " + credPath);
            }
            return credential;
        }

        private static void UploadBasicImage(string path, DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = Path.GetFileName(path);
            fileMetadata.MimeType = "image/jpeg";
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;

            //Console.WriteLine("File ID: " + file.Id);

        }

        private static void ListFiles(DriveService service, ref string pageToken)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1;
            //listRequest.Fields = "nextPageToken, files(id, name)";
            listRequest.Fields = "nextPageToken, files(name)";
            listRequest.PageToken = pageToken;
            listRequest.Q = "mimeType='image/jpeg'";

            // List files.
            var request = listRequest.Execute();

            if (request.Files != null && request.Files.Count > 0)
            {
                foreach (var file in request.Files)
                {
                    Console.WriteLine("{0}", file.Name);
                }

                pageToken = request.NextPageToken;

                if (request.NextPageToken != null)
                {
                    //Console.WriteLine("Press any key to conti...");
                    //Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("No files found.");

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UserCredential credential;

            credential = GetCredentials();

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            UploadBasicImage(@"D:\ddd.txt", service);
            string pageToken = null;
            do
            {
                ListFiles(service, ref pageToken);

            } while (pageToken != null);
        }
    }

    public static class TIMEE
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 - (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(1 * diff).Date;
        }
    }
}
