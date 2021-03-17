using CarAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

/* კლასში მოცემულია სტატიკური ფუნქციები MS SQL ბაზასთან სამუშაოდ */

namespace CarAPI
{
    public static class DatabaseConnector
    {
        // შეცვალეთ მისამართი პროგრამის გაშვებამდე 
        private static string connString = @"Server=localhost\SQLEXPRESS;Database=CAR_API_DB;Trusted_Connection=True;";

        // ფუნქცია უშვებს SQL ბრძანებას სკალარულად და აბრუნებს შედეგს (წარმატებით გაეშვა თუ არა)
        public static bool ExecuteCommand(SqlCommand command)
        {
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                command.Connection = conn;
                conn.Open();

                int affected = command.ExecuteNonQuery();
                command.Clone();
                
                if (affected != 0)  
                    return true;
                else
                    return false;

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }


        static object SafeValue(object par) => (par == DBNull.Value ? null : par);

        // ფუნქცია უშვებს SQL ბრძანებას და შედეგს აბრუნებს Car ობიექტების კოლექციის სახით
        public static List<Car> ExecuteWithReader(SqlCommand command)
        {
            List<Car> cars = new List<Car>();
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                command.Connection = conn;
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cars.Add(new Car(
                            (string)SafeValue(reader["CarID"]), (string)SafeValue(reader["Brand"]), 
                            (int)SafeValue(reader["PYear"]), (string)SafeValue(reader["CarDesc"]), 
                            (string)SafeValue(reader["ImgUrl"]), (int)SafeValue(reader["Features"]),
                            (double)SafeValue(reader["Cost"]), (string)SafeValue(reader["Currency"]), 
                            (double)SafeValue(reader["CostGel"])
                        )
                    );
                }

                return cars;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
