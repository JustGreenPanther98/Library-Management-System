using MySql.Data.MySqlClient;
using System;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;

public class BookInfo
{
    public string BookId { get; set; }
    public string BookName { get; set; }
    public string AuthorName { get; set; }
    public string Description { get; set; }
    public decimal BookPrice { get; set; }
    public int AvailableQuantity { get; set; }
}
public class BorrowerInfo
{
    public int LibraryCardNum { get; set; }
    public int BorrowerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public int BorrowedBooksCount { get; set; }
}

public class BorrowedBook
{
    public string BookId { get; set; }
    public int IssuedToLibraryCardNum { get; set; }
    public String IssuedDate { get; set; }
    public String ReturnDate { get; set; }
    public int Fine { get; set; }
}