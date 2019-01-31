using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace De_Vakacientjes
{
    static class VakacientjesDb
    {
        private static string connectionString = "server=localhost;user=root;database=vakacientjes;port=3306;password=2631";

        public static bool AddFamily(string name, string email)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = $"insert into family(name, email) values ('{name}', '{email}')";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                conn.Close();

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.AddFamily failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                return false;
            }
        }

        public static bool AddChild(int familyId, string firstName, string lastName)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = $"insert into child(family_id, first_name, last_name) values ({familyId}, '{firstName}', '{lastName}')";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                conn.Close();

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.AddChild failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                return false;
            }
        }

        public static bool AddParent(int familyId, string firstName, string lastName)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = $"insert into parent(family_id, first_name, last_name) values ({familyId}, '{firstName}', '{lastName}')";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                conn.Close();

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.AddParent failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                return false;
            }
        }

        public static Family GetFamily(string familyName)
        {
            Family family;

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = $"select * from family where name = '{familyName}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    family.Id = Convert.ToInt32(reader["id"]);
                    family.Name = reader["name"];
                    family.Email = reader["email"];
                }
                reader.Close();

                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.GetFamilyId failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return family;
        }

        public static Family GetChild(string firstName, string lastName)
        {
            ...
            Child child;

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = $"select * from child, family where (first_name = '{firstName}') and (last_name = '{lastName}')";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    child.Id = Convert.ToInt32(reader["id"]);
                    child.FamilyId = reader["name"];
                    child.Email = reader["email"];
                }
                reader.Close();

                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.GetFamilyId failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return family;
        }

    }
}
