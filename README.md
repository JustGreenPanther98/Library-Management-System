<h1>📚 Library Management System (C# + ADO.NET + MySQL)</h1>

<h2>📌 Project Overview</h2>
<p>
This project is a <strong>console-based Library Management System</strong> developed using
<strong>C#</strong>, <strong>ADO.NET</strong>, and <strong>MySQL</strong>.
It allows students (borrowers) and library employees to manage books, borrowing,
returns, and user accounts through a menu-driven CLI interface.
</p>

<p>
The project focuses on <strong>database connectivity, SQL-based data operations,
and real-world workflow logic</strong>, rather than advanced architectural or design patterns.
</p>

<hr/>

<h2>⚙️ Core Functionalities</h2>

<h3>👤 Student (Borrower) Operations</h3>
<ul>
  <li>Create a library account</li>
  <li>Login using <strong>Library Card Number</strong> and <strong>Borrower ID</strong></li>
  <li>View available books</li>
  <li>Borrow books with availability checks</li>
  <li>Return borrowed books</li>
  <li>Automatic fine calculation on late returns</li>
  <li>View borrowed book details</li>
  <li>Delete borrower account (only if no active borrowed books exist)</li>
</ul>

<h3>🧑‍💼 Employee (Admin) Operations</h3>
<ul>
  <li>Secure employee access using a password</li>
  <li>Add new books to the library</li>
  <li>Update book details (price, quantity, description)</li>
  <li>Delete books from inventory</li>
  <li>View all registered borrowers</li>
  <li>View all borrowed books and pending returns</li>
</ul>

<hr/>

<h2>🗄️ Database &amp; Persistence</h2>
<ul>
  <li><strong>Database:</strong> MySQL</li>
  <li><strong>Database Access:</strong> ADO.NET (MySql.Data)</li>
  <li>SQL-based CRUD operations</li>
  <li>Application-level checks for book availability and borrow limits</li>
  <li>Library Card Number generated using <code>AUTO_INCREMENT</code></li>
</ul>

<hr/>

<h2>🧠 Code Structure (Based on Actual Implementation)</h2>

<h3>📁 Program.cs</h3>
<ul>
  <li>Acts as the main controller</li>
  <li>Handles user interaction and menu flow</li>
  <li>Invokes student and employee operation classes</li>
</ul>

<h3>📁 Data Models</h3>
<ul>
  <li><strong>BookInfo</strong> – Represents book details</li>
  <li><strong>BorrowerInfo</strong> – Represents borrower/student information</li>
  <li><strong>BorrowedBook</strong> – Represents issued book records</li>
</ul>

<p>
These classes act as <strong>plain data holders</strong> (DTO-like).
</p>

<h3>📁 Operation Classes</h3>
<ul>
  <li>Separate classes for student and employee operations</li>
  <li>Contain SQL queries, business rules, and database interaction logic</li>
</ul>

<h3>📁 Database Connection</h3>
<ul>
  <li>Uses a <strong>Singleton-style</strong> database connection class</li>
  <li>Centralized MySQL connection management</li>
</ul>

<hr/>

<h2>⚠️ Architecture Notes</h2>
<ul>
  <li>No formal design patterns (MVC, Repository, layered architecture)</li>
  <li>Business logic and database access are tightly coupled</li>
  <li>SQL queries are written directly inside operation methods</li>
  <li><code>Program.cs</code> acts as a God class / orchestrator</li>
  <li>Error handling is minimal and flow-driven</li>
</ul>

<p>
This structure reflects an <strong>early learning-stage project</strong>, focused on
functional correctness and end-to-end execution.
</p>

<hr/>

<h2>🎯 Learning Outcomes</h2>
<ul>
  <li>Using ADO.NET to connect C# applications with MySQL</li>
  <li>Executing parameterized SQL queries</li>
  <li>Managing relational data using SQL</li>
  <li>Implementing real-world workflows (borrow / return / fine calculation)</li>
  <li>Understanding the importance of clean architecture through experience</li>
</ul>

<hr/>

<h2>🧠 Note to Reviewers</h2>
<p>
This repository is preserved <strong>as a learning milestone</strong>.
More recent projects demonstrate cleaner architecture, better separation of concerns,
and modern backend practices.
</p>
<hr/>

