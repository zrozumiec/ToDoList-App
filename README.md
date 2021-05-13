# aspnet-core-final-project

 **A project that students must complete at the end of their studies in order to receive a certificate of successful completion of the course** 

**TODO List Application**

Purpose
The purpose of the task: to get a practical experience  how to create a web application using previous knowlage that students got during C# Basics  and ASP.NET Core courses.This task will give students a practical experience of how to create domain models of a real applications, how to check its functionality by Unit tests and how to demonstrate it to the customers by providing a user interface applications. 
As a user interface application an ASP.NET Core MVC application will be used.

Estimated time to complete: 20 hours.
Task status: mandatory / manually-checked.


Description
Create a TODO list web  application and a class library with a TODO List domain model.
You may use a Domain model from your previous assignment from Module #10 as a prototype.
The functionality of the TODO list application is similar to the Microsofts's To Do application https://www.microsoft.com/ru-ru/microsoft-365/microsoft-to-do-list-app?rtc=1
You may briefly look through the application to figure out what functionaity your application should provide to the users

The project solution should containg at least projects:

1. a class library project which contains classes that represent a domain model for the TODO List application;
2. a client ASP.NET Core MVC  application that end users can use to work with the TODO application;
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
_Optinal requirements:_
- create a free user account at Microsoft Azure
- deploy yor project to MS Azure cloud and provide URL to your mentor

 **You may use this application deployed at MS Azure as your portfolio project **
