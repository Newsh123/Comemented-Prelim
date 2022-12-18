//Skeleton Program code for the AQA A Level Paper 1 Summer 2023 examination
//this code should be used in conjunction with the Preliminary Material
//written by the AQA Programmer Team
//developed in the Visual Studio Community Edition programming environment

using System;
using System.Collections.Generic;

namespace Dastan
{
    class Program
    {
        static void Main(string[] args)
        {
            Dastan ThisGame = new Dastan(6, 6, 4);                                                      // initialises the dastan class and sets it's rows & columns to 6 and number of piece to 4 - the board is created along with the pieces
            ThisGame.PlayGame();                                                                        // causes the game to start after it has been created
            Console.WriteLine("Goodbye!");                                                              // prints goodbye once PlayGame() ends which would mean the game ends
            Console.ReadLine();                                                                         // waits for user input before ending the program
        }
    }

    class Dastan
    {
        protected List<Square> Board;                                                                   // board is one-dimensianal list of the square class
        protected int NoOfRows, NoOfColumns, MoveOptionOfferPosition;                                   // number of rows, number of columns & offer position are defined as integers
        protected List<Player> Players = new List<Player>();                                            // adds the list of players (most likely contains 2 players) 
        protected List<string> MoveOptionOffer = new List<string>();                                    // the list of possible moves that can be played
        protected Player CurrentPlayer;                                                                 // the current player who needs to make a move
        protected Random RGen = new Random();                                                           // random number generator to be used in the program

        public Dastan(int R, int C, int NoOfPieces)
        {
            Players.Add(new Player("Player One", 1));                                                   // initialises player one - sets score to 100, sets name to "Player One" and direction to downwards (+1)
            Players.Add(new Player("Player Two", -1));                                                  // initialises player two - sets score to 100, sets name to "Player Two" and direction to upwards (-1)
            CreateMoveOptions();                                                                        // creates the move options for both players with their respective directions
            NoOfRows = R;                                                                               // sets the number of rows equal to what has been inputted
            NoOfColumns = C;                                                                            // sets the number of columns equal to what has been inputted
            MoveOptionOfferPosition = 0;                                                                // sets the offer position to 0
            CreateMoveOptionOffer();                                                                    // adds all of the moves to the move option offer list
            CreateBoard();                                                                              // creates the board of specified rows and columns (kotlas are created at the same time)
            CreatePieces(NoOfPieces);                                                                   // adds the pieces and mirza to the board 
            CurrentPlayer = Players[0];                                                                 // sets the current player as player one
        }

        private void DisplayBoard()
        {
            Console.Write(Environment.NewLine + "   ");                                                 // skips a line and inrements by 3 spaces
            for (int Column = 1; Column <= NoOfColumns; Column++)                                       // loops through the number of columns declared when Dastan was initialised 
            {
                Console.Write(Column.ToString() + "  ");                                                // writes the column number with 2 spaces seperating it from the next one
            }
            Console.Write(Environment.NewLine + "  ");                                                  // goes onto the next line and increments by 2 spaces
            for (int Count = 1; Count <= NoOfColumns; Count++)                                          // loops through the number of columns declared when Dastan was initialised 
            {
                Console.Write("---");                                                                   // writing the top line of the top cells
            }
            Console.WriteLine("-");                                                                     // adding an extra dash at the end so that it reaches the full length of the board and starting a new line (Console.WriteLine over Console.Write)
            for (int Row = 1; Row <= NoOfRows; Row++)                                                   // loops through the number of rows declared when Dastan was initialised 
            {
                Console.Write(Row.ToString() + " ");                                                    // writes the row number followed by a space to seperate it from the rest of the board
                for (int Column = 1; Column <= NoOfColumns; Column++)                                   // loops through the number of columns declared when Dastan was initialised
                {
                    int Index = GetIndexOfSquare(Row * 10 + Column);                                    // getting the index for the next square to be added to the board using GetIndexOfSquare function
                    Console.Write("|" + Board[Index].GetSymbol());                                      // writing | (represents the start of the cell) followed by the symbol in that boards square (either blank or a kotla)
                    Piece PieceInSquare = Board[Index].GetPieceInSquare();                              // declaring a tempoary piece to represent theh piece in the current square
                    if (PieceInSquare == null)                                          
                    {
                        Console.Write(" ");                                                             // if there is no piece in the square then a blank space is written
                    }
                    else
                    {
                        Console.Write(PieceInSquare.GetSymbol());                                       // if there is a piece in the square then the piece's symbol is written
                    }
                }
                Console.WriteLine("|");                                                                 // adding a final | at the end to complete the line so it reaches the full length of the board and starts a new line (Console.WriteLine over Console.Write)
            }
            Console.Write("  -");                                                                       // start of the bottom of the square (increments by 2 before starting the bottom)               
            for (int Column = 1; Column <= NoOfColumns; Column++)                                       // loops through the number of columns declared when Dastan was initialised
            {
                Console.Write("---");                                                                   // writing the bottom lines of each square
            }
            Console.WriteLine();                                                                        // skipping a line
            Console.WriteLine();                                                                        // skipping a line
        }

