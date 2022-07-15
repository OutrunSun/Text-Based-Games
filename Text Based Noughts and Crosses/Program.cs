using System;
using System.Collections.Generic;
using System.Linq;

namespace Text_Based_Noughts_and_Crosses
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.Write("NOUGHTS AND CROSSES - C# CONSOLE EDITION");
            Console.Write("\nBy Noah Ramdhony\n");
            Console.ReadLine();
            GameLoop();
            Console.Write("Thanks for Playing");
        }

        static void GameLoop()
        {
            char[,] board = {{' ', ' ', ' '}, {' ', ' ', ' '}, {' ', ' ', ' '}, {'0', ' ', ' '}}; // Last Value indicates the state of play 3 = Loss 1 = Win

            string playerSelection;

            Random turnDecider = new Random();

            int turn = turnDecider.Next(0, 10) % 2; // MOD -> 0 = Player, 1 = Bot

            while (board[3, 0] == '0')
            {   
                Renderer(board);

                if (turn % 2 == 0)
                {
                    board = PlayerMove(board);
                }
                else
                {
                    board = BotMove(board);
                    //Console.ReadLine(); // Add to bugfix
                }

                board = WinChecker(board);

                switch (board[3, 0])
                {
                    case '1':
                        Renderer(board);
                        Console.Write("\n\nYou Won\n");
                        break;

                    case '2':
                        Renderer(board);
                        Console.Write("\n\nYou Drew\n");
                        break;

                    case '3':
                        Renderer(board);
                        Console.Write("\n\nYou Lost\n");
                        break;
                }
                turn++;
                

            }
        }

        static char[,] BotMove(char[,] board)
        {
            // A line to make the dev mode console look better
            Console.Write("\nPotentials:\n");

            // Lists for positions of potential plays:
            List<int> xLocals = new List<int>();
            List<int> yLocals = new List<int>();

            // Order of below things defines its priority - most potentials can be offensive or defensive:

            for (int y = 0; y < 3; y++)
            {

                // Checks for pair with space (Vertical):
                if (board[y, 0] == board[y, 2] && board[y, 1] == ' ' && board[y, 2] != ' ')
                {
                    xLocals.Add(1);
                    yLocals.Add(y);
                    Console.Write("[" + xLocals.Last() + ", " + yLocals.Last() + "] Added - Middle Gap (V)\n");
                }
                // Checks for pair with space (Horizontal):
                if (board[0, y] == board[2, y] && board[1, y] == ' ' && board[2, y] != ' ')
                {
                    xLocals.Add(y);
                    yLocals.Add(1);
                    Console.Write("[" + xLocals.Last() + ", " + yLocals.Last() + "] Added - Middle Gap (H)\n");
                }


                // Two in a row
                for (int x = 0; x < 2; x++)
                {
                    // Checks for two in a row (Vertical):
                    if (board[y, x] == board[y, x + 1] && board[y, x] != ' ')
                    {
                        yLocals.Add(y);
                        switch (x)
                        {
                            case 0:
                                xLocals.Add(x + 2);
                                break;
                            
                            case 1:
                                xLocals.Add(x - 1);
                                break;
                        }
                        Console.Write("[" + xLocals.Last() + ", " + yLocals.Last() + "] Added - Two Items in a row (V)\n");
                    }
                    // Checks for two in a row (Horizontal)(Variables are the wrong way round to make it be in one loop):
                    if (board[x, y] == board[x + 1, y] && board[x, y] != ' ')
                    {
                        xLocals.Add(y);
                        switch (x)
                        {
                            case 0:
                                yLocals.Add(x + 2);
                                break;
                            
                            case 1:
                                yLocals.Add(x - 1);
                                break;
                        }
                        Console.Write("[" + xLocals.Last() + ", " + yLocals.Last() + "] Added - Two Items in a row (H)\n");
                    }
                }
            }

            // Checks for two in a row (Diagonal):
            for (int i = -1; i < 2; i+=2)
            {
                for (int j = -1; j < 2; j+=2)
                {
                    // Adds the location of a blocking corner:
                    if (board[1, 1] == board[1 + i, 1 + j] && board[1, 1] != ' ')
                    {
                        xLocals.Add((j * -1) + 1);
                        yLocals.Add((i * -1) + 1);
                        Console.Write("[" + xLocals.Last() + ", " + yLocals.Last() + "] Added - Two Items in a row (D)\n");
                    }
                }
            }

            // Double Corner trick counter:
            if (board[0, 0] == board[2, 2] && board[0, 0] == 'X')
            {
                xLocals.Add(0);
                yLocals.Add(1);
                Console.Write("[" + xLocals.Last() + ", " + yLocals.Last() + "] Added - Corner Trap Counter (V1)\n");
            }
            else if (board[0, 2] == board[2, 0] && board[0, 2] == 'X')
            {
                xLocals.Add(2);
                yLocals.Add(1);
                Console.Write("[" + xLocals.Last() + ", " + yLocals.Last() + "] Added - Corner Trap Counter (V2)\n");
            }


            

            // Checks for locations of priority positions
            // Middle
            if (board[1, 1] == ' ')
            {
                xLocals.Add(1);
                yLocals.Add(1);
                Console.Write("[" + xLocals.Last() + ", " + yLocals.Last() + "] Added - Middle Location\n");
            }

            // Corners:
            for (int i = 0; i < 3; i+=2)
            {
                for (int j = 0; j < 3; j+=2)
                {
                    if (board[i, j] == ' ')
                    {
                        xLocals.Add(j);
                        yLocals.Add(i);
                        Console.Write("[" + xLocals.Last() + ", " + yLocals.Last() + "] Added - Corner Locations\n");
                    }
                }
            }
            
            // The Rest (Default moves)
            for (int i = 0; i < 3; i+=2)
            {
                if (board[i, 1] == ' ')
                {
                    xLocals.Add(1);
                    yLocals.Add(i);
                    Console.Write("[" + xLocals.Last() + ", " + yLocals.Last() + "] Added - Empty Locations\n");
                }
            }
            for (int i = 0; i < 3; i+=2)
            {
                if (board[1, i] == ' ')
                {
                    xLocals.Add(i);
                    yLocals.Add(1);
                    Console.Write("[" + xLocals.Last() + ", " + yLocals.Last() + "] Added - Empty Locations\n");
                }
            }


            // Proccesses the potential Moves
            for (int potential = 0; potential < xLocals.Count; potential++)
            {
                if (board[yLocals[potential], xLocals[potential]] == ' ')
                {
                    board[yLocals[potential], xLocals[potential]] = 'O';
                    break;
                }
            }
            return board;
        }

        static char[,] PlayerMove(char[,] board)
        {
            int xCoord = 0;
            int yCoord = 0;
            string playerSelection;

            bool validInput = false;

            do
            {
                
                Console.Write("\nInput Move: ");
                playerSelection = Console.ReadLine().ToUpper();

                // Converts Letter into index
                switch (playerSelection[0])
                {
                    case 'A':
                        xCoord = 0;
                        break;

                    case 'B':
                        xCoord = 1;
                        break;
                    
                    case 'C':
                        xCoord = 2;
                        break;
                    
                    case 'E':
                        xCoord = 0;
                        playerSelection = "E0"; // Beacuse I don't want the console bitching at me
                        board[3, 0] = '2'; // Breaks the Gameloop
                        break;
                }
                yCoord = playerSelection[1] - '0';

                try
                {
                    if (board[xCoord, yCoord - 1] == ' ')
                    {
                        board[xCoord, yCoord - 1] = 'X';
                        validInput = true;
                    }
                }
                catch (System.IndexOutOfRangeException){}
            } while (validInput == false);

            return board;
        }

        static char[,] WinChecker(char[,] board)
        {
            // Counter for how many places
            int filledCount = 0;

            // Diagonal Win:
            if ((board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) || (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]))
            {
                switch (board[1, 1])
                {
                    case 'X':
                        board[3, 0] = '1';
                        break;
                    
                    case 'O':
                        board[3, 0] = '3';
                        break;
                } 
            }

            for (int i = 0; i < 3; i++)
            {
                // Horizontal Win:
                if (board[0, i] == board[1, i] && board[1, i] == board[2, i])
                {
                    switch (board[0, i])
                    {
                        case 'X':
                            board[3, 0] = '1';
                            break;
                        
                        case 'O':
                            board[3, 0] = '3';
                            break;
                    }
                } // Vertical Win:
                else if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                {
                    switch (board[i, 0])
                    {
                        case 'X':
                            board[3, 0] = '1';
                            break;
                        
                        case 'O':
                            board[3, 0] = '3';
                            break;
                    }
                }

                // Draw condition:
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] != ' ')
                    {
                        filledCount++;
                    }
                }
                if (filledCount == 9)
                {
                    board[3, 0] = '2';
                }
                
            }
            
            return board;
        }

        static void Renderer(char[,] board)
        {
            Console.Clear();

            Console.Write("NOUGHTS AND CROSSES - C# CONSOLE EDITION\n");

            Console.Write("\n   A   B   C\n");
            for (int y = 0; y < 3; y++)
            {
                Console.Write((y + 1) + "  " + board[0, y] + " | " + board[1, y] + " | " + board[2, y]);
                if (y != 2)
                {
                    Console.Write("\n  -----------\n");
                }
            }
        }
    }
}

