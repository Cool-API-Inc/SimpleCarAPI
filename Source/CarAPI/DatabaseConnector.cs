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
        private static string connString = @"[CONNECTION STRING HERE]]";

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
                    cars.Add(new Car((string)reader["CarID"], (string)reader["Brand"], (int)reader["PYear"],
                        (string)reader["CarDesc"], (string)reader["ImgUrl"], (int)reader["Features"],
                        (double)reader["Cost"], (string)reader["Currency"], (double)reader["CostGel"]));
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
