# aspnet-core-final-project

 **A project that students must complete at the end of their studies in order to receive a certificate of successful completion of the course** 

**TODO List Application**

**Purpose**

The purpose of the task is to get a practical experience in creating web applications using previous knowledge from C# Basics and ASP.NET  Core courses. This task will give students a practical experience in creating domain models of real applications, developing Unit tests and  implementing a user interface and the presentation layer of web applications using .NET Core and ASP.Core.  

Estimated time to complete: 20 hours. 
Task status: mandatory / manually-checked

**Success criteria**: All mandatory options implemented

**Requirements**

 Create a “TODO List” web application that allows users to manage personal activities.
The web application should work correctly for Desk top and mobile clients.
Students may use a Domain model from their previous assignment from Module #10 as a starting point for their design.  However, this application should be more functional and provide users with a real possibility to be used in their daily life.
The functionality of the TODO List application might be similar to the Microsofts's To Do application https://www.microsoft.com/ru-ru/microsoft-365/microsoft-to-do-list-app?rtc=1 
You are encouraged to briefly look through the application to figure out what  your application could provide to the users. Don’t try to copy the design of the above To Do application, try to create your own design and functionality

The task solution should be multiproject solution containing at least 3 VS projects:

1. a class library project which contains classes that represent a domain model for the TODO List application;
2. a client ASP.NET Core MVC  application that end users can use to interact with the TODO application;
3. a unit test project that provides unit tests for the TODO List class library

You solution may look like:

![](images/todo-solition.png) 

Users of TODO List application should be able to:



| Use case # | Functionality | Mandatory/Optional | Priority |
| ------ | ------ |------ |------ |
 |1 | View all TODO lists at the application’s list view  |M |1 |
| 2 | Create new TODO lists  |M |1 |
| 3 | Add TODO entries (items) to a TODO list  |M |1 |
| 4 | Enter a title, a description, a due date,a creation date to each TODO item |M |1 |
| 5 | Change TODO items status to: Completed, In Progress, Not Started     |M |1 |
| 6 | Modify a TODO list or a TODO entry   |M |1 |
| 7 | Store all TODO List application data in database |M |1 |
| 8 | Show/hide completed items in a TODO list    |O |2 |
| 9 | Copy existing todo list     |O |2 |
| 10 |Remove/hide a list from the  application’s list view     |O |2 |
| 11 |Add additonal fields/notes to TODO items |O |2 |
| 12 |Hide a TODO list from the list view or remove it completely from the TODO list database|O |2 |
| 13 | See the TODO items that are due today |O |2 |
| 14 | Create a reminder for the TODO item  |O |2 |
 
A sample design of the UI may look like:

![](images/ToDo-Lists.png) 
 
- add TODO entries
 
![](images/ToDo-List-Items.png) 
 

- all the TODO List application data is stored in the database; (mandatory )
- student may use EF Core framework or other ORMs to save and read data from the database; (optional)
- add Unit test project and  unit tests to test the domain model and asp.net core mvc ui project; (mandatory )

**Deployment requirements:**

- commit and push your latest working version to the gitLab and demonstrate live application from your computer to your mentor (mandatory)
- create a free user account at Microsoft Azure (optional)
- deploy yor project to MS Azure cloud and provide URL to your mentor (optional)

 **You may use this application as your portfolio project, so it is highly recommneded to deply it to Azure cloud **
