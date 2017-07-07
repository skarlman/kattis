using System;
using System.Collections.Generic;
using System.Linq;

namespace KnightsInAFen
{
    public class Program
    {
        static void Main(string[] args)
        {
            var numberOfBoards = int.Parse(Console.ReadLine());
            var rawBoards = new List<string>(numberOfBoards*5);

            for (int i = 0; i < numberOfBoards*5; i++)
            {
                rawBoards.Add(Console.ReadLine());
            }

            var result = CalculateMoves(rawBoards);

            result.ForEach(r =>
            {
                if (r <= 10)
                    Console.WriteLine("Solvable in {0} move(s).", r);
                else
                    Console.WriteLine("Unsolvable in less than 11 move(s).");
            });
        }

        public static List<int> CalculateMoves(List<string> input)
        {
            var result = new List<int>();
            var numberOfBoards = input.Count/5;

            for (int i = 0; i < numberOfBoards; i++)
            {
                var currentBoard = input.Skip(i*5).Take(5).ToList();
                result.Add(SolveBoard(currentBoard));
            }

            return result;
        }

        private static int SolveBoard(List<string> board)
        {
            var boardsToExplore = new Queue<BoardWithLevel>();

            var startingBoard = new BoardWithLevel(board, FindEmptySquare(board));
            boardsToExplore.Enqueue(startingBoard);

            var seenBoards = new Dictionary<string, bool>();

            while (boardsToExplore.Count > 0)
            {
                var currentBoard = boardsToExplore.Dequeue();

                if (BoardIsComplete(currentBoard))
                {
                    return currentBoard.Level;
                }

                var currentBoardKey = string.Join("",currentBoard);

                if (seenBoards.ContainsKey(currentBoardKey))
                {
                    continue;
                }

                seenBoards.Add(currentBoardKey, true);

                var numberOfMisplacedPieces = NumberOfMisplacedPieces(currentBoard);
                var minimumTotalMoves = (numberOfMisplacedPieces-1)+currentBoard.Level;

                if (currentBoard.Level < 10 && minimumTotalMoves < 11)
                {
                    var emptySquare = currentBoard.EmptySquarePosition;
                    var possibleMoves = GetAllPossibleMoves(emptySquare);

                    foreach (var move in possibleMoves)
                    {
                        var newBoard = CreateUpdatedBoard(currentBoard, emptySquare, move);
                        var newBoardKey = string.Join("", newBoard);

                        if (!seenBoards.ContainsKey(newBoardKey))
                        {
                            boardsToExplore.Enqueue(new BoardWithLevel(newBoard.ToList(), move, currentBoard.Level + 1));
                        }
                    }
                }
            }
            return 11;
        }

        private static string[] CreateUpdatedBoard(BoardWithLevel currentBoard, Position emptySquare, Position move)
        {
            var newBoard = new string[5];
            currentBoard.CopyTo(newBoard);

            var theRow = newBoard[emptySquare.Row].ToCharArray();
            theRow[emptySquare.Column] = newBoard[move.Row][move.Column];
            newBoard[emptySquare.Row] = new string(theRow);

            theRow = newBoard[move.Row].ToCharArray();
            theRow[move.Column] = ' ';
            newBoard[move.Row] = new string(theRow);

            return newBoard;
        }


        static readonly string[] CompletedBoard = new string[5]
        {
                        "11111",
                        "01111",
                        "00 11",
                        "00001",
                        "00000"
        };

        public static int NumberOfMisplacedPieces(List<string> board)
        {
            var counter = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (board[i][j] != CompletedBoard[i][j])
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }
        public static bool BoardIsComplete(List<string> board)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (board[i][j] != CompletedBoard[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static Position FindEmptySquare(List<string> currentBoard)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (currentBoard[i][j] == ' ')
                    {
                        return new Position(i, j);
                    }
                }
            }
            return null;
        }

        public class Position 
        {
            public Position(int row, int column)
            {
                Row = row;
                Column = column;
            }

            public int Row { get; set; }
            public int Column { get; set; }
        }

        public class BoardWithLevel : List<string>
        {
            public int Level { get; set; }

            public Position EmptySquarePosition { get; set; }

            public BoardWithLevel(List<string> collection, Position emptySquare, int level=0) : base(collection)
            {
                this.Level = level;
                this.EmptySquarePosition = emptySquare;
            }
        }

        public static List<Position> GetAllPossibleMoves(Position emptySquare)
        {
            var result = new List<Position>();

            result.Add(new Position(emptySquare.Row-2, emptySquare.Column-1));
            result.Add(new Position(emptySquare.Row-2, emptySquare.Column+1));
            result.Add(new Position(emptySquare.Row+2, emptySquare.Column-1));
            result.Add(new Position(emptySquare.Row+2, emptySquare.Column+1));

            result.Add(new Position(emptySquare.Row-1, emptySquare.Column-2));
            result.Add(new Position(emptySquare.Row-1, emptySquare.Column+2));
            result.Add(new Position(emptySquare.Row+1, emptySquare.Column-2));
            result.Add(new Position(emptySquare.Row+1, emptySquare.Column+2));

            return result.Where(p => p.Row >= 0 && p.Column >= 0 && p.Row < 5 && p.Column < 5).ToList();
        }
    }
}
