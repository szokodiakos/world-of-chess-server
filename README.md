# world-of-chess-server
.NET Chessgame server-side

A multiplayer chess game with chess engine implemented in .NET.

Chessgine - The chess engine project containing the chess game's logic

Server - A WPF project (with nice and simple UI) implementing the server.
The clients can connect to the server using the SignalR library.
The server itself can connect to a MySQL database where it stores high scores and players' ratings.

ChessgineTest - Demo project to try the Chessgine API

ChessgineUnitTest - Unit test project for the Chessgine (and also examples for the API) with 90%+ code coverage
