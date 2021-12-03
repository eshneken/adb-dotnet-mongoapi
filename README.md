# adb-dotnet-mongoapi
Demonstration of using MongoDB Compatible APIs to connect a .NET application to an OCI Autonomous JSON Database

## Introduction
Software development shops running .NET applications are used to native interaction with a document-oriented, NoSQL database.  Often times this is accomplished by connecting to CosmosDB in Azure.  From an Oracle perspective, the recommended mapping is to leverage Autonomous JSON Database (ADB-J).  However, the ADB-J APIs for native, NoSQL-style interaction with JSON documents are called SODA (Simple Oracle Document Access) and do not currently support .NET.

As an alternative, it is possible to use ADB-J's support for MongoDB APIs to achieve NoSQL interactions with ADB-J from a .NET application.  In addition, one of the benefits of Oracle's converged database strategy is that it is also possible to seamlessly query JSON and relational data together.  This repository aims to show simple examples of both of these concepts.

## Prerequisites
1. Make sure that you have provisioned an Autonomous JSON Database.  You can get there from the OCI Console by following *Overview ->
Autonomous Database*.  You may accept all defaults *except* you must leverage network access controls by selecting *Secure access from allowed IPs and VCNs only* under the *Network Access* section.  Since you are going to be connecting from your laptop for these demos, you can simulate public access by setting an ACL of *CIDR Block* with a value of "0.0.0.0/0".
1. You can use the default *admin* user to store your JSON and relational data.  Although this user should be pre-configured with all permissions you can verify this by navigating to your ADB's home page, clicking the *Database Actions* button at the top left and selecting *Database Users* under *Administration*.  Edit the *admin* user and verify that *Web Access* is enabled and that under the *Granted Roles* section the *SODA_APP* permission is set.

## Getting Started
There are two aspects to examine.  The first is the interaction with ADB-J through .NET code and the second is the view of this from the database side and interoperation with relational operations.

### .NET Code and Demo
The program is run via a console app that is contained in *Program.cs*.  The demo leverages the concept of a collection of car information.  A car contains information about the make, model, and number of cylinders and is defined by *Car.cs*.  Access to the database is expressed through the repository pattern where the repository code is contained in *CarRepository.cs*.

To configure the demo you will need to copy the *appsettings.template.json* file to *appsettings.json* and update the *ConnectString* and *DBName*.  The *DBName* is simply your database username (i.e. admin) and the *ConnectString* needs to be updated with your username (i.e. admin) substituted in several places along with the password of that user.  Please note that if the password has any special characters they will need to be URI encoded (for example, "pass#word" would be written as "pass%23word").  You can verify the connection string in the OCI Console by navigating to your ADB instance, clicking the *Service Console* button and then selecting the *Development* menu item.  If you've configured your network ACLs correctly you will see the MongoDB API connect string shown and it should correspond to the template provided (minus the embedded password).

You should examine the code in *Program.cs* to understand what the demo will do.  It essentially adds 5 vehicles to the DB as documents, queries them, performs updates, filters them by make, and removes one of them.  This demonstrates a standard CRUD scenario.  

Run the demo by issuing a *dotnet run* command from your shell.  You may need to install the dependencies referenced in the *adb-dotnet-mongoapi.csproj* file using NuGet either via the command line or through a plugin in an IDE like VSCode.

### Verify in the Database
Once you've run the demo and verified the output on the command line you may want to also verify how all of this looks in ADB-J.  You may also want to exercise the ability to perform relational queries against your JSON documents as well as to mix-and-match queries against relational and document structures.

Click the *Database Actions* button from your ADB's landing page in the OCI Console and select the *SQL* tile.  This brings you into the in-browser SQL developer view.

Start by creating some sample relational data
1. Copy the contents of the *sql/create-table-ddl.sql* script into the worksheet and execute.  You now have an empty car 'dealer' table
1. Right click on the 'dealer' table in the Navigator panel on the left side of the screen (you may need to hit the refresh button next to the search bar if the table is not showing) and select *Data Loading -> Upload Data*
1. Drag the *sql/dealer-data.csv* file into selector and verify that 10 rows of data appear in the preview.
1. Click *Next* and verify the mapping of rows to columns looks correct.  Click *Next* again and complete the import by clicking *Finish*

You are now positioned to run a few queries to demonstrate the ability to manipulate JSON data from SQL and to mix JSON and relational data.  Paste the contents of *sql/demo-worksheet.sql* into the SQL worksheet and run each of the queries either individually (by highlighting the queries) or as a script by clicking the "Run Script" button.
1. Query 1 shows the ability to surface data inside the JSON document as relational columns
1. Query 2 shows the contents of the relational Dealer table you created in the previous step
1. Query 3 shows the ability to perform relational operations on a combination of the JSON data and the relational table


## Notes/Issues
* Nothing at this time

## URLs
* https://blogs.oracle.com/database/post/introducing-oracle-database-api-for-mongodb
* https://www.mongodb.com/developer/quickstart/csharp-crud-tutorial/


## Contributing
This project is open source.  Please submit your contributions by forking this repository and submitting a pull request!  Oracle appreciates any contributions that are made by the open source community.

## License
Copyright (c) 2021 Oracle and/or its affiliates.

Licensed under the Universal Permissive License (UPL), Version 1.0.

See [LICENSE](LICENSE) for more details.