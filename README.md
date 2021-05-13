# aspnet-core-final-project

 **A project that students must complete at the end of their studies in order to receive a certificate of successful completion of the course** 

TODO List Application

Purpose
The purpose of the task: to get a practical experience  how to create a web application using previous knowlwndge that students get during C# basic Course and ASP.NET Core courses, how to create a reusable library of classes that represent a domain model for a real application, the TODO List application in this case. This task will give a practical experience of how to create domain models of a real applications, how to check its functionality by Unit tests and how to demonstrate it to the customers by providing a user interface applications. As a user interface application a simple console application will be used.
Estimated time to complete: 20 hours.
Task status: mandatory / manually-checked.

This task requires you to create Unit tests for your solution. Please read about unit testing techniques before creating unit tests:


Testing principles https://sttp.site/chapters/getting-started/testing-principles.html

Boundary testing https://sttp.site/chapters/testing-techniques/boundary-testing.html



Description
Create a TODO list web  application and a class library with a TODO List domain model.
You may use a Domain model from your previous assignment from Module #10 as a prototype.

Student needs to create a solution with 3 projects:

1. a class library project which contains classes that represent a domain model for a TODO List application;
2. a client ASP.NET Core MVC  application that demonstrates the usage of the class library;
3. a unit test project that provides unit tests for the TODO List class library



User of TODO List application should be able to:

- create multiple TODO lists;
- show all TODO lists at the list view UI
- remove/hide a list from the  list view;
- add TODO entries to the TODO list;
- user should be able to show/hide completed TODO items in a list
- the user can decide whether to hide the  TODO list  from the list view  or remove it completely from the TODO list database;
- each TODO item should as a minimun have: a title, a description, a due date,a creation date, assignet to
- add additonal fields to TODO items based on your design ideas to increase the available functionality;
- the user should  be able to modify a TODO list or a TODO entry;
- user can set his TODO items  to completed,in progress, not started status;
_by default newly created TODO entry is in not started status;_

- all the TODO List application data is stored in the database;
- student may use EF Core framework or other ORMs to save and read data from the database;
- add Unit test project and  unit tests to test the domain model and asp.net core mvc ui project;
 
