# ASP.NET Core Final Project

 **A project that students must complete to receive a certificate of successful completion of the track** 

**TODO List Application**

**Purpose**

The project will allow you to solidify your proficiency in C# and ASP.NET Core. As you design your web application, you will get a practical experience of creating domain models, developing unit tests, and implementing a user interface and the presentation layer using .NET Core and ASP.NET Core. When you complete this project, you will have a working web application showpiece to include in your portfolio.

Estimated time to complete: 40 hours. 


**Requirements**

The goal of the project is to create a To Do List web application that allows users to manage their personal activities by creating to do lists. The web application should work correctly on a desktop computer and a mobile phone. You may use the domain model from the Week #10 assignment as a starting point for your design. However, this application should be more functional.  

You are encouraged to check out [this application](https://www.microsoft.com/ru-ru/microsoft-365/microsoft-to-do-list-app?rtc=1 ) as an example in order to get a general understanding of the desired result. However, please do not copy the design of the Microsoft application. Try to create your own design and functionality. 

**Project  Solution Requirements**

The solution should be multi-project containing at least three VS projects:

1. A class library project which contains classes that represent a domain model for the To Do List application.
2. A client ASP.NET Core MVC application that end users can use to interact with the To Do List application.
3. A unit test project that provides unit tests for the To Do List class library.

The VS solution may look as follows:

![](images/todo-solition.png) 

Users of TODO List application should be able to:


| Use case # | Functionality | Mandatory/Optional | Priority |
| ------ | ------ |------ |------ |
 |1 | View all To Do lists at the application’s list view  |M |1 |
| 2 | Create new To Do lists  |M |1 |
| 3 | Add To Do entries (items) to a To Do list  |M |1 |
| 4 | Enter a title, a description, a due date,a creation date to each To Do item |M |1 |
| 5 | Change To Do items status to: Completed, In Progress, Not Started     |M |1 |
| 6 | Modify a To Do list or a To Do entry   |M |1 |
| 7 | Store all To Do List application data in a database |M |1 |
| 8 | Show/hide completed items in a To Do list    |O |2 |
| 9 | Copy existing To Do list     |O |2 |
| 10 |Remove/hide a list from the application’s list view     |O |2 |
| 11 |Add additonal fields/notes to To Do items |O |2 |
| 12 |Hide a To Do list from the list view or remove it completely from the To Do list database|O |2 |
| 13 | See the To Do items that are due today |O |2 |
| 14 | Create a reminder for a To Do item  |O |2 |
 
A sample design of the UI may look like:

![](images/ToDo-Lists.png) 
 
- add TODO entries
 
![](images/ToDo-List-Items.png) 
 
**Project Architecture  Requirements**

- All the To Do List application data is stored in the database. 
- The EF Core or other ORMs to save and read data from the database can be used.  
- The unit test project and unit tests have to be created for the domain model. (mandatory )

**Deployment requirements:**

- Commit and push your latest working version to GitLab to present the application to your mentor in live mode by sharing your screen. Create a free user account at Microsoft Azure, deploy your project to the MS Azure Cloud, and provide a URL to your mentor (optional). 

 **You may use this application as your portfolio project, so it is highly recommneded to deply it to Azure cloud**
