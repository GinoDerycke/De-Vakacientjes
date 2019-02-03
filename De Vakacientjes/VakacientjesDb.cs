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
            name = name.Trim();
            email = email.Trim();

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
            firstName = firstName.Trim();
            lastName = lastName.Trim();

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
            firstName = firstName.Trim();
            lastName = lastName.Trim();

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
            familyName = familyName.Trim();

            Family family = new Family();

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = $"select * from family where lower(name) = lower('{familyName}')";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    family.Id = Convert.ToInt32(reader["id"]);
                    family.Name = reader["name"].ToString();
                    family.Email = reader["email"].ToString();
                }
                reader.Close();

                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.GetFamily failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return family;
        }

        public static Child GetChild(string firstName, string lastName)
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();

            Child child = new Child();

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = "select c.id as c_id, c.family_id, c.first_name, c.last_name, f.id as f_id " +
                             $"from child c, family f where (lower(c.first_name) = lower('{firstName}')) and (lower(c.last_name) = lower('{lastName}')) and (c.family_id = f.id)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    child.Id = Convert.ToInt32(reader["c_id"]);
                    child.FamilyId = Convert.ToInt32(reader["family_id"]);
                    child.FirstName = reader["first_name"].ToString();
                    child.LastName = reader["last_name"].ToString();
                }
                reader.Close();

                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.GetChild failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return child;
        }

        public static Child GetChild(string fullName)
        {
            fullName = fullName.Trim();

            Child child = new Child();

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = "select c.id as c_id, c.family_id, c.first_name, c.last_name, f.id as f_id " +
                             $"from child c, family f where (lower(CONCAT(c.first_name, ' ', c.last_name)) = lower('{fullName}')) and (c.family_id = f.id)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    child.Id = Convert.ToInt32(reader["c_id"]);
                    child.FamilyId = Convert.ToInt32(reader["family_id"]);
                    child.FirstName = reader["first_name"].ToString();
                    child.LastName = reader["last_name"].ToString();
                }
                reader.Close();

                if (child.Id == -1)
                {
                    sql = "select c.id as c_id, c.family_id, c.first_name, c.last_name, f.id as f_id " +
                           $"from child c, family f where (lower(CONCAT(c.last_name, ' ', c.first_name)) = lower('{fullName}')) and (c.family_id = f.id)";
                    cmd.CommandText = sql;
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        child.Id = Convert.ToInt32(reader["c_id"]);
                        child.FamilyId = Convert.ToInt32(reader["family_id"]);
                        child.FirstName = reader["first_name"].ToString();
                        child.LastName = reader["last_name"].ToString();
                    }
                    reader.Close();
                }

                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.GetChild failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return child;
        }

        public static Parent GetParent(string firstName, string lastName)
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();

            Parent parent = new Parent();

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = "select p.id as p_id, p.family_id, p.first_name, p.last_name, f.id as f_id " +
                             $"from parent p, family f where (lower(p.first_name) = lower('{firstName}')) and (lower(p.last_name) = lower('{lastName}')) and (p.family_id = f.id)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    parent.Id = Convert.ToInt32(reader["p_id"]);
                    parent.FamilyId = Convert.ToInt32(reader["family_id"]);
                    parent.FirstName = reader["first_name"].ToString();
                    parent.LastName = reader["last_name"].ToString();
                }
                reader.Close();

                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.GetParent failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return parent;
        }

        public static Parent GetParent(string fullName)
        {
            fullName = fullName.Trim();

            Parent parent = new Parent();

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = "select p.id as p_id, p.family_id, p.first_name, p.last_name, f.id as f_id " +
                             $"from parent p, family f where (lower(CONCAT(p.first_name, ' ', p.last_name)) = lower('{fullName}')) and (p.family_id = f.id)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    parent.Id = Convert.ToInt32(reader["p_id"]);
                    parent.FamilyId = Convert.ToInt32(reader["family_id"]);
                    parent.FirstName = reader["first_name"].ToString();
                    parent.LastName = reader["last_name"].ToString();
                }
                reader.Close();

                if (parent.Id == -1)
                {
                    sql = "select p.id as p_id, p.family_id, p.first_name, p.last_name, f.id as f_id " +
                           $"from parent p, family f where (lower(CONCAT(p.last_name, ' ', p.first_name)) = lower('{fullName}')) and (p.family_id = f.id)";
                    cmd.CommandText = sql;

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        parent.Id = Convert.ToInt32(reader["p_id"]);
                        parent.FamilyId = Convert.ToInt32(reader["family_id"]);
                        parent.FirstName = reader["first_name"].ToString();
                        parent.LastName = reader["last_name"].ToString();
                    }
                    reader.Close();
                }

                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.GetParent failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return parent;
        }

        public static List<Family> GetFamilies()
        {
            List<Family> list = new List<Family>();

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = "select * from family order by name";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Family family = new Family();
                    family.Id = Convert.ToInt32(reader["id"]);
                    family.Name = reader["name"].ToString();
                    family.Email = reader["email"].ToString();
                    list.Add(family);
                }
                reader.Close();

                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.GetFamilies failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return list;
        }

        public static List<Child> GetChildren()
        {
            List<Child> list = new List<Child>();

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = "select * from child order by last_name, first_name";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Child child = new Child();
                    child.Id = Convert.ToInt32(reader["id"]);
                    child.FamilyId = Convert.ToInt32(reader["family_id"]);
                    child.FirstName = reader["first_name"].ToString();
                    child.LastName = reader["last_name"].ToString();
                    list.Add(child);
                }
                reader.Close();

                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.GetChildren failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return list;
        }

        public static List<Parent> GetParents()
        {
            List<Parent> list = new List<Parent>();

            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();

                string sql = "select * from parent order by last_name, first_name";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Parent parent = new Parent();
                    parent.Id = Convert.ToInt32(reader["id"]);
                    parent.FamilyId = Convert.ToInt32(reader["family_id"]);
                    parent.FirstName = reader["first_name"].ToString();
                    parent.LastName = reader["last_name"].ToString();
                    list.Add(parent);
                }
                reader.Close();

                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show($"VakacientjesDb.GetParents failed.\n\n{e.ToString()}");

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return list;
        }
    }
}
