# aspnet-core-final-project

 **A project that students must complete at the end of their studies in order to receive a certificate of successful completion of the course** 

**TODO List Application**

**Purpose**

The purpose of the task is to get a practical experience in creating web applications using previous knowledge from C# Basics and ASP.NET  Core courses. This task will give students a practical experience in creating domain models of real applications, developing Unit tests and  implementing a user interface and the presentation layer of web applications using .NET Core and ASP.Core.  

Estimated time to complete: 20 hours. 
Task status: mandatory / manually-checked.
Success criteria: All mandatory options implemented

**Requirements**

 Create a “TODO List” web application that allows users to manage personal activities.
The web application should work correctly for Desk top and mobile clients.
Students may use a Domain model from their previous assignment from Module #10 as a starting point for their design.  However, this application should be more functional and provide users with a real possibility to be used in their daily life.
The functionality of the TODO List application is similar to the Microsofts's To Do application https://www.microsoft.com/ru-ru/microsoft-365/microsoft-to-do-list-app?rtc=1 
You are encouraged to briefly go through the application to figure out what functionality your application should provide to the users. Don’t try to copy the design of the above To Do application, try to create your own design and functionality

The project solution should contain at least projects:

1. a class library project which contains classes that represent a domain model for the TODO List application;
2. a client ASP.NET Core MVC  application that end users can use to interact with the TODO application;
3. a unit test project that provides unit tests for the TODO List class library



Users of TODO List application should be able to:

- create multiple TODO lists  (mandatory option)
 - show all TODO lists at the list view (mandatory option)
 a dasboard with all todo lists may look like:
 ![](images/ToDo-Lists.png) 
- remove/hide a list from the  list view (optional option)
- add TODO entries to the TODO list; (mandatory option)
a sample design of TODO list may look like:
![](images/ToDo-List-Items.png) 
- copy existing todo list (optional)
- user should be able to show/hide completed items in a TODO list (optional option)
- the user can decide whether to hide the  TODO list  from the list view  or remove it completely from the TODO list database; (optional option)
- each TODO item should as a minimun have: a title, a description, a due date,a creation date, assignet to (optional requirements)
- add additonal fields to TODO items based on your design ideas to increase the available functionality; (optional requirement)
- the user should  be able to modify a TODO list or a TODO entry;(mandatory option)
- user can set his TODO items  to completed,in progress, not started status;(mandatory option)
_by default newly created TODO entry is in not started status;_ (optional,  may choose better design)

- all the TODO List application data is stored in the database; (mandatory option)
- student may use EF Core framework or other ORMs to save and read data from the database; (optional)
- add Unit test project and  unit tests to test the domain model and asp.net core mvc ui project; (mandatory option)

**Deployment requirements:**

- commit and push your latest version to gitLab and demonstrate live application from your computer to your mentor (mandatory)
- create a free user account at Microsoft Azure (optional)
- deploy yor project to MS Azure cloud and provide URL to your mentor (optional)

 **You may use this application as your portfolio project, so it is highly recommneded to deply it to Azure cloud **