        private void DisplayState()
        {
            DisplayBoard();                                                                             // calls displayBoard
            Console.WriteLine("Move option offer: " + MoveOptionOffer[MoveOptionOfferPosition]);        // displaying the first player's move offer
            Console.WriteLine();                                                                        // skipping a line
            Console.WriteLine(CurrentPlayer.GetPlayerStateAsString());                                  // displays the players play string (players name, their score and their move option queue)
            Console.WriteLine("Turn: " + CurrentPlayer.GetName());                                      // states the players who turn it is 
            Console.WriteLine();                                                                        // skips a line 
        }

        private int GetIndexOfSquare(int SquareReference)
        {
            int Row = SquareReference / 10;                                                             // row is the value in the tens column of SquareReference
            int Col = SquareReference % 10;                                                             // column is the value in the units column of SquareReference
            return (Row - 1) * NoOfColumns + (Col - 1);                                                 // returns the index of the square in the list (example: 42 is converted to 19)
        }

        private bool CheckSquareInBounds(int SquareReference)
        {
            int Row = SquareReference / 10;                                                             // sets the row number as the tens digit from the square reference
            int Col = SquareReference % 10;                                                             // sets the column number as the units digit from the square reference
            if (Row < 1 || Row > NoOfRows)
            {
                return false;                                                                           // if the row number is below 1 or is above the declared number of rows the function returns false
            }
            else if (Col < 1 || Col > NoOfColumns)
            {
                return false;                                                                           // if the column number is below 1 or is above the declared number of columns the function returns false
            }
            else
            {
                return true;                                                                            // if neither of the above is true then the function returns true
            }
        }

        private bool CheckSquareIsValid(int SquareReference, bool StartSquare)
        {
            if (!CheckSquareInBounds(SquareReference))                                                  
            {
                return false;                                                                           // if the square is not within the range of the list then the function returns false
            }
            Piece PieceInSquare = Board[GetIndexOfSquare(SquareReference)].GetPieceInSquare();          // declaring a tempoary piece for the piece in the given square reference
            if (PieceInSquare == null)
            {
                if (StartSquare)
                {
                    return false;                                                                       // if there is no piece in the square and it is checking that it is a valid start square then the function returns false
                }
                else
                {
                    return true;                                                                        // if there is no piece in the square but it is not checking that it is a valid start square then the function returns true
                }
            }
            else if (CurrentPlayer.SameAs(PieceInSquare.GetBelongsTo()))
            {
                if (StartSquare)
                {
                    return true;                                                                        // if the piece in the square belongs to the player and it is checking that it is a valid start square then the function returns true 
                }
                else
                {
                    return false;                                                                       // if the piece in the square belongs to the player and it is not checking that it is a valid start square then the function returns false
                }
            }
            else
            {
                if (StartSquare)
                {
                    return false;                                                                       // if the piece in the square does not belong to the player and it is checking that it is a valid start sqaure then the function returns false
                }
                else
                {
                    return true;                                                                        // if the piece in the square does not belong to the palyer and it is not checking that it is a valid start square then the function returns true
                }
            }
        }

        private bool CheckIfGameOver()
        {
            bool Player1HasMirza = false;                                                               // creating flag for player one having a mirza to false by default
            bool Player2HasMirza = false;                                                               // creating flag for player two having a mirza to false by default
            foreach (var S in Board)                                                                    // looping through each square in the board
            {
                Piece PieceInSquare = S.GetPieceInSquare();                                             // tempoary variable for the piece in the current square
                if (PieceInSquare != null)
                {
                    if (S.ContainsKotla() && PieceInSquare.GetTypeOfPiece() == "mirza" && !PieceInSquare.GetBelongsTo().SameAs(S.GetBelongsTo()))
                    {
                        return true;                                                                    // retuns true if the square contains a kotla, contains a mirza and the mirza does not belong to the same player as the kotla
                    }
                    else if (PieceInSquare.GetTypeOfPiece() == "mirza" && PieceInSquare.GetBelongsTo().SameAs(Players[0]))
                    {
                        Player1HasMirza = true;                                                         // sets the player one having mirza flag to true
                    }
                    else if (PieceInSquare.GetTypeOfPiece() == "mirza" && PieceInSquare.GetBelongsTo().SameAs(Players[1]))
                    {
                        Player2HasMirza = true;                                                         // sets the player two having mirza flag to true
                    }
                }
            }
            return !(Player1HasMirza && Player2HasMirza);                                               // returns false if both players have mirza, true if one player does have mirza
        }

