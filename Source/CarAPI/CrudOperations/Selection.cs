using CarAPI.Models;
using CarAPI.ResultTypes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

/* კლასი ემსახურება ბაზაში რეგისტრირებული მანქანების მონაცემების წაკითხვას */

namespace CarAPI.CrudOperations
{
    public static class Selection
    {
        public static SelectionResult GetCar(string id)
        {
            if (id != null)  // თუ id მითითებულია ვაბრუნებთ მის შესაბამის ჩანაწერს
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = "SELECT CarID, Brand, PYear, CarDesc, ImgUrl, Features, Cost, Currency, CostGel " +
                        "FROM CAR_INFO WHERE CarID = @id";
                    command.Parameters.AddWithValue("@id", id);

                    List<Car> cars = DatabaseConnector.ExecuteWithReader(command);

                    if (cars.Count > 0)
                        return new SelectionResult(true, cars);
                    else
                        return new SelectionResult(false);

                }
                catch (Exception)
                {
                    return new SelectionResult(false);
                }
            }
            else // წინააღმდეგ შემთხვევაში ვაბრუნებთ ყველა მნიშვნელობას
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT CarID, Brand, PYear, CarDesc, ImgUrl, Features, Cost, Currency, CostGel " +
                        "FROM CAR_INFO";
                    return new SelectionResult(true, DatabaseConnector.ExecuteWithReader(command));
                }
                catch (Exception)
                {
                    return new SelectionResult(false);
                }
            }
        }
    }
}
