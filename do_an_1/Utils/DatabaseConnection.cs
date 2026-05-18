using System;
using Microsoft.Data.SqlClient;

namespace do_an_1.Utils
{
    public static class DatabaseConnection
    {
        private static readonly string connectionString =
            "Server=LAPTOP-BETZI\\SQLEXPRESS;" +
            "Database=STUDY_TRACKER;" +
            "Integrated Security=True;" +              // ← Windows Auth
            "TrustServerCertificate=True;" +
            "MultipleActiveResultSets=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static void TestConnection()
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    System.Windows.Forms.MessageBox.Show(
                        "✅ Kết nối SQL Server thành công!",
                        "Study Tracker",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(
                        $"❌ Lỗi kết nối SQL Server:\n{ex.Message}",
                        "Study Tracker",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }
    }
}