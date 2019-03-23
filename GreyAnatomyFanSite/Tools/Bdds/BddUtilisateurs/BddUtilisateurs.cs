using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

using GreyAnatomyFanSite.Tools;

namespace GreyAnatomyFanSite.Models
{
    public class BddUtilisateurs
    {
        private static BddUtilisateurs _instance = null;
        private static readonly object _lock = new object();

        public static BddUtilisateurs Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new BddUtilisateurs();
                    }
                    return _instance;
                }
            }
        }


        public void UpdateAvatar(Membres m)
        {
            IDbCommand command = new SqlCommand("UPDATE Membres SET Avatar = @Avatar WHERE Id = @Id", (SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@Avatar", SqlDbType.VarChar) { Value = m.Avatar });
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = m.IdMembre });
            ConnectionUtilisateurs.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionUtilisateurs.Instance.Close();
        }


        public Membres GetMembreByNo(Membres m)
        {
            IDbCommand command = new SqlCommand("SELECT Id, Pseudo, Mail, Statut FROM Membres WHERE NumeroUnique = @NumeroUnique", (SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@NumeroUnique", SqlDbType.VarChar) { Value = m.NoUnique });
            ConnectionUtilisateurs.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            if(!reader.Read())
            {
                reader.Close();
                command.Dispose();
                ConnectionUtilisateurs.Instance.Close();
                return null;
            }
            
            else
            {
                m = new Membres { IdMembre = reader.GetInt32(0), Pseudo = reader.GetString(1), Statut = reader.GetString(3), Mail = reader.GetString(2) };
                reader.Close();
                command.Dispose();

                if (m.Statut == "Inactif")
                {
                    command = new SqlCommand("UPDATE Membres SET Statut = @Statut WHERE Id = @Id", (SqlConnection)ConnectionUtilisateurs.Instance);
                    command.Parameters.Add(new SqlParameter("@Statut", SqlDbType.VarChar) { Value = "Membre" });
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = m.IdMembre });
                    command.ExecuteNonQuery();
                }
                
                command.Dispose();
                ConnectionUtilisateurs.Instance.Close();
                return m;
            }
        }

        public Membres GetMembreById(int id)
        {
            IDbCommand command = new SqlCommand("SELECT * FROM Membres WHERE Id = @Id", (SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            ConnectionUtilisateurs.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            reader.Read();
            Membres m = new Membres { Pseudo = reader.GetString(1), IdMembre = reader.GetInt32(0), Avatar = reader.GetString(6) };
            reader.Close();
            command.Dispose();
            ConnectionUtilisateurs.Instance.Close();
            return m;
        }



        public string GetNoUniqueMembre(Membres m)
        {
            IDbCommand command = new SqlCommand("SELECT NumeroUnique FROM Membres WHERE Mail = @Mail", (SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@Mail", SqlDbType.VarChar) { Value = m.Mail });
            ConnectionUtilisateurs.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            reader.Read();
            string NoUnique = (string)reader.GetValue(0);
            reader.Close();
            command.Dispose();
            ConnectionUtilisateurs.Instance.Close();
            return NoUnique;
        }


        public Membres ComparePassword (string mail, string HashPassword)
        {

            IDbCommand command = new SqlCommand("SELECT Password FROM Membres WHERE Mail = @Mail",(SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@Mail", SqlDbType.VarChar) { Value = mail });
            ConnectionUtilisateurs.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            reader.Read();
            string PassWord = (string)reader.GetValue(0);
            reader.Close();
            command.Dispose();
            ConnectionUtilisateurs.Instance.Close();
            if (PassWord == HashPassword)
            {

                command = new SqlCommand("SELECT Id, Pseudo, Avatar, Statut, NumeroUnique FROM Membres WHERE Mail = @Mail",(SqlConnection)ConnectionUtilisateurs.Instance);
                command.Parameters.Add(new SqlParameter("@Mail", SqlDbType.VarChar) { Value = mail });
                ConnectionUtilisateurs.Instance.Open();
                reader = (SqlDataReader)command.ExecuteReader();
                reader.Read();

                Membres m = new Membres { IdMembre = reader.GetInt32(0), Pseudo = reader.GetString(1), Avatar = reader.GetString(2), Statut = reader.GetString(3), NoUnique = reader.GetString(4), Mail = mail };
                reader.Close();
                command.Dispose();
                ConnectionUtilisateurs.Instance.Close();
                return m;
            }
            else
            {
                Membres m = new Membres { Mail = mail};
                return m;
            }

        }



        public void RegisterMembre(Membres m)
        {
            IDbCommand command = new SqlCommand("INSERT INTO Membres (Pseudo, Mail, Password, NumeroUnique, DateInscription) OUTPUT INSERTED.ID VALUES (@Pseudo, @Mail, @Password, @NumeroUnique, @DateInscription)", (SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@Pseudo", SqlDbType.VarChar) { Value = m.Pseudo });
            command.Parameters.Add(new SqlParameter("@Mail", SqlDbType.VarChar) { Value = m.Mail });
            command.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar) { Value = m.Password });
            command.Parameters.Add(new SqlParameter("@NumeroUnique", SqlDbType.VarChar) { Value = m.NoUnique });
            command.Parameters.Add(new SqlParameter("@DateInscription", SqlDbType.DateTime) { Value = m.DateInscription });

            ConnectionUtilisateurs.Instance.Open();
            int Id = (int)command.ExecuteScalar();
            command.Dispose();
            command = new SqlCommand("UPDATE IP SET IdMembre = @IdMembre WHERE IP = @IP", (SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@IdMembre", SqlDbType.Int) { Value = Id });
            command.Parameters.Add(new SqlParameter("@IP", SqlDbType.VarChar) { Value = m.Ip });
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionUtilisateurs.Instance.Close();
        }

        public void UpdatePassword(Membres m)
        {
            IDbCommand command = new SqlCommand("UPDATE Membres SET Password = @Password WHERE NumeroUnique = @NumeroUnique", (SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@NumeroUnique", SqlDbType.VarChar) { Value = m.NoUnique });
            command.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar) { Value = m.Password });
            ConnectionUtilisateurs.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionUtilisateurs.Instance.Close();
        }

        public bool PseudoExist(Membres m)
        {
            bool Exist = false;
            IDbCommand command = new SqlCommand("SELECT * FROM Membres WHERE Pseudo = @Pseudo",(SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@Pseudo", SqlDbType.VarChar) { Value = m.Pseudo });
            ConnectionUtilisateurs.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            if (reader.Read())
            {
                Exist = true;
            }
            reader.Close();
            command.Dispose();
            ConnectionUtilisateurs.Instance.Close();
            return Exist;
        }


        public bool MailExist(Membres m)
        {
            bool Exist = false;
            IDbCommand command = new SqlCommand("SELECT * FROM Membres WHERE Mail = @Mail", (SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@Mail", SqlDbType.VarChar) { Value = m.Mail });
            ConnectionUtilisateurs.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            if (reader.Read())
            {
                Exist = true;
            }
            reader.Close();
            command.Dispose();
            ConnectionUtilisateurs.Instance.Close();
            return Exist;
        }


        public int GetVisit(Visiteur v)
        {
            int IdIp = 0;
            IDbCommand command = new SqlCommand("SELECT Id FROM IP WHERE IP = @IP",(SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@IP", SqlDbType.VarChar) { Value = v.Ip });
            ConnectionUtilisateurs.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            if (!reader.Read())
            {
                reader.Close();
                command.Dispose();
                command = new SqlCommand("INSERT INTO IP (IP) OUTPUT INSERTED.ID VALUES (@IP)", (SqlConnection)ConnectionUtilisateurs.Instance);
                command.Parameters.Add(new SqlParameter("@IP", SqlDbType.VarChar) { Value = v.Ip });
                IdIp = (int)command.ExecuteScalar();
                command.Dispose();
            }
            else
            {
                IdIp = (int)reader.GetValue(0);
                reader.Close();
                command.Dispose();
            }

            command = new SqlCommand("SELECT * FROM Visites WHERE IdIP = @IdIP AND Date = @Date",(SqlConnection)ConnectionUtilisateurs.Instance);
            command.Parameters.Add(new SqlParameter("@IdIP", SqlDbType.Int) { Value = IdIp });
            command.Parameters.Add(new SqlParameter("@Date", SqlDbType.Date) { Value = v.Date });     
            reader = (SqlDataReader)command.ExecuteReader();

            if (!reader.Read())
            {
                reader.Close();
                command.Dispose();
                command = new SqlCommand("INSERT INTO Visites (IdIP, Date, NbreVue) VALUES (@IdIP, @Date, @NbreVue)", (SqlConnection)ConnectionUtilisateurs.Instance);
                command.Parameters.Add(new SqlParameter("@IdIP", SqlDbType.Int) { Value = IdIp });
                command.Parameters.Add(new SqlParameter("@Date", SqlDbType.Date) { Value = v.Date });
                command.Parameters.Add(new SqlParameter("@NbreVue", SqlDbType.Int) { Value = 1 });
                command.ExecuteNonQuery();

            }

            else
            {
                int NbreVue = (int)reader.GetValue(3);
                int id = (int)reader.GetValue(0);
                reader.Close();
                command.Dispose();
                command = new SqlCommand("UPDATE Visites SET NbreVue = @NbreVue WHERE Id = @Id", (SqlConnection)ConnectionUtilisateurs.Instance);
                command.Parameters.Add(new SqlParameter("@NbreVue", SqlDbType.Int) { Value = NbreVue + 1 });
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
                command.ExecuteNonQuery();
            }

            command.Dispose();
            
            command = new SqlCommand("SELECT COUNT (*) FROM Visites", (SqlConnection)ConnectionUtilisateurs.Instance);
            
            int VisitUnique = (int)command.ExecuteScalar();
            command.Dispose();
            ConnectionUtilisateurs.Instance.Close();
            return VisitUnique;
        }
    }
}