        private int GetSquareReference(string Description)
        {
            int SelectedSquare;                                                                         // declaring the selected square variable as an integer
            Console.Write("Enter the square " + Description + " (row number followed by column number): ");     // ouputting to the suer the type of square to enter (such as the square they want to move the piece to)
            SelectedSquare = Convert.ToInt32(Console.ReadLine());                                       // reading the reference for the square inputted by the user (no input checking so could crash the program)
            return SelectedSquare;                                                                      // returns the selected square
        }

        private void UseMoveOptionOffer()
        {
            int ReplaceChoice;                                                                          // declaring the ReplaceChoice as an integer
            Console.Write("Choose the move option from your queue to replace (1 to 5): ");              // giving the user the options for what move they want to replace
            ReplaceChoice = Convert.ToInt32(Console.ReadLine());                                        // reading in the users choice and converting result to an integer (no error checking so could crash program)
            CurrentPlayer.UpdateMoveOptionQueueWithOffer(ReplaceChoice - 1, CreateMoveOption(MoveOptionOffer[MoveOptionOfferPosition], CurrentPlayer.GetDirection()));      // updates the players queue to swap the move option offer into the queue
            CurrentPlayer.ChangeScore(-(10 - (ReplaceChoice * 2)));                                     // decreases the players score according to where the replaced move option was (e.g. if move option was at position 3 the score would decrease by 4)
            MoveOptionOfferPosition = RGen.Next(0, 5);                                                  // chooses a random move option between 0 and 4 inclusive to be the next offer
        }

        private int GetPointsForOccupancyByPlayer(Player CurrentPlayer)
        {
            int ScoreAdjustment = 0;                                                                    // setting the score adjustment to 0 by default
            foreach (var S in Board)                                                                    // looping through each square in the board
            {
                ScoreAdjustment += (S.GetPointsForOccupancy(CurrentPlayer));                            // adding the points gained from the current playing occupying each square (+5 if the square is his kotla and contains his own piece, +1 if the square is the other players kotla and contains the current players piece, +0 if neither of the other 2 conditions are met) 
            }
            return ScoreAdjustment;                                                                     // returning the score adjustment required for the player
        }

        private void UpdatePlayerScore(int PointsForPieceCapture)
        {
            CurrentPlayer.ChangeScore(GetPointsForOccupancyByPlayer(CurrentPlayer) + PointsForPieceCapture);        // add the points for the occupying squares and taking a piece to the current players score
        }

        private int CalculatePieceCapturePoints(int FinishSquareReference)
        {
            if (Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare() != null)
            {
                return Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare().GetPointsIfCaptured();     // if there is a piece in the square then return the points gained from capturing the square
            }
            return 0;                                                                                   // if there is not a piece in the square then return 0 points for capturing it
        }

