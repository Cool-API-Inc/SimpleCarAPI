using CarAPI.ResultTypes;
using System.Data.SqlClient;

/* კლასი ემსახურება ბაზიდან მანქანის ამოღებას */

namespace CarAPI.CrudOperations
{
    public static class Deletion
    {
        public static DeletionResult DeleteCar(string id)
        {
            if (id == null)
                return new DeletionResult(false);

            SqlCommand command = new SqlCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "DELETE FROM CAR_INFO WHERE CarID = @id";
            command.Parameters.AddWithValue("@id", id);

            if (DatabaseConnector.ExecuteCommand(command))
                return new DeletionResult(true);
            else
                return new DeletionResult(false);

        }
    }
}
