 ClassTutorial3
This repository contains all completed Stage 2 C# source files needed to start Stage 3 of the tutorial.

<b>Codeed by:</b>Deja Ballard


<b>Purpose:</b>Tutorial 3 expands from tutorial 2 by implementing a database to save and populate the application instead of file serilization. For this tutorial, We will be making two seperate project solutions:

One to host the database as a server, which is hosted locally on port 60064 and uses an API controller to manage the SQL statements.

The other solution is the windows form application, this is the front end and user's interface for this tutorial and it uses HTTP's Representational State Transfer(REST) protocols to connection to the database server by talking to the API.


<b>Troubleshooting and issues I came across:</b>

1) when having multiple project solutions and editing between the two, you need to manually rebuild the non-startup (not highlighted) project. this is because visual studio only manages one project at a time. as a result,  we don't see errors and when running the non-startup, it will tun from last buil.

2) make sure when adding code to the DTO class, replicate to the other projects DTO.

3) Because the database is a mdf file, errors can arise with connection issues. refreshing the database connection and the computer itself, will re connect the application (I have no clue why restarting fixes it, but it works? so).


<B>Branch Control:</B>

<i>Master:</i> The starting point, supplied by the teacher, which was forked

<i>Edited_Verion_1:</i> Only has the server solution and gets the list of names in the database. Can be viewed within a web browser.

<i>Edited_Verion_2:</i> Two solutions, Windows form displays the artist list and will open up a window for individual artists when clicked. Only able to edit and add artists.

<i>Edited_Verion_3:</i> Fully functional tutorial. The windows form is now able to edit and add artworks aswell as deletion of artists and artworks.

