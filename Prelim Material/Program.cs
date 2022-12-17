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
            Console.Write(Environment.NewLine + "   ");
            for (int Column = 1; Column <= NoOfColumns; Column++)
            {
                Console.Write(Column.ToString() + "  ");
            }
            Console.Write(Environment.NewLine + "  ");
            for (int Count = 1; Count <= NoOfColumns; Count++)
            {
                Console.Write("---");
            }
            Console.WriteLine("-");
            for (int Row = 1; Row <= NoOfRows; Row++)
            {
                Console.Write(Row.ToString() + " ");
                for (int Column = 1; Column <= NoOfColumns; Column++)
                {
                    int Index = GetIndexOfSquare(Row * 10 + Column);
                    Console.Write("|" + Board[Index].GetSymbol());
                    Piece PieceInSquare = Board[Index].GetPieceInSquare();
                    if (PieceInSquare == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(PieceInSquare.GetSymbol());
                    }
                }
                Console.WriteLine("|");
            }
            Console.Write("  -");
            for (int Column = 1; Column <= NoOfColumns; Column++)
            {
                Console.Write("---");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private void DisplayState()
        {
            DisplayBoard();
            Console.WriteLine("Move option offer: " + MoveOptionOffer[MoveOptionOfferPosition]);
            Console.WriteLine();
            Console.WriteLine(CurrentPlayer.GetPlayerStateAsString());
            Console.WriteLine("Turn: " + CurrentPlayer.GetName());
            Console.WriteLine();
        }

        private int GetIndexOfSquare(int SquareReference)
        {
            int Row = SquareReference / 10;                                                             // row is the value in the tens column of SquareReference
            int Col = SquareReference % 10;                                                             // column is the value in the units column of SquareReference
            return (Row - 1) * NoOfColumns + (Col - 1);                                                 // returns the index of the square in the list (example: 42 is converted to 19)
        }

        private bool CheckSquareInBounds(int SquareReference)
        {
            int Row = SquareReference / 10;
            int Col = SquareReference % 10;
            if (Row < 1 || Row > NoOfRows)
            {
                return false;
            }
            else if (Col < 1 || Col > NoOfColumns)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckSquareIsValid(int SquareReference, bool StartSquare)
        {
            if (!CheckSquareInBounds(SquareReference))
            {
                return false;
            }
            Piece PieceInSquare = Board[GetIndexOfSquare(SquareReference)].GetPieceInSquare();
            if (PieceInSquare == null)
            {
                if (StartSquare)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (CurrentPlayer.SameAs(PieceInSquare.GetBelongsTo()))
            {
                if (StartSquare)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (StartSquare)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private bool CheckIfGameOver()
        {
            bool Player1HasMirza = false;
            bool Player2HasMirza = false;
            foreach (var S in Board)
            {
                Piece PieceInSquare = S.GetPieceInSquare();
                if (PieceInSquare != null)
                {
                    if (S.ContainsKotla() && PieceInSquare.GetTypeOfPiece() == "mirza" && !PieceInSquare.GetBelongsTo().SameAs(S.GetBelongsTo()))
                    {
                        return true;
                    }
                    else if (PieceInSquare.GetTypeOfPiece() == "mirza" && PieceInSquare.GetBelongsTo().SameAs(Players[0]))
                    {
                        Player1HasMirza = true;
                    }
                    else if (PieceInSquare.GetTypeOfPiece() == "mirza" && PieceInSquare.GetBelongsTo().SameAs(Players[1]))
                    {
                        Player2HasMirza = true;
                    }
                }
            }
            return !(Player1HasMirza && Player2HasMirza);
        }

        private int GetSquareReference(string Description)
        {
            int SelectedSquare;
            Console.Write("Enter the square " + Description + " (row number followed by column number): ");
            SelectedSquare = Convert.ToInt32(Console.ReadLine());
            return SelectedSquare;
        }

        private void UseMoveOptionOffer()
        {
            int ReplaceChoice;
            Console.Write("Choose the move option from your queue to replace (1 to 5): ");
            ReplaceChoice = Convert.ToInt32(Console.ReadLine());
            CurrentPlayer.UpdateMoveOptionQueueWithOffer(ReplaceChoice - 1, CreateMoveOption(MoveOptionOffer[MoveOptionOfferPosition], CurrentPlayer.GetDirection()));
            CurrentPlayer.ChangeScore(-(10 - (ReplaceChoice * 2)));
            MoveOptionOfferPosition = RGen.Next(0, 5);
        }

        private int GetPointsForOccupancyByPlayer(Player CurrentPlayer)
        {
            int ScoreAdjustment = 0;
            foreach (var S in Board)
            {
                ScoreAdjustment += (S.GetPointsForOccupancy(CurrentPlayer));
            }
            return ScoreAdjustment;
        }

        private void UpdatePlayerScore(int PointsForPieceCapture)
        {
            CurrentPlayer.ChangeScore(GetPointsForOccupancyByPlayer(CurrentPlayer) + PointsForPieceCapture);
        }

        private int CalculatePieceCapturePoints(int FinishSquareReference)
        {
            if (Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare() != null)
            {
                return Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare().GetPointsIfCaptured();
            }
            return 0;
        }

        public void PlayGame()
        {
            bool GameOver = false;
            while (!GameOver)
            {
                DisplayState();
                bool SquareIsValid = false;
                int Choice;
                do
                {
                    Console.Write("Choose move option to use from queue (1 to 3) or 9 to take the offer: ");
                    Choice = Convert.ToInt32(Console.ReadLine());
                    if (Choice == 9)
                    {
                        UseMoveOptionOffer();
                        DisplayState();
                    }
                }
                while (Choice < 1 || Choice > 3);
                int StartSquareReference = 0;
                while (!SquareIsValid)
                {
                    StartSquareReference = GetSquareReference("containing the piece to move");
                    SquareIsValid = CheckSquareIsValid(StartSquareReference, true);
                }
                int FinishSquareReference = 0;
                SquareIsValid = false;
                while (!SquareIsValid)
                {
                    FinishSquareReference = GetSquareReference("to move to");
                    SquareIsValid = CheckSquareIsValid(FinishSquareReference, false);
                }
                bool MoveLegal = CurrentPlayer.CheckPlayerMove(Choice, StartSquareReference, FinishSquareReference);
                if (MoveLegal)
                {
                    int PointsForPieceCapture = CalculatePieceCapturePoints(FinishSquareReference);
                    CurrentPlayer.ChangeScore(-(Choice + (2 * (Choice - 1))));
                    CurrentPlayer.UpdateQueueAfterMove(Choice);
                    UpdateBoard(StartSquareReference, FinishSquareReference);
                    UpdatePlayerScore(PointsForPieceCapture);
                    Console.WriteLine("New score: " + CurrentPlayer.GetScore() + Environment.NewLine);
                }
                if (CurrentPlayer.SameAs(Players[0]))
                {
                    CurrentPlayer = Players[1];
                }
                else
                {
                    CurrentPlayer = Players[0];
                }
                GameOver = CheckIfGameOver();
            }
            DisplayState();
            DisplayFinalResult();
        }

        private void UpdateBoard(int StartSquareReference, int FinishSquareReference)
        {
            Board[GetIndexOfSquare(FinishSquareReference)].SetPiece(Board[GetIndexOfSquare(StartSquareReference)].RemovePiece());
        }

        private void DisplayFinalResult()
        {
            if (Players[0].GetScore() == Players[1].GetScore())
            {
                Console.WriteLine("Draw!");
            }
            else if (Players[0].GetScore() > Players[1].GetScore())
            {
                Console.WriteLine(Players[0].GetName() + " is the winner!");
            }
            else
            {
                Console.WriteLine(Players[1].GetName() + " is the winner!");
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
            Players[0].AddToMoveOptionQueue(CreateMoveOption("chowkidar", 1));                          // adds ryott move where forwards direction is downwards    
            Players[0].AddToMoveOptionQueue(CreateMoveOption("cuirassier", 1));                         // adds ryott move where forwards direction is downwards
            Players[0].AddToMoveOptionQueue(CreateMoveOption("faujdar", 1));                            // adds ryott move where forwards direction is downwards
            Players[0].AddToMoveOptionQueue(CreateMoveOption("jazair", 1));                             // adds ryott move where forwards direction is downwards
            Players[1].AddToMoveOptionQueue(CreateMoveOption("ryott", -1));                             // adds ryott move where forwards direction is downwards
            Players[1].AddToMoveOptionQueue(CreateMoveOption("chowkidar", -1));                         // adds ryott move where forwards direction is downwards
            Players[1].AddToMoveOptionQueue(CreateMoveOption("jazair", -1));                            // adds ryott move where forwards direction is downwards
            Players[1].AddToMoveOptionQueue(CreateMoveOption("faujdar", -1));                           // adds ryott move where forwards direction is downwards    
            Players[1].AddToMoveOptionQueue(CreateMoveOption("cuirassier", -1));                        // adds ryott move where forwards direction is downwards
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
            return Symbol;
        }

        public string GetTypeOfPiece()
        {
            return TypeOfPiece;
        }

        public Player GetBelongsTo()
        {
            return BelongsTo;
        }

        public int GetPointsIfCaptured()
        {
            return PointsIfCaptured;
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
            PieceInSquare = P;                                                                          // sets the piece in the square as  P
        }

        public virtual Piece RemovePiece()
        {
            Piece PieceToReturn = PieceInSquare;
            PieceInSquare = null;
            return PieceToReturn;
        }

        public virtual Piece GetPieceInSquare()
        {
            return PieceInSquare;
        }

        public virtual string GetSymbol()
        {
            return Symbol;
        }

        public virtual int GetPointsForOccupancy(Player CurrentPlayer)
        {
            return 0;
        }

        public virtual Player GetBelongsTo()
        {
            return BelongsTo;
        }

        public virtual bool ContainsKotla()
        {
            if (Symbol == "K" || Symbol == "k")
            {
                return true;
            }
            else
            {
                return false;
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
                return 0;
            }
            else if (BelongsTo.SameAs(CurrentPlayer))
            {
                if (CurrentPlayer.SameAs(PieceInSquare.GetBelongsTo()) && (PieceInSquare.GetTypeOfPiece() == "piece" || PieceInSquare.GetTypeOfPiece() == "mirza"))
                {
                    return 5;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (CurrentPlayer.SameAs(PieceInSquare.GetBelongsTo()) && (PieceInSquare.GetTypeOfPiece() == "piece" || PieceInSquare.GetTypeOfPiece() == "mirza"))
                {
                    return 1;
                }
                else
                {
                    return 0;
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
            return Name;
        }

        public bool CheckIfThereIsAMoveToSquare(int StartSquareReference, int FinishSquareReference)
        {
            int StartRow = StartSquareReference / 10;
            int StartColumn = StartSquareReference % 10;
            int FinishRow = FinishSquareReference / 10;
            int FinishColumn = FinishSquareReference % 10;
            foreach (var M in PossibleMoves)
            {
                if (StartRow + M.GetRowChange() == FinishRow && StartColumn + M.GetColumnChange() == FinishColumn)
                {
                    return true;
                }
            }
            return false;
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
            return RowChange;
        }

        public int GetColumnChange()
        {
            return ColumnChange;
        }
    }

    class MoveOptionQueue
    {
        private List<MoveOption> Queue = new List<MoveOption>();

        public string GetQueueAsString()
        {
            string QueueAsString = "";
            int Count = 1;
            foreach (var M in Queue)
            {
                QueueAsString += Count.ToString() + ". " + M.GetName() + "   ";
                Count += 1;
            }
            return QueueAsString;
        }

        public void Add(MoveOption NewMoveOption)
        {
            Queue.Add(NewMoveOption);
        }

        public void Replace(int Position, MoveOption NewMoveOption)
        {
            Queue[Position] = NewMoveOption;
        }

        public void MoveItemToBack(int Position)
        {
            MoveOption Temp = Queue[Position];
            Queue.RemoveAt(Position);
            Queue.Add(Temp);
        }

        public MoveOption GetMoveOptionInPosition(int Pos)
        {
            return Queue[Pos];
        }
    }

    class Player
    {
        private string Name;                                                                        // name of the player ("player one" or "player two") 
        private int Direction, Score;                                                               // direction that the player should move - +1 results in downwards direction whereas -1 results in upwards direction
        private MoveOptionQueue Queue = new MoveOptionQueue();                                      // players queue for the moves that they can have

        public Player(string N, int D)
        {
            Score = 100;                                                                            // setting the players starting score to 100                        
            Name = N;                                                                               // setting the players name to what has been inputted
            Direction = D;                                                                          // setting the players direction
        }

        public bool SameAs(Player APlayer)
        {
            if (APlayer == null)
            {
                return false;
            }
            else if (APlayer.GetName() == Name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetPlayerStateAsString()
        {
            return Name + Environment.NewLine + "Score: " + Score.ToString() + Environment.NewLine + "Move option queue: " + Queue.GetQueueAsString() + Environment.NewLine;
        }

        public void AddToMoveOptionQueue(MoveOption NewMoveOption)
        {
            Queue.Add(NewMoveOption);                                                               // adds the move option inputted to the end of the queue
        }

        public void UpdateQueueAfterMove(int Position)
        {
            Queue.MoveItemToBack(Position - 1);
        }

        public void UpdateMoveOptionQueueWithOffer(int Position, MoveOption NewMoveOption)
        {
            Queue.Replace(Position, NewMoveOption);
        }

        public int GetScore()
        {
            return Score;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetDirection()
        {
            return Direction;
        }

        public void ChangeScore(int Amount)
        {
            Score += Amount;
        }

        public bool CheckPlayerMove(int Pos, int StartSquareReference, int FinishSquareReference)
        {
            MoveOption Temp = Queue.GetMoveOptionInPosition(Pos - 1);
            return Temp.CheckIfThereIsAMoveToSquare(StartSquareReference, FinishSquareReference);
        }
    }
}