        public void PlayGame()
        {
            bool GameOver = false;                                                                      // setting gameOver to false 
            while (!GameOver)                                                                           // looping while gameOver is set to false, loop is exited once game ends
            {
                DisplayState();                                                                         // displays the game state information for the next players turn 
                bool SquareIsValid = false;                                                             // setting that the square is not valid
                int Choice;                                                                             // declaring the integer Choice 
                do                                                                                      // looping while the user does not choose a number between 1 and 3 (loops at least once)
                {
                    Console.Write("Choose move option to use from queue (1 to 3) or 9 to take the offer: ");    // displaying instructions to the user
                    Choice = Convert.ToInt32(Console.ReadLine());                                       // converting the users choice into an integer (no error checking for if it is already an integer so could cause a crash) 
                    if (Choice == 9)
                    {
                        UseMoveOptionOffer();                                                           // if choice is set to 9 then the move option offer is used
                        DisplayState();                                                                 // redisplays the current game state with the updated information from the move option offer being taken
                    }
                }
                while (Choice < 1 || Choice > 3);
                int StartSquareReference = 0;                                                           // declaring the variable for the square of origin for the piece the user wants to move
                while (!SquareIsValid)                                                                  // looping while the square is not verified
                {
                    StartSquareReference = GetSquareReference("containing the piece to move");          // gets the square reference for the piece that the user wants to move
                    SquareIsValid = CheckSquareIsValid(StartSquareReference, true);                     // checks if the square is a valid place to go from
                }
                int FinishSquareReference = 0;                                                          // declaring the variable for the sqaure the player wants to move to 
                SquareIsValid = false;                                                                  // setting that the square is invalid
                while (!SquareIsValid)                                                                  // looping while the square is not valid
                {
                    FinishSquareReference = GetSquareReference("to move to");                           // gets the square reference for the square that the user wants to move to
                    SquareIsValid = CheckSquareIsValid(FinishSquareReference, false);                   // checks if the square is a valid place to move to
                }
                bool MoveLegal = CurrentPlayer.CheckPlayerMove(Choice, StartSquareReference, FinishSquareReference);    // move is legal if there is a valid move in the move option that takes the piece from the start positon to the finish position
                if (MoveLegal)
                {
                    int PointsForPieceCapture = CalculatePieceCapturePoints(FinishSquareReference);     // if the move is legal the points for capturing a piece at the finish square is retrieved
                    CurrentPlayer.ChangeScore(-(Choice + (2 * (Choice - 1))));                          // changes by the score by the amount based on the choice chosen (choice 3 results in -7 score
                    CurrentPlayer.UpdateQueueAfterMove(Choice);                                         // updating the player's move option queue to move the selected moe option to the back of it
                    UpdateBoard(StartSquareReference, FinishSquareReference);                           // moves the piece to the new place on the board
                    UpdatePlayerScore(PointsForPieceCapture);                                           // adds the points gained that round to the players score (points for occupying squares + points for capture)
                    Console.WriteLine("New score: " + CurrentPlayer.GetScore() + Environment.NewLine);
                }
                if (CurrentPlayer.SameAs(Players[0]))
                {
                    CurrentPlayer = Players[1];                                                         // if the current player is "player one" then the current player is updated to "plaer two"                             
                }
                else
                {
                    CurrentPlayer = Players[0];                                                         // if the current player is not "player one" then the current player is set to "player one"
                }
                GameOver = CheckIfGameOver();                                                           // check for if the game is over yet, loop ends if it is
            }
            DisplayState();                                                                             // displays the current game state information (I HATE THIS IT SHOULDN'T DO THIS! IT TELLS THE NEXT PLAYER TO GO BEFORE ENDING!?!?!??!)
            DisplayFinalResult();                                                                       // displays the result of the game
        }

        private void UpdateBoard(int StartSquareReference, int FinishSquareReference)
        {
            Board[GetIndexOfSquare(FinishSquareReference)].SetPiece(Board[GetIndexOfSquare(StartSquareReference)].RemovePiece());       // moving the piece from the start square to the finish square
        }

        private void DisplayFinalResult()
        {
            if (Players[0].GetScore() == Players[1].GetScore())
            {
                Console.WriteLine("Draw!");                                                             // if the players both have the same score then it ends in a draw
            }
            else if (Players[0].GetScore() > Players[1].GetScore())
            {
                Console.WriteLine(Players[0].GetName() + " is the winner!");                            // if player one has a higher score then player one wins
            }
            else
            {
                Console.WriteLine(Players[1].GetName() + " is the winner!");                            // if player two has a higher score then player two wins
            }
        }

        private void CreateBoard()
        {
            Square S;                                                                                   // declares new tempoary square called S
            Board = new List<Square>();                                                                 // defines Board as a list of squares
            for (int Row = 1; Row <= NoOfRows; Row++)                                                   // loops through number of rows
            {
                for (int Column = 1; Column <= NoOfColumns; Column++)                                   // loops through number of columns 
                {
                    if (Row == 1 && Column == NoOfColumns / 2)
                    {
                        S = new Kotla(Players[0], "K");                                                 // defines the square as a kotla for player one if it is on row one and is in the middle of the board
                    }
                    else if (Row == NoOfRows && Column == NoOfColumns / 2 + 1)
                    {
                        S = new Kotla(Players[1], "k");                                                 // defines the square as a kotla for player two if it is on the last row and is in the middle of the board (opposite side of middle to player one's)
                    }
                    else
                    {
                        S = new Square();                                                               // if square does not contain a kotla it is initialised as a square
                    }
                    Board.Add(S);                                                                       // add square to the board list (one-dimenisonal) 
                }
            }
        }

