using MySql.Data.MySqlClient;
using System;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
public class OperationOnBorrowers
{
    public int CreateAccount(DBConnection db, BorrowerInfo student)
    {
        try
        {
            if (db.IsConnected())
            {
                string query = "INSERT INTO borrower_info(borrower_id,Borrower_First_Name,Borrower_Last_Name,Phone_Number,address,email) value(@id,@fn,@ln,@ph,@ad,@email);";
                MySqlCommand command = new MySqlCommand(query, db.Connection);
                command.Parameters.AddWithValue("id", student.BorrowerId);
                command.Parameters.AddWithValue("fn", student.FirstName);
                command.Parameters.AddWithValue("ln", student.LastName);
                command.Parameters.AddWithValue("ph", student.PhoneNumber);
                command.Parameters.AddWithValue("ad", student.Address);
                command.Parameters.AddWithValue("email", student.Email);
                command.ExecuteNonQuery();
                string queryforlibrarynumber = "select library_card_number from borrower_info where borrower_id=@bid";
                MySqlCommand commandforlibrarynumber = new MySqlCommand(queryforlibrarynumber, db.Connection);
                commandforlibrarynumber.Parameters.AddWithValue("bid", student.BorrowerId);
                var reader = commandforlibrarynumber.ExecuteReader();
                while (reader.Read())
                {
                    int num = Convert.ToInt32(reader["library_card_number"]);
                    reader.Close();
                    return num;
                }
                reader.Close();
                return 0;
            }
            return -1;
        }
        catch (Exception e)
        {
            return -1;
        }
    }

    public bool IsUserValid(DBConnection db, BorrowerInfo borrower)
    {
        if (db.IsConnected())
        {
            string query = "select * from borrower_info where library_card_number=@ln AND borrower_id=@bi";
            MySqlCommand command = new MySqlCommand(query, db.Connection);
            command.Parameters.AddWithValue("ln", borrower.LibraryCardNum);
            command.Parameters.AddWithValue("bi", borrower.BorrowerId);
            var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return false;
            }
            reader.Close();
            return true;
        }
        return false;
    }
    public List<BookInfo> DisplayAvailableBookDetails(DBConnection db)
    {
        List<BookInfo> books = new List<BookInfo>();
        if (db.IsConnected())
        {
            string query = "select book_id,book_name,author_name,description_of_book from information where Available_Quantity>0 ORDER BY Book_Name;";
            MySqlCommand command = new MySqlCommand(query, db.Connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                BookInfo book = new BookInfo();
                book.BookId = reader["book_id"].ToString();
                book.BookName = reader["Book_Name"].ToString();
                book.AuthorName = reader["Author_Name"].ToString();
                book.Description = reader["description_of_book"].ToString();
                books.Add(book);
            }
            reader.Close();
        }
        return books;
    }
    public int BorrowABook(DBConnection db, BorrowerInfo student, BookInfo book)
    {
        try
        {
            string query = "select Number_of_bookissued from borrower_info where library_card_number = @l AND borrower_id=@b;";
            MySqlCommand command1 = new MySqlCommand(query, db.Connection);
            command1.Parameters.AddWithValue("l", student.LibraryCardNum);
            command1.Parameters.AddWithValue("b", student.BorrowerId);
            MySqlDataReader reader = command1.ExecuteReader();
            while (reader.Read())
            {
                if (Convert.ToInt32(reader["Number_of_bookissued"]) >= 3)
                {
                    reader.Close();
                    return 2; // User has already borrowed 3 books
                }
            }
            reader.Close();
            if (db.IsConnected())
            {
                string query1 = "UPDATE information SET Available_Quantity = Available_Quantity -1 where Book_id=@bid AND Available_Quantity>0;";
                MySqlCommand commandforinformation = new MySqlCommand(query1, db.Connection);
                commandforinformation.Parameters.AddWithValue("bid", book.BookId);
                int x = commandforinformation.ExecuteNonQuery();
                if (x == 0)
                    return 0;//No BOOK WITH THIS ID IS AVAILABLE
                string query2 = "UPDATE borrower_info SET Number_Of_bookissued = Number_Of_bookissued+1 where library_card_number = @lnum";
                MySqlCommand command = new MySqlCommand(query2, db.Connection);
                command.Parameters.AddWithValue("lnum", student.LibraryCardNum);
                command.ExecuteNonQuery();
                string query3 = "INSERT INTO borrowed_books(Book_Id,Issued_To_Library_Id) value(@bookid,@libid);";
                MySqlCommand commandforborrowedbooks = new MySqlCommand(query3, db.Connection);
                commandforborrowedbooks.Parameters.AddWithValue("bookid", book.BookId);
                commandforborrowedbooks.Parameters.AddWithValue("libid", student.LibraryCardNum);
                commandforborrowedbooks.ExecuteNonQuery();
                return 1; // Book borrowed successfully
            }
            return -1;
        }
        catch (Exception e)
        {

            return -1;
        }
    }
    public int ReturnABook(DBConnection db, BorrowerInfo student, BookInfo book)
    {
        //borrowedbook info - > BookId , LibCard => Update return date and Fine
        //information table -> Available Quantity +1
        //borrwer info -> Number of book issued -1
        try
        {
            if (db.IsConnected())
            {
                // Check if the book is issued to the student
                string query = "Select * from borrowed_books where Book_Id=@bid AND Issued_to_Library_id=@lnum AND Return_date is NULL";
                MySqlCommand command = new MySqlCommand(query, db.Connection);
                command.Parameters.AddWithValue("bid", book.BookId);
                command.Parameters.AddWithValue("lnum", student.LibraryCardNum);
                var reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    return 0; //No such book is issued to you
                }
                reader.Close();
                // Update the borrower_info table to decrease the number of books issued
                string query3 = "UPDATE borrower_info SET Number_Of_bookissued = Number_Of_bookissued-1 where library_card_number = @lnum AND Number_Of_bookissued>=1";
                MySqlCommand command3 = new MySqlCommand(query3, db.Connection);
                command3.Parameters.AddWithValue("lnum", student.LibraryCardNum);
                command3.ExecuteNonQuery();

                // Update the information table to increase the available quantity of the book
                string query2 = "UPDATE information SET Available_Quantity = Available_Quantity +1 where Book_id=@bid;";
                MySqlCommand command2 = new MySqlCommand(query2, db.Connection);
                command2.Parameters.AddWithValue("bid", book.BookId);
                command2.ExecuteNonQuery();

                // Update the borrowed_books table to set the return date and calculate the fine and return it
                string query1 = "UPDATE borrowed_books set return_date=curdate(),fine= CASE WHEN TIMESTAMPDIFF(MONTH, issued_date, CURDATE()) > 3 THEN (TIMESTAMPDIFF(MONTH, issued_date, CURDATE()) - 3) *500 ELSE 0 END WHERE Issued_to_Library_id = @id AND book_id = @bookId; select fine from borrowed_books WHERE Issued_to_Library_id = @id AND book_id = @bookId ";
                MySqlCommand command1 = new MySqlCommand(query1, db.Connection);
                command1.Parameters.AddWithValue("id", student.LibraryCardNum);
                command1.Parameters.AddWithValue("bookId", book.BookId);
                var reader1 = command1.ExecuteReader();
                int fine = 0;
                while (reader1.Read())
                {
                    fine = Convert.ToInt32(reader1["fine"]);
                }
                reader1.Close();
                if (fine > 0)
                    return fine;
                return 1; // No fine
            }
            return -1; // Connection error
        }
        catch (Exception e)
        {
            return -1;
        }
    }
    public int DeleteAccount(DBConnection db, BorrowerInfo b)
    {
        try
        {
            if (db.IsConnected())
            {
                string query1 = "Select * from borrowed_books where Issued_to_Library_id=@lnum AND return_date is null;";
                MySqlCommand command1 = new MySqlCommand(query1, db.Connection);
                command1.Parameters.AddWithValue("lnum", b.LibraryCardNum);
                var reader = command1.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    return 2; // User has borrowed books, cannot delete account
                }
                reader.Close();
                string query = "DELETE FROM borrower_info WHERE library_card_number=@lnum AND borrower_id=@bid AND Number_of_bookissued = 0;";
                MySqlCommand command = new MySqlCommand(query, db.Connection);
                command.Parameters.AddWithValue("lnum", b.LibraryCardNum);
                command.Parameters.AddWithValue("bid", b.BorrowerId);
                int result = command.ExecuteNonQuery();
                if (result == 0)
                    return 0;
                return 1;
            }
            return -1;
        }
        catch (Exception e)
        {
            Console.WriteLine("Wrong" + e.Message);
            return -1;
        }
    }
    public List<BorrowedBook> BorrowedBooksDetail(DBConnection db, BorrowerInfo b)
    {
        List<BorrowedBook> borrowedBook = new List<BorrowedBook>();
        if (db.IsConnected())
        {
            string query = "Select * from borrowed_books where Issued_to_Library_id=@bid AND return_date is NULL;";
            MySqlCommand command = new MySqlCommand(query, db.Connection);
            command.Parameters.AddWithValue("bid", b.LibraryCardNum);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                BorrowedBook book = new BorrowedBook();
                book.BookId = reader["Book_Id"].ToString();
                book.IssuedDate = reader["Issued_Date"].ToString();
                borrowedBook.Add(book);
            }
            reader.Close();
        }
        return borrowedBook;
    }
}
