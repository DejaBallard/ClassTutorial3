# ClassTutorial3
This repository contains all completed Stage 2 C# source files needed to start Stage 3 of the tutorial.

By Deja Ballard

Troubleshooting and issues i came across

1) when having multiple project solutions and editing between the two, you need to manually rebuild the non-startup (not highlighted) project. this is because visual studio only manages one project at a time. as a result,  we don't see errors and when running the non-startup, it will tun from last buil.

2) make sure when adding code to the DTO class, replicate to the other projects DTO.

3)because the database is a mdf file, errors can arise with connection issues. refreshing the database connection and the computer itself, will re connect the application (i have no clue why restarting fixes it, but it works? so).


