
Initial Setup

This is a project powered by .NET 7, to run correctly the App you need to do the following steps:

You need to check if you have .NET 7 on your machine. To do that you can execute dotnet help. Install if you don´t have it.
Open a git bash (or terminal) into a custom folder and clone the repository inside them.
after that, go inside the project folder called Practica 2 and run dotnet restore.
Finally, you can start the project executing the command dotnet run inside the practica 2 folder.
The api will launch in port:7230


to load the data seeds into the project you must do it with the postman app, you must copy all the data from the data seed in the model folder and use the post petition with the following directions: 
"https://localhost:7230/api/Reservation/load-users" for user´s seed
"https://localhost:7230/api/Reservation/load-books" for book´s seed
"https://localhost:7230/api/Reservation/load-reserves" for user´s seed

this petition use a list input, so dont forget the [] at the start and the end


