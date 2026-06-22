using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace EduCenterApp
{
    public partial class MainWindow : Window
    {
        string connStr = "server=localhost;database=adult_edu;uid=root;pwd=;";

        public MainWindow()
        {
            InitializeComponent();
            LoadAllData();
        }

        private void LoadAllData()
        {
            LoadCursanti("");
            LoadCursuri("");
            LoadInscrieri();
            LoadComboBoxes();
            LoadReportAndStats();
        }

        
        private DataTable ExecuteSelect(string query, MySqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        private void ExecuteQuery(string query, MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

       
        private void LoadCursanti(string filter)
        {
            string q = "SELECT IdCursant, Nume, Prenume, Telefon, Email FROM Cursant WHERE Nume LIKE @f OR Email LIKE @f";
            GridCursanti.ItemsSource = ExecuteSelect(q, new[] { new MySqlParameter("@f", "%" + filter + "%") }).DefaultView;
        }

        private void TxtSearchC_TextChanged(object sender, TextChangedEventArgs e) => LoadCursanti(txtSearchC.Text);

        private void BtnAddCursant_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateCursant()) return;
            try
            {
                ExecuteQuery("INSERT INTO Cursant (Nume, Prenume, Telefon, Email) VALUES (@n, @p, @t, @e)",
                new[] { new MySqlParameter("@n", txtNumeC.Text), new MySqlParameter("@p", txtPrenumeC.Text),
                            new MySqlParameter("@t", txtTelefonC.Text), new MySqlParameter("@e", txtEmailC.Text) });
                LoadAllData();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка (возможно Email уже существует):\n" + ex.Message); }
        }

        private void BtnUpdateCursant_Click(object sender, RoutedEventArgs e)
        {
            if (GridCursanti.SelectedItem == null || !ValidateCursant()) return;
            DataRowView row = (DataRowView)GridCursanti.SelectedItem;
            ExecuteQuery("UPDATE Cursant SET Nume=@n, Prenume=@p, Telefon=@t, Email=@e WHERE IdCursant=@id",
            new[] { new MySqlParameter("@n", txtNumeC.Text), new MySqlParameter("@p", txtPrenumeC.Text),
                new MySqlParameter("@t", txtTelefonC.Text), new MySqlParameter("@e", txtEmailC.Text),
                        new MySqlParameter("@id", row["IdCursant"]) });
            LoadAllData();
        }

        private void BtnDeleteCursant_Click(object sender, RoutedEventArgs e)
        {
            if (GridCursanti.SelectedItem == null) return;
            DataRowView row = (DataRowView)GridCursanti.SelectedItem;
            ExecuteQuery("DELETE FROM Cursant WHERE IdCursant=@id", new[] { new MySqlParameter("@id", row["IdCursant"]) });
            LoadAllData();
        }

        private void GridCursanti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridCursanti.SelectedItem is DataRowView row)
            {
                txtNumeC.Text = row["Nume"].ToString();
                txtPrenumeC.Text = row["Prenume"].ToString();
                txtTelefonC.Text = row["Telefon"].ToString();
                txtEmailC.Text = row["Email"].ToString();
            }
        }

        private bool ValidateCursant()
        {
            if (string.IsNullOrWhiteSpace(txtNumeC.Text) || string.IsNullOrWhiteSpace(txtPrenumeC.Text) ||
                string.IsNullOrWhiteSpace(txtTelefonC.Text) || string.IsNullOrWhiteSpace(txtEmailC.Text))
            { MessageBox.Show("Все поля обязательны!"); return false; }

            if (!Regex.IsMatch(txtEmailC.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            { MessageBox.Show("Неверный формат Email!"); return false; }
            return true;
        }

      
        private void LoadCursuri(string filter)
        {
            string q = "SELECT IdCurs, Denumire, Formator, Pret, DurataZile FROM Curs WHERE Formator LIKE @f";
            GridCursuri.ItemsSource = ExecuteSelect(q, new[] { new MySqlParameter("@f", "%" + filter + "%") }).DefaultView;
        }

        private void TxtSearchCurs_TextChanged(object sender, TextChangedEventArgs e) => LoadCursuri(txtSearchCurs.Text);

        private void BtnAddCurs_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateCurs()) return;
            ExecuteQuery("INSERT INTO Curs (Denumire, Formator, Pret, DurataZile) VALUES (@d, @f, @p, @z)",
            new[] { new MySqlParameter("@d", txtDenumire.Text), new MySqlParameter("@f", txtFormator.Text),
                new MySqlParameter("@p", Convert.ToDecimal(txtPret.Text)), new MySqlParameter("@z", Convert.ToInt32(txtDurata.Text)) });
            LoadAllData();
        }

        private void BtnUpdateCurs_Click(object sender, RoutedEventArgs e)
        {
            if (GridCursuri.SelectedItem == null || !ValidateCurs()) return;
            DataRowView row = (DataRowView)GridCursuri.SelectedItem;
            ExecuteQuery("UPDATE Curs SET Denumire=@d, Formator=@f, Pret=@p, DurataZile=@z WHERE IdCurs=@id",
            new[] { new MySqlParameter("@d", txtDenumire.Text), new MySqlParameter("@f", txtFormator.Text),
                new MySqlParameter("@p", Convert.ToDecimal(txtPret.Text)), new MySqlParameter("@z", Convert.ToInt32(txtDurata.Text)),
                new MySqlParameter("@id", row["IdCurs"]) });
            LoadAllData();
        }

        private void BtnDeleteCurs_Click(object sender, RoutedEventArgs e)
        {
            if (GridCursuri.SelectedItem == null) return;
            DataRowView row = (DataRowView)GridCursuri.SelectedItem;
            ExecuteQuery("DELETE FROM Curs WHERE IdCurs=@id", new[] { new MySqlParameter("@id", row["IdCurs"]) });
            LoadAllData();
        }

        private void GridCursuri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridCursuri.SelectedItem is DataRowView row)
            {
                txtDenumire.Text = row["Denumire"].ToString();
                txtFormator.Text = row["Formator"].ToString();
                txtPret.Text = row["Pret"].ToString();
                txtDurata.Text = row["DurataZile"].ToString();
            }
        }

        private bool ValidateCurs()
        {
            if (string.IsNullOrWhiteSpace(txtDenumire.Text) || string.IsNullOrWhiteSpace(txtFormator.Text))
            { MessageBox.Show("Название и Преподаватель обязательны!"); return false; }
            if (!decimal.TryParse(txtPret.Text, out decimal pret) || pret <= 0)
            { MessageBox.Show("Цена должна быть числом больше 0!"); return false; }
            if (!int.TryParse(txtDurata.Text, out int dur) || dur <= 0)
            { MessageBox.Show("Длительность должна быть числом больше 0!"); return false; }
            return true;
        }

       
        private void LoadInscrieri()
        {
            string q = @"SELECT i.IdInscriere, CONCAT(c.Nume, ' ', c.Prenume) AS Cursant, cr.Denumire AS Curs, 
                                i.DataInscriere, i.StatusPlata 
                         FROM Inscriere i
                         JOIN Cursant c ON i.IdCursant = c.IdCursant
                         JOIN Curs cr ON i.IdCurs = cr.IdCurs";
            GridInscrieri.ItemsSource = ExecuteSelect(q).DefaultView;
        }

        private void LoadComboBoxes()
        {
            cmbCursant.ItemsSource = ExecuteSelect("SELECT IdCursant, CONCAT(Nume, ' ', Prenume) AS FullName FROM Cursant").DefaultView;
            cmbCurs.ItemsSource = ExecuteSelect("SELECT IdCurs, Denumire FROM Curs").DefaultView;
        }

        private void BtnAddInscriere_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCursant.SelectedValue == null || cmbCurs.SelectedValue == null || cmbStatus.SelectedItem == null)
            { MessageBox.Show("Выберите слушателя, курс и статус!"); return; }

            try
            {
                string status = (cmbStatus.SelectedItem as ComboBoxItem).Content.ToString();
                ExecuteQuery("INSERT INTO Inscriere (IdCursant, IdCurs, DataInscriere, StatusPlata) VALUES (@ic, @c, CURDATE(), @s)",
                new[] { new MySqlParameter("@ic", cmbCursant.SelectedValue), new MySqlParameter("@c", cmbCurs.SelectedValue), new MySqlParameter("@s", status) });
                LoadAllData();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) MessageBox.Show("Ошибка: Этот слушатель уже записан на этот курс!");
                else MessageBox.Show(ex.Message);
            }
        }

        private void BtnDeleteInscriere_Click(object sender, RoutedEventArgs e)
        {
            if (GridInscrieri.SelectedItem == null) return;
            DataRowView row = (DataRowView)GridInscrieri.SelectedItem;
            ExecuteQuery("DELETE FROM Inscriere WHERE IdInscriere=@id", new[] { new MySqlParameter("@id", row["IdInscriere"]) });
            LoadAllData();
        }

       
        private void LoadReportAndStats()
        {
         
            string reportQuery = @"
                SELECT CONCAT(c.Nume, ' ', c.Prenume) AS Cursant, 
                       COUNT(i.IdInscriere) AS NumarInscrieri, 
                       SUM(CASE WHEN i.StatusPlata='Оплачено' THEN cr.Pret ELSE 0 END) AS TotalAchitat
                FROM Cursant c
                LEFT JOIN Inscriere i ON c.IdCursant = i.IdCursant
                LEFT JOIN Curs cr ON i.IdCurs = cr.IdCurs
                GROUP BY c.IdCursant
                ORDER BY TotalAchitat DESC";

            DataTable dtReport = ExecuteSelect(reportQuery);
            GridReport.ItemsSource = dtReport.DefaultView;

         
            DataTable dtTotal = ExecuteSelect("SELECT COUNT(*) FROM Cursant");
            lblTotalStudents.Text = dtTotal.Rows[0][0].ToString();

            DataTable dtRevenue = ExecuteSelect("SELECT SUM(cr.Pret) FROM Inscriere i JOIN Curs cr ON i.IdCurs=cr.IdCurs WHERE i.StatusPlata='Оплачено'");
            string totalRev = dtRevenue.Rows[0][0].ToString();
            lblTotalRevenue.Text = string.IsNullOrEmpty(totalRev) ? "0 lei" : totalRev + " lei";

            if (!string.IsNullOrEmpty(totalRev) && Convert.ToInt32(dtTotal.Rows[0][0]) > 0)
            {
                decimal avg = Convert.ToDecimal(totalRev) / Convert.ToDecimal(dtTotal.Rows[0][0]);
                lblAvgRevenue.Text = avg.ToString("0.00") + " lei";
            }
            else { lblAvgRevenue.Text = "0 lei"; }

            DataTable dtTopCourse = ExecuteSelect(@"
                SELECT cr.Denumire, COUNT(i.IdInscriere) as cnt 
                FROM Inscriere i JOIN Curs cr ON i.IdCurs=cr.IdCurs 
                GROUP BY cr.IdCurs ORDER BY cnt DESC LIMIT 1");
            if (dtTopCourse.Rows.Count > 0)
                lblTopCourse.Text = $"{dtTopCourse.Rows[0]["Denumire"]} ({dtTopCourse.Rows[0]["cnt"]} рег.)";
        }

        private void BtnLoadReport_Click(object sender, RoutedEventArgs e) => LoadReportAndStats();

      
        private void BtnExportTXT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = "Raport.txt";
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("--- ОТЧЕТ ПО СЛУШАТЕЛЯМ ---");
                    sw.WriteLine($"Сгенерирован: {DateTime.Now}\n");
                    foreach (DataRowView row in GridReport.ItemsSource)
                    {
                        sw.WriteLine($"Слушатель: {row["Cursant"]} | Курсов: {row["NumarInscrieri"]} | Оплачено: {row["TotalAchitat"]} lei");
                    }
                    sw.WriteLine("\n--- СТАТИСТИКА ---");
                    sw.WriteLine($"Всего студентов: {lblTotalStudents.Text}");
                    sw.WriteLine($"Общая выручка: {lblTotalRevenue.Text}");
                }
                MessageBox.Show("Отчет успешно сохранен в файл Raport.txt (в папке с программой)!");
            }
            catch (Exception ex) { MessageBox.Show("Ошибка экспорта: " + ex.Message); }
        }
    }
}