        private void CreatePieces(int NoOfPieces)
        {
            Piece CurrentPiece;                                                                         // declares new tempoary piece called CurrentPiece
            for (int Count = 1; Count <= NoOfPieces; Count++)                                           // loops through the number of pieces
            {
                CurrentPiece = new Piece("piece", Players[0], 1, "!");                                  // creates a new piece called "piece" for player one and gives one point if captured with a symbol of "!"
                Board[GetIndexOfSquare(2 * 10 + Count + 1)].SetPiece(CurrentPiece);                     // adds the piece to the board at square specified (at count 3 the square would be 24 and that is converted to the index 9)
            }
            CurrentPiece = new Piece("mirza", Players[0], 5, "1");                                      // creates a new piece called "mirza" for player one and gives five points if captured with a symbol of "1"
            Board[GetIndexOfSquare(10 + NoOfColumns / 2)].SetPiece(CurrentPiece);                       // adds the mirza to the board at the same position that the kotla is at
            for (int Count = 1; Count <= NoOfPieces; Count++)                                           // loops through the number of pieces
            {
                CurrentPiece = new Piece("piece", Players[1], 1, "\"");                                 // creates a new piece called "piece" for player two and gives one point if captured with a symbol of "\"
                Board[GetIndexOfSquare((NoOfRows - 1) * 10 + Count + 1)].SetPiece(CurrentPiece);        // adds the piece to the board at square specified (at count 3 the square would be 24 and that is converted to the index 9)
            }
            CurrentPiece = new Piece("mirza", Players[1], 5, "2");                                      // creates a new piece called "mirza" for player two and gives five points if captured with a symbol of "2"
            Board[GetIndexOfSquare(NoOfRows * 10 + (NoOfColumns / 2 + 1))].SetPiece(CurrentPiece);      // adds the mirza to the board at the same position that the kotla is at
        }

        private void CreateMoveOptionOffer()
        {
            MoveOptionOffer.Add("jazair");                                                              // adds "jazair" to the move option offer list
            MoveOptionOffer.Add("chowkidar");                                                           // adds "chowkidar" to the move option offer list
            MoveOptionOffer.Add("cuirassier");                                                          // adds "cuirassier" to the move option offer list
            MoveOptionOffer.Add("ryott");                                                               // adds "ryott" to the move option offer list    
            MoveOptionOffer.Add("faujdar");                                                             // adds "faujdar" to the move option offer list
        }

        private MoveOption CreateRyottMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("ryott");                                         // initialises new move option for ryott 
            Move NewMove = new Move(0, 1 * Direction);                                                  // creates a new move that moves piece horizonally 1 place
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves 
            NewMove = new Move(0, -1 * Direction);                                                      // creates a new move that moves piece horizonally 1 place   
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            NewMove = new Move(1 * Direction, 0);                                                       // creates a new move that moves piece vertically 1 place
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            NewMove = new Move(-1 * Direction, 0);                                                      // creates a new move that moves piece vertically 1 place
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            return NewMoveOption;                                                                       // returns the move options created within this function
        }

