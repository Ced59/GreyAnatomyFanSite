using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GreyAnatomyFanSite.Models.Surveys;

namespace GreyAnatomyFanSite.Tools.Bdds.BddSurveys
{
    public class BddSurveys
    {
        private static BddSurveys _instance = null;
        private static readonly object _lock = new object();

        public static BddSurveys Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new BddSurveys();
                    }
                    return _instance;
                }
            }
        }

        public List<Answer> AddAnswer(Answer answer)
        {
            IDbCommand command = new SqlCommand("INSERT INTO Answers (Label, IdSurvey) VALUES (@Label, @IdSurvey)", (SqlConnection)ConnectionSurveys.Instance);
            command.Parameters.Add(new SqlParameter("@Label", SqlDbType.VarChar) { Value = answer.Label });
            command.Parameters.Add(new SqlParameter("@IdSurvey", SqlDbType.Int) { Value = answer.IdSurvey });
            ConnectionSurveys.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            List<Answer> answers = GetAnswersById(answer.IdSurvey);
            ConnectionSurveys.Instance.Close();
            return answers;
            
        }

        public List<Survey> GetAllSurveys()
        {
            List<Survey> surveys = new List<Survey>();
            IDbCommand command = new SqlCommand("SELECT * FROM Surveys ORDER BY DateCreation DESC", (SqlConnection)ConnectionSurveys.Instance);
            ConnectionSurveys.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            while (reader.Read())
            {
                Survey s = new Survey();
                s.Titre = reader.GetString(1);
                s.Id = reader.GetInt32(0);
                s.IdCreateur = reader.GetInt32(2);
                s.Online = ConvertIntBool.ConvertIntToBool(reader.GetInt32(3));
                s.DateCreation = reader.GetDateTime(4);
                surveys.Add(s);
            }
            reader.Close();
            command.Dispose();

            foreach (Survey s in surveys)
            {
                s.Answers = GetAnswersById(s.Id);
            }
            ConnectionSurveys.Instance.Close();
            return surveys;
        }

        public Survey GetSurvey(Survey survey)
        {
            Survey s = new Survey();
            IDbCommand command = new SqlCommand("SELECT * FROM Surveys WHERE Id = @Id", (SqlConnection)ConnectionSurveys.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = survey.Id });
            ConnectionSurveys.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            reader.Read();
            s.Titre = reader.GetString(1);
            s.Id = reader.GetInt32(0);
            s.IdCreateur = reader.GetInt32(2);
            s.DateCreation = reader.GetDateTime(4);
            reader.Close();
            command.Dispose();
            s.Answers = GetAnswersById(s.Id);
            ConnectionSurveys.Instance.Close();
            return s;

        }

        public void Validation(Survey survey)
        {
            IDbCommand command = new SqlCommand("UPDATE Surveys SET Online = '1', DateCreation = @DateCreation WHERE Id = @Id", (SqlConnection)ConnectionSurveys.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = survey.Id });
            command.Parameters.Add(new SqlParameter("@DateCreation", SqlDbType.DateTime) { Value = DateTime.Now });
            ConnectionSurveys.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSurveys.Instance.Close();
        }

        public List<Answer> DeleteAnswer(Answer answer)
        {
            IDbCommand command = new SqlCommand("DELETE FROM Answers WHERE Id = @Id", (SqlConnection)ConnectionSurveys.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = answer.Id });
            ConnectionSurveys.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            List<Answer> answers = GetAnswersById(answer.IdSurvey);
            ConnectionSurveys.Instance.Close();
            return answers;
        }

        public Survey AddSurvey(Survey survey)
        {
            IDbCommand command = new SqlCommand("INSERT INTO Surveys (Titre, IdCreateur, DateCreation) OUTPUT INSERTED.ID VALUES (@Titre, @IdCreateur, @DateCreation)", (SqlConnection)ConnectionSurveys.Instance);
            command.Parameters.Add(new SqlParameter("@Titre", SqlDbType.VarChar) { Value = survey.Titre });
            command.Parameters.Add(new SqlParameter("@IdCreateur", SqlDbType.Int) { Value = survey.IdCreateur });
            command.Parameters.Add(new SqlParameter("@DateCreation", SqlDbType.DateTime) { Value = DateTime.Now });
            ConnectionSurveys.Instance.Open();
            survey.Id = (int)command.ExecuteScalar();
            command.Dispose();
            ConnectionSurveys.Instance.Close();
            return survey;
        }


        private List<Answer> GetAnswersById(int IdSurvey)
        {
            List<Answer> answers = new List<Answer>();
            IDbCommand command = new SqlCommand("SELECT * FROM Answers WHERE IdSurvey = @IdSurvey", (SqlConnection)ConnectionSurveys.Instance);
            command.Parameters.Add(new SqlParameter("@IdSurvey", SqlDbType.Int) { Value = IdSurvey });
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            while (reader.Read())
            {
                Answer a = new Answer();
                a.Id = reader.GetInt32(0);
                a.Label = reader.GetString(1);
                a.IdSurvey = reader.GetInt32(2);
                answers.Add(a);
            }
            reader.Close();
            command.Dispose();
            return answers;
        }
    }
}
