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

            return GetAnswersById(answer.IdSurvey);
            
        }

        public Survey GetTitle(Survey survey)
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
            reader.Close();
            command.Dispose();
            ConnectionSurveys.Instance.Close();
            return s;

        }

        public void Validation(Survey survey)
        {
            IDbCommand command = new SqlCommand("UPDATE Surveys SET Online = '1' WHERE Id = @Id", (SqlConnection)ConnectionSurveys.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = survey.Id });
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

            return GetAnswersById(answer.IdSurvey);
        }

        public Survey AddSurvey(Survey survey)
        {
            IDbCommand command = new SqlCommand("INSERT INTO Surveys (Titre, IdCreateur) OUTPUT INSERTED.ID VALUES (@Titre, @IdCreateur)", (SqlConnection)ConnectionSurveys.Instance);
            command.Parameters.Add(new SqlParameter("@Titre", SqlDbType.VarChar) { Value = survey.Titre });
            command.Parameters.Add(new SqlParameter("@IdCreateur", SqlDbType.Int) { Value = survey.IdCreateur });
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
            ConnectionSurveys.Instance.Close();
            return answers;
        }
    }
}
