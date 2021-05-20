using System;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Collections.Specialized;
namespace Employee
{
    /*
        This program will ask a user to enter in 5 options. Those options are to insert a user, delete a user, 
        update a user, show all users or quit the program. This program stores all user input in a MySql database.
        The database will store the first and last name and the job title that was entered. Each record
        will also have a unique id associated with it because two people can work in the same job title
        and have the same name, so there would need to be a way to identify each user. I am also assuming the
        user will enter a valid name and a valid job title. This ia all done through the console.
      
     */
    class Program
    {
        /*The main method will ask the user to enter in a option and a specfic action will occur if the valid
         * option is entered.
         */
        static void Main(string[] args)
        {
            using var con = getDBConnection();
            con.Open();

            //ask user for an option to enter if the correct option is not listed the program
            //will keep asking the user for a valid option.
            Console.WriteLine("1: insert a user");
            Console.WriteLine("2: delete a user");
            Console.WriteLine("3: update a user");
            Console.WriteLine("4: show all users");
            Console.WriteLine("5: quit");
            Console.WriteLine("Enter a value between 1 to 5");
            int n;
            string res = Console.ReadLine();

            while (!int.TryParse(res, out n) || (n > 5 || n < 0))
            {
                Console.WriteLine(n);
                Console.WriteLine("Not a vaild value");
                Console.WriteLine("Enter a value between 1 to 5");
                res = Console.ReadLine();
            }

            //if the user enters a right value it execute that value and keep asking
            // the user for a valid option until they want to quit.
            while (n > 0 && n < 5 || (!int.TryParse(res, out n)))
            {
                //depending on the option the user makes one of these statements will be executed.
                switch (n)
                {
                    case 1:
                        insert();
                        break;
                    case 2:
                        delete();
                        break;
                    case 3:
                        update();
                        break;
                    case 4:
                        showAll();
                        break;

                    default:
                        Console.WriteLine("Default case");
                        Console.WriteLine("Not a valid value");
                        Console.WriteLine("Enter a value between 1 to 5");
                        res = Console.ReadLine();
                        int.TryParse(res, out n);
                        break;

                }

                // ask the user for an option 
                Console.WriteLine("1: insert a user");
                Console.WriteLine("2: delete a user");
                Console.WriteLine("3: update a user");
                Console.WriteLine("4: show all users");
                Console.WriteLine("5: quit");
                Console.WriteLine("Enter a value between 1 to 5");
                res = Console.ReadLine();
                int.TryParse(res, out n);
                
                //if the option is not value print a message
                if (n < 0 || n > 5)
                {
                    Console.WriteLine("Not a valid value");
                    Console.WriteLine("Enter a value between 1 to 5");

                    res = Console.ReadLine();
                    int.TryParse(res, out n);
                    continue;
                }

            }
            con.Close();

        }


        /*This method will insert a user by asking for a first name, last name and job title. I am
         * assuming the user will enter in valid first and last names.  
         */
        public static void insert()
        {
            String fname, lname, job;

            try
            {
                //get the user's name and job title
                Console.WriteLine("Please enter a valid first Name");
                fname = Console.ReadLine();

                //check to make sure the values entered is not empty or null
                while (string.IsNullOrEmpty(fname))
                {
                    Console.WriteLine("Please enter a valid first Name");
                    fname = Console.ReadLine();
                }
                Console.WriteLine("Please enter a valid last Name");
                lname = Console.ReadLine();

                while (string.IsNullOrEmpty(lname))
                {
                    Console.WriteLine("Please enter a valid last Name");
                    lname = Console.ReadLine();
                }

               
                Console.WriteLine("Enter job title");
                job = Console.ReadLine();
                while (string.IsNullOrEmpty(job))
                {
                    Console.WriteLine("Enter job title");
                    job = Console.ReadLine();
                }

                //insert to the database the user by using a parameterized query. 
                //if there is an error the user will not be inserted.
                using (MySqlConnection con = getDBConnection())
                {
                    string sql = @"INSERT INTO test (first_Name, Last_Name, job_Title) VALUES (@fname,@lname,@job)";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@fname", fname);
                    cmd.Parameters.AddWithValue("@lname", lname);
                    cmd.Parameters.AddWithValue("@job", job);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Console.WriteLine("User inserted");
                }
            }
            catch
            {
                Console.WriteLine("User not inserted");
            }
        }
        /*This method will delete a user by getting the right id. If the id exists the user will be deleted.
         * Else there will be a message that says the user was not deleted.
         */
        public static void delete()
        {

            int id;
            //deleting the user by getting the right id.
            try
            {
                Console.WriteLine("Enter Id of person you want to delete");
                id = Int32.Parse(Console.ReadLine());
                using (MySqlConnection con = getDBConnection())
                {
                    String q = @"DELETE FROM test WHERE emp_id=" + @id;
                    MySqlCommand cmd = new MySqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();

                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        Console.WriteLine("user not found");
                    }
                    else {

                        Console.WriteLine("user deleted");
                    }
                    con.Close();
                }
            }
            catch
            {
                Console.WriteLine("User id is not valid");

            }
        }

        /*This method will update all user's job in the database. This method will ask the user for the id of the person
         * that will be updated. If the id exists the user's job will be updated. 
        */
        public static void update()
        {
            int id;
            String job;


            try
            {
                Console.WriteLine("Enter Id of person's job title you want to update");
                id = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Enter the name of the new job title ");
                job = Console.ReadLine();
                using (MySqlConnection con = getDBConnection())
                {
                    
                    String q = @"UPDATE test SET job_Title =  @job  WHERE emp_id =  @id";
                    MySqlCommand cmd = new MySqlCommand(q, con);
                    cmd.Parameters.AddWithValue("@job", job);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        Console.WriteLine("user not found");
                    }
                    else
                    {

                        Console.WriteLine("user updated");
                    }
                    con.Close();
                    con.Close();
                }
            }
            catch { Console.WriteLine("data not updated"); }
        }

        /*This method is the connection to the database. Since this program will access the database
         * frequently no code will be repeated. This method gets the connection found in the app.config
         * file, since passwords and usernames should not be stored in a program.
         */
        public static MySqlConnection getDBConnection()
        {
            MySqlConnection databaseCon = null;
            string conStr = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            databaseCon = new MySqlConnection(conStr);
            return databaseCon;
        }

        /*This method will show all users in the database and format the info. 
         */
        public static void showAll()
        {
            using (MySqlConnection con = getDBConnection())
            {
                String q = "SELECT * from test";
                MySqlCommand cmd = new MySqlCommand(q, con);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine($"{"id",5} {"First Name",40} {"Last Name",40} {"Job Title",30}" + " ");

                //print each user's info and format the info.
                while (reader.Read())
                {

                    Console.Write($"{reader["emp_id"],5}");
                    Console.Write($"{reader["first_Name"],40}");
                    Console.Write($"{reader["last_Name"],40}");
                    Console.Write($"{reader["job_Title"],40}");

                    Console.WriteLine();
                }
                reader.Close();
                cmd.ExecuteNonQuery();
                con.Close();

            }

        }
    }
}