        private MoveOption CreateFaujdarMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("faujdar");                                       // initialises new move option for faujdar 
            Move NewMove = new Move(0, -1 * Direction);                                                 // creates a new move that moves piece horizonally 1 place
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves 
            NewMove = new Move(0, 1 * Direction);                                                       // creates a new move that moves piece horizonally 1 place
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves 
            NewMove = new Move(0, 2 * Direction);                                                       // creates a new move that moves piece horizonally 2 place
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves 
            NewMove = new Move(0, -2 * Direction);                                                      // creates a new move that moves piece horizonally 2 place
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves 
            return NewMoveOption;                                                                       // returns the move options created within this function
        }

        private MoveOption CreateJazairMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("jazair");                                        // initialises new move option for jazair
            Move NewMove = new Move(2 * Direction, 0);                                                  // creates a new move that moves piece vertically 2 places (forwards)
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            NewMove = new Move(2 * Direction, -2 * Direction);                                          // creates a new move that moves piece diagonally in / direction 2 places (forwards)
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            NewMove = new Move(2 * Direction, 2 * Direction);                                           // creates a new move that moves piece diagonally in \ direction 2 places (forwards)
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            NewMove = new Move(0, 2 * Direction);                                                       // creates a new move that moves piece horizontally 2 places
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            NewMove = new Move(0, -2 * Direction);                                                      // creates a new move that moves piece horizontally 2 places
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves    
            NewMove = new Move(-1 * Direction, -1 * Direction);                                         // creates a new move that moves piece diagonally in \ direction 1 place (backwards)
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves    
            NewMove = new Move(-1 * Direction, 1 * Direction);                                          // creates a new move that moves piece diagonally in / direction 1 place (backwards)
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            return NewMoveOption;                                                                       // returns the move options created within this function
        }

        private MoveOption CreateCuirassierMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("cuirassier");                                    // initialises new move option for chowkidar 
            Move NewMove = new Move(1 * Direction, 0);                                                  // creates a new move that moves piece vertically 1 place (forwards)
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            NewMove = new Move(2 * Direction, 0);                                                       // creates a new move that moves piece vertically 2 places (forwards)
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            NewMove = new Move(1 * Direction, -2 * Direction);                                          // creates a new move that moves piece in ¬ shape (forwards)
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves    
            NewMove = new Move(1 * Direction, 2 * Direction);                                           // creates a new move that moves piece in ¬ shape (forwards)
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            return NewMoveOption;                                                                       // returns the move options created within this function
        }

        private MoveOption CreateChowkidarMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("chowkidar");                                     // initialises new move option for chowkidar 
            Move NewMove = new Move(1 * Direction, 1 * Direction);                                      // creates a new move that moves piece diagonally in \ direction 1 place
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves 
            NewMove = new Move(1 * Direction, -1 * Direction);                                          // creates a new move that moves piece diagonally in / direction 1 place
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves  
            NewMove = new Move(-1 * Direction, 1 * Direction);                                          // creates a new move that moves piece diagonally in / direction 1 place
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            NewMove = new Move(-1 * Direction, -1 * Direction);                                         // creates a new move that moves piece diagonally in \ direction 1 place
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves        
            NewMove = new Move(0, 2 * Direction);                                                       // creates a new move that moves piece horizontally 2 places
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            NewMove = new Move(0, -2 * Direction);                                                      // creates a new move that moves piece horizontally 2 places
            NewMoveOption.AddToPossibleMoves(NewMove);                                                  // adds the new move to the possible moves
            return NewMoveOption;                                                                       // returns the mvoe options created within this function
        }

        private MoveOption CreateMoveOption(string Name, int Direction)
        {
            if (Name == "chowkidar")
            {
                return CreateChowkidarMoveOption(Direction);                                            // creates the move options that let the piece move either 1 place diagonally or 2 places horizontally if the name requried is "chowdikar"
            }
            else if (Name == "ryott")
            {
                return CreateRyottMoveOption(Direction);                                                // creates the move options that let the piece move either 1 place vertically or 1 place horizontally if the name requried is "ryott"
            }
            else if (Name == "faujdar")
            {
                return CreateFaujdarMoveOption(Direction);                                              // creates the move options that let the piece move either 1 or 2 places horizontally if the name requried is "faujdar"
            }
            else if (Name == "jazair")
            {
                return CreateJazairMoveOption(Direction);                                               // creates the move options that let the piece move in jazair move patter (see reference) if the name requried is "jazair"
            }
            else
            {
                return CreateCuirassierMoveOption(Direction);                                           // creates the move options that let the piece move either 1 place vertically or 1 place horizontally if the name requried is "cuirassier"
            }
        }

        private void CreateMoveOptions()
        {
            Players[0].AddToMoveOptionQueue(CreateMoveOption("ryott", 1));                              // adds ryott move where forwards direction is downwards
            Players[0].AddToMoveOptionQueue(CreateMoveOption("chowkidar", 1));                          // adds chowkidar move where forwards direction is downwards    
            Players[0].AddToMoveOptionQueue(CreateMoveOption("cuirassier", 1));                         // adds curiasser move where forwards direction is downwards
            Players[0].AddToMoveOptionQueue(CreateMoveOption("faujdar", 1));                            // adds faujdar move where forwards direction is downwards
            Players[0].AddToMoveOptionQueue(CreateMoveOption("jazair", 1));                             // adds jazair move where forwards direction is downwards
            Players[1].AddToMoveOptionQueue(CreateMoveOption("ryott", -1));                             // adds ryott move where backwards direction is downwards
            Players[1].AddToMoveOptionQueue(CreateMoveOption("chowkidar", -1));                         // adds chowdikar move where backwards direction is downwards
            Players[1].AddToMoveOptionQueue(CreateMoveOption("jazair", -1));                            // adds jazair move where backwards direction is downwards
            Players[1].AddToMoveOptionQueue(CreateMoveOption("faujdar", -1));                           // adds faujdar move where backwards direction is downwards    
            Players[1].AddToMoveOptionQueue(CreateMoveOption("cuirassier", -1));                        // adds curissier move where backwards direction is downwards
        }
    }

    class Piece
    {
        protected string TypeOfPiece, Symbol;                                                           // type of piece and symbol for the piece is defined as a string (type of piece is either piece or mirza) 
        protected int PointsIfCaptured;                                                                 // points if the piece is captured is held as an integer
        protected Player BelongsTo;                                                                     // the player who the piece belongs to

        public Piece(string T, Player B, int P, string S)                                               // initialiser sets variables as what is inputted 
        {
            TypeOfPiece = T;
            BelongsTo = B;
            PointsIfCaptured = P;
            Symbol = S;
        }

        public string GetSymbol()
        {
            return Symbol;                                                                              // returns the piece's symbol
        }

        public string GetTypeOfPiece()
        {
            return TypeOfPiece;                                                                         // returns the type of piece it is
        }

        public Player GetBelongsTo()
        {
            return BelongsTo;                                                                           // returns the person that the piece belongs to                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             Ahmed Bad :)
        }

        public int GetPointsIfCaptured()
        {
            return PointsIfCaptured;                                                                    // returns the points that are gained if that piece is captured
        }
    }

    class Square
    {
        protected string Symbol;                                                                        // symbol in square (example: k for kotla)
        protected Piece PieceInSquare;                                                                  // piece that is in a square
        protected Player BelongsTo;                                                                     // player that the piece belongs to

        public Square()
        {
            PieceInSquare = null;                                                                       // defines piece in square as null (therefore no piece is in square)
            BelongsTo = null;                                                                           // defines the person the piece belongs to as null (as no piece in square now) 
            Symbol = " ";                                                                               // defines the default symbol as a space
        }

        public virtual void SetPiece(Piece P)
        {
            PieceInSquare = P;                                                                          // sets the piece in the square as P
        }

        public virtual Piece RemovePiece()
        {
            Piece PieceToReturn = PieceInSquare;                                                        // setting the returned piece to the piece in the square
            PieceInSquare = null;                                                                       // removing the piece in the square from the square
            return PieceToReturn;                                                                       // returning the returned piece 
        }

        public virtual Piece GetPieceInSquare()
        {
            return PieceInSquare;                                                                       // returns the piece in this square
        }

        public virtual string GetSymbol()
        {
            return Symbol;                                                                              // returns the squares symbol (" ", "K" or "k"
        }

        public virtual int GetPointsForOccupancy(Player CurrentPlayer)
        {
            return 0;                                                                                   // returning 0 as the points for occupying a normal square
        }

        public virtual Player GetBelongsTo()
        {
            return BelongsTo;                                                                           // return the player that the square belongs to 
        }

        public virtual bool ContainsKotla()
        {
            if (Symbol == "K" || Symbol == "k")                
            {
                return true;                                                                            // returns true if the square contains a kotla
            }
            else
            {
                return false;                                                                           // returns false if the square does not contain a kotla
            }
        }
    }

    class Kotla : Square
    {
        public Kotla(Player P, string S) : base()                                                       // default intialisation process is done first
        {
            BelongsTo = P;                                                                              // set the person that the square belongs to as the player given
            Symbol = S;                                                                                 // set the symbol as "K" for player one or "k" for player two
        }

        public override int GetPointsForOccupancy(Player CurrentPlayer)
        {
            if (PieceInSquare == null)
            {
                return 0;                                                                               // returning 0 if there is no piece in the kotla square
            }
            else if (BelongsTo.SameAs(CurrentPlayer))
            {
                if (CurrentPlayer.SameAs(PieceInSquare.GetBelongsTo()) && (PieceInSquare.GetTypeOfPiece() == "piece" || PieceInSquare.GetTypeOfPiece() == "mirza"))
                {
                    return 5;                                                                           // returns 5 if the kotla belongs to the current player and the piece in the square also belongs to the current player
                }
                else
                {
                    return 0;                                                                           // returns 0 if the kotla belongs to the current player but the piece in the square does not belong to the current player
                }
            }
            else
            {
                if (CurrentPlayer.SameAs(PieceInSquare.GetBelongsTo()) && (PieceInSquare.GetTypeOfPiece() == "piece" || PieceInSquare.GetTypeOfPiece() == "mirza"))
                {
                    return 1;                                                                           // returns 1 if the kotla doesn't belong to the current player but is occupied by one of the current player pieces 
                }
                else
                {
                    return 0;                                                                           // returns 0 if the kotla belongs to the other player and is occupied by the other players piece
                }
            }
        }
    }

    class MoveOption
    {
        protected string Name;                                                                          // name of the move (example: "Ryott")
        protected List<Move> PossibleMoves;                                                             // list of possible places that can be moved to

        public MoveOption(string N)
        {
            Name = N;                                                                                   // sets the name of the move option
            PossibleMoves = new List<Move>();                                                           // initialises the list of possible moves
        }

        public void AddToPossibleMoves(Move M)
        {
            PossibleMoves.Add(M);                                                                       // adds the new move to the list of possible moves
        }

        public string GetName()
        {
            return Name;                                                                                // returns the name of the move option
        }

        public bool CheckIfThereIsAMoveToSquare(int StartSquareReference, int FinishSquareReference)
        {
            int StartRow = StartSquareReference / 10;                                                   // getting the row number for the starting square from the tens column
            int StartColumn = StartSquareReference % 10;                                                // getting the column number for the starting square from the units column
            int FinishRow = FinishSquareReference / 10;                                                 // getting the row number for the finishing square from the tens column 
            int FinishColumn = FinishSquareReference % 10;                                              // getting the column number for the finishing square from the units column
            foreach (var M in PossibleMoves)                                                            // looping through the possible places that can be moved to
            {
                if (StartRow + M.GetRowChange() == FinishRow && StartColumn + M.GetColumnChange() == FinishColumn)
                {
                    return true;                                                                        // returns true if adding that move to the start position ends up at the finish position
                }
            }
            return false;                                                                               // returns false if none of the moves result in the finish position
        }
    }

    class Move
    {
        protected int RowChange, ColumnChange;                                                          // creates the row change and column change

        public Move(int R, int C)
        {
            RowChange = R;                                                                              // row change is set
            ColumnChange = C;                                                                           // column change is set
        }

        public int GetRowChange()
        {
            return RowChange;                                                                           // returns the change in rows for that move
        }

        public int GetColumnChange()
        {
            return ColumnChange;                                                                        // returns the change in columns for that move
        }
    }

    class MoveOptionQueue
    {
        private List<MoveOption> Queue = new List<MoveOption>();                                        // queue list

        public string GetQueueAsString()
        {
            string QueueAsString = "";                                                                  // setting queue as string to "" by default
            int Count = 1;                                                                              // setting count to 1
            foreach (var M in Queue)
            {
                QueueAsString += Count.ToString() + ". " + M.GetName() + "   ";                         // creating the options int the move option queue
                Count += 1;                                                                             // incrementing count
            }
            return QueueAsString;                                                                       // returning the queue as a string 
        }

        public void Add(MoveOption NewMoveOption)
        {
            Queue.Add(NewMoveOption);                                                                   // adds the move option to the back of the queue
        }

        public void Replace(int Position, MoveOption NewMoveOption)
        {
            Queue[Position] = NewMoveOption;                                                            // sets the item at index Position in the queue is replaced with NewMoveOption
        }

        public void MoveItemToBack(int Position)
        {
            MoveOption Temp = Queue[Position];                                                          // creates a tempoary move option based on move option selected
            Queue.RemoveAt(Position);                                                                   // remoing the move option from the queue
            Queue.Add(Temp);                                                                            // adding the move option to the back of the queue
        }

        public MoveOption GetMoveOptionInPosition(int Pos)
        {
            return Queue[Pos];                                                                          // returns the move at the specified position in the queue 
        }
    }

    class Player
    {
        private string Name;                                                                            // name of the player ("player one" or "player two") 
        private int Direction, Score;                                                                   // direction that the player should move - +1 results in downwards direction whereas -1 results in upwards direction
        private MoveOptionQueue Queue = new MoveOptionQueue();                                          // players queue for the moves that they can have

        public Player(string N, int D)
        {
            Score = 100;                                                                                // setting the players starting score to 100                        
            Name = N;                                                                                   // setting the players name to what has been inputted
            Direction = D;                                                                              // setting the players direction
        }

        public bool SameAs(Player APlayer)
        {
            if (APlayer == null)                                                                    
            {
                return false;                                                                           // if there is no player inputted into the function then false is returned
            }
            else if (APlayer.GetName() == Name)
            {
                return true;                                                                            // if the player inputted shares this player's name then true is returned
            }
            else
            {   
                return false;                                                                           // if the players names do not match then false is returned
            }
        }

        public string GetPlayerStateAsString()
        {
            return Name + Environment.NewLine + "Score: " + Score.ToString() + Environment.NewLine + "Move option queue: " + Queue.GetQueueAsString() + Environment.NewLine;            // returns the players play string (players name, their score and their move option queue)
        }

        public void AddToMoveOptionQueue(MoveOption NewMoveOption)
        {
            Queue.Add(NewMoveOption);                                                                   // adds the move option inputted to the end of the queue
        }

        public void UpdateQueueAfterMove(int Position)
        {
            Queue.MoveItemToBack(Position - 1);                                                         // moves the item to the back of the queue                                                     
        }

        public void UpdateMoveOptionQueueWithOffer(int Position, MoveOption NewMoveOption)
        {
            Queue.Replace(Position, NewMoveOption);                                                     // replaces the move option at currently at index Position in the player's queue with NewMoveOption
        }

        public int GetScore()
        {
            return Score;                                                                               // returns the players score
        }

        public string GetName()
        {
            return Name;                                                                                // returns the players name (player one or player two)
        }

        public int GetDirection()
        {
            return Direction;                                                                           // returns the players direction
        }

        public void ChangeScore(int Amount)
        {
            Score += Amount;                                                                            // increments the score by the amount specified
        }

        public bool CheckPlayerMove(int Pos, int StartSquareReference, int FinishSquareReference)
        {
            MoveOption Temp = Queue.GetMoveOptionInPosition(Pos - 1);                                   // tempoary move option created from what user submitted
            return Temp.CheckIfThereIsAMoveToSquare(StartSquareReference, FinishSquareReference);       // returns true if one of the moves for the move option results in the piece moving from the start square to the finish square
        }
    }
}