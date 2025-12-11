using System;
using MySql.Data.MySqlClient;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
public class OperationsOnBooks
{
    //For employees
    private String _password = "@LibPassword.og.org.Lib.987\"";
    public int AddBook(DBConnection db, BookInfo b)
    {
        try
        {
            string query = "INSERT INTO information value(@BookId,@BookName,@AuthorName,@Description,@BookPrice,@AvailableQuantity)";
            if (db.IsConnected())
            {
                MySqlCommand Command = new MySqlCommand(query, db.Connection);
                Command.Parameters.AddWithValue("@BookId", b.BookId);
                Command.Parameters.AddWithValue("@BookName", b.BookName);
                Command.Parameters.AddWithValue("@AuthorName", b.AuthorName);
                Command.Parameters.AddWithValue("@Description", b.Description);
                Command.Parameters.AddWithValue("@BookPrice", b.BookPrice);
                Command.Parameters.AddWithValue("@AvailableQuantity", b.AvailableQuantity);
                Command.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            return -1;//Something went wrong
        }
        return 1;//Book added successfully
    }
    public int UpdateBookAvailableQuantity(DBConnection db, BookInfo bookinfo)
    {
        try
        {
            if (db.IsConnected())
            {
                string query = "UPDATE information set Available_Quantity = @Quantity WHERE Book_Id=@BookId;";
                MySqlCommand command = new MySqlCommand(query, db.Connection);
                command.Parameters.AddWithValue("@BookId", bookinfo.BookId);
                command.Parameters.AddWithValue("@Quantity", bookinfo.AvailableQuantity);
                var result = command.ExecuteNonQuery();
                if (result == 0)
                    return 0;// no rows updated
                return 1;//sucessfully updated
            }
        }
        catch (Exception e)
        {
            return -1;//something went wrong
        }
        return -1;//something went wrong
    }
    public int UpdateBookInfo(DBConnection db, BookInfo Book, String Bookid)//Bookid is the old book id, Book.BookId is the new book id
    {
        try
        {
            if (db.IsConnected())
            {
                string query = "UPDATE information set Book_Id=@BookId,Book_Name=@Name,Author_Name=@AName,Description_Of_Book=@Description,Book_price_in_rupees=@BookPrice,Available_Quantity=@Quantity WHERE Book_Id=@BookID_;";
                MySqlCommand command = new MySqlCommand(query, db.Connection);
                command.Parameters.AddWithValue("BookId", Book.BookId);
                command.Parameters.AddWithValue("BookID_", Bookid);
                command.Parameters.AddWithValue("Name", Book.BookName);
                command.Parameters.AddWithValue("AName", Book.AuthorName);
                command.Parameters.AddWithValue("Description", Book.Description);
                command.Parameters.AddWithValue("BookPrice", Book.BookPrice);
                command.Parameters.AddWithValue("Quantity", Book.AvailableQuantity);
                command.ExecuteNonQuery();
                return 1;
            }
            return 0;//not updated
        }
        catch (Exception e)
        {
            return -1;//eroor
        }
    }
    public int RemoveBookFromLibrary(DBConnection db, String BookId)
    {
        try
        {
            string query = "DELETE FROM information WHERE Book_Id=@BookId;";
            if (db.IsConnected())
            {
                MySqlCommand command = new MySqlCommand(query, db.Connection);
                command.Parameters.AddWithValue("BookId", BookId);
                var result = command.ExecuteNonQuery();
                if (result == 0)
                    return 0;
                return 1;
            }
            return -1;//connection error
        }
        catch (Exception e)
        {
            return -1;
        }
    }
    public List<BookInfo> BookInformation(DBConnection db)
    {
        List<BookInfo> books = new List<BookInfo>();
        if (db.IsConnected())
        {
            String query = "SELECT * FROM information ORDER BY Book_Name; ";
            MySqlCommand command = new MySqlCommand(query, db.Connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                BookInfo book = new BookInfo();
                book.BookId = reader["Book_Id"].ToString();
                book.BookName = reader["Book_Name"].ToString();
                book.AuthorName = reader["Author_Name"].ToString();
                book.Description = reader["Description_Of_Book"].ToString();
                book.BookPrice = Convert.ToDecimal(reader["Book_price_in_rupees"]);
                book.AvailableQuantity = Convert.ToInt32(reader["Available_Quantity"]);
                books.Add(book);
            }
            reader.Close();
        }
        return books;
    }

    public List<BorrowerInfo> DisplayRegisteredStudents(DBConnection db)
    {
        List<BorrowerInfo> Infos = new List<BorrowerInfo>();
        if (db.IsConnected())
        {
            string query = "select * from borrower_info order by library_card_number";
            MySqlCommand command = new MySqlCommand(query, db.Connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                BorrowerInfo info = new BorrowerInfo();
                info.LibraryCardNum = Convert.ToInt32(reader["library_card_number"]);
                info.BorrowerId = Convert.ToInt32(reader["Borrower_id"]);
                info.FirstName = reader["Borrower_First_Name"].ToString();
                info.LastName = reader["Borrower_Last_Name"].ToString();
                info.PhoneNumber = reader["Phone_Number"].ToString();
                info.Address = reader["address"].ToString();
                info.Email = reader["email"].ToString();
                info.BorrowedBooksCount = Convert.ToInt32(reader["Number_of_bookissued"]);
                Infos.Add(info);
            }
            reader.Close();
        }
        return Infos;
    }
    public List<BorrowedBook> DisplayDetailsOfBorrowedBooks(DBConnection db)
    {
        List<BorrowedBook> borrowedBooksInfo = new List<BorrowedBook>();
        if (db.IsConnected())
        {
            string query = "Select * from borrowed_books";
            MySqlCommand command = new MySqlCommand(query, db.Connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                BorrowedBook info = new BorrowedBook();
                info.BookId = reader["Book_id"].ToString();
                info.IssuedToLibraryCardNum = Convert.ToInt32(reader["Issued_to_Library_id"]);
                info.IssuedDate = reader["issued_date"].ToString();
                info.ReturnDate = reader["return_date"].ToString();
                info.Fine = Convert.ToInt32(reader["fine"]);
                borrowedBooksInfo.Add(info);
            }
            reader.Close();
        }
        return borrowedBooksInfo;
    }

    public List<BorrowedBook> DisplayDetailsOfBorrowedBooksWithPendingReturn(DBConnection db)
    {
        List<BorrowedBook> borrowedBooksInfo = new List<BorrowedBook>();
        if (db.IsConnected())
        {
            string query = "Select * from borrowed_books where return_date IS NULL";
            MySqlCommand command = new MySqlCommand(query, db.Connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                BorrowedBook info = new BorrowedBook();
                info.BookId = reader["Book_id"].ToString();
                info.IssuedToLibraryCardNum = Convert.ToInt32(reader["Issued_to_Library_id"]);
                info.IssuedDate = reader["issued_date"].ToString();
                borrowedBooksInfo.Add(info);
            }
            reader.Close();
        }
        return borrowedBooksInfo;
    }
    public bool IsPasswordValid(String pass)
    {
        return this._password.Equals(pass);
    }
}
