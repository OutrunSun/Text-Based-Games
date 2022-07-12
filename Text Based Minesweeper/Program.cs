using System;

namespace Text_Based_Minesweeper{
    class Program{
        static void Main(string[] args){
            Console.Clear();
            Setup();
            Console.Write("Thanks for playing");
        }

        static void Setup(){
            Console.WriteLine("MINESWEEPER - C# CONSOLE EDITION");
            Console.WriteLine("By Noah Ramdhony");

            // Width Input:
            Console.Write("Input Width (MAX: 25): ");
            int width = Convert.ToInt32(Console.ReadLine());
                
            
            // Height Input
            Console.Write("Input Height(MAX: 10): ");
            int height = Convert.ToInt32(Console.ReadLine());
            
            // Difficulty Input
            Console.Write("Input Difficulty(1 to 5): ");
            int difficulty = (6 - (Convert.ToInt32(Console.ReadLine()))) * 2;
            // Passes values to the Generator
            Generator(width, height, difficulty);
        }

        static void Generator(int w = 5, int h = 5, int difficulty = 2){
            // Defines the total bombs in the area as an int
            int totalBombs = (w * h) / difficulty;

            // Defines an array with volume specified in Setup()
            int[,] hiddenGrid = new int[w, h];

            // Defines an array that the user sees
            char[,] displayedGrid = new char[w, h];

            // Adds the 'bombs' to the hiddenGrid - indicated by a 9 Value
            Random Coord = new Random();

            //Random yCoord = new Random();
            int x, y = 0;
            for (int i = totalBombs; i > -1; i--){
                x = Coord.Next(0, w);
                y = Coord.Next(0, h);
                if (hiddenGrid[x, y] != 9){
                    hiddenGrid[x, y] = 9;
                }
                else{
                    i++;
                };
            }

            // Assigns Values to both grids
            // Counts the occurances of bombs
            int counter = 0;
            for (int x2 = 0; x2 < w; x2++){
                for (int y2 = 0; y2 < h; y2++){
                    
                    // Sets all values int he visible grid to '?'
                    displayedGrid[x2, y2] = '?';

                    // Skips all bombs in hidden Grid
                    if (hiddenGrid[x2, y2] != 9){
                        counter = 0;
                        for (int modifierX = -1; modifierX < 2; modifierX++){
                            for (int modifierY = -1; modifierY < 2; modifierY++){
                                try{
                                    // Counts how many adjacent blocks have a bomb
                                    if (hiddenGrid[(x2 + modifierX), (y2 + modifierY)] == 9)
                                    {
                                        counter++;
                                    }
                                }
                                // Skips attempts outside of the bounds
                                catch (System.IndexOutOfRangeException){}
                            }
                        }
                        // Sets the block to a value of how many bombs adjacent
                        hiddenGrid[x2, y2] = counter;
                    }
                }
            }
            // Passes control over to the GameLoop
            GameLoop(w, h, displayedGrid, hiddenGrid, totalBombs);
        }

        static void DeveloperRenderer(int w, int h, char[,] displayedGrid, int[,] hiddenGrid, int bombTotal, int flagsPlaced, bool loss, int turns){
            Console.Clear();
            Console.Write("MINESWEEPER - C# CONSOLE EDITION\n");
            Console.Write("? = Hidden, ! = Flagged, # = Exploded\nDEVELOPER RENDER\n");

            // Draws the bar of X coordinates
            string topBar = "% A B C D E F G H I J K L M N O P Q R S T U V W X Y ";
            Console.Write(topBar.Substring(0, ((w + 1) * 2)));

            // Draws the Displayed Grid itself
            for (int y = 0; y < (h); y++){
                Console.Write("\n" + y + " ");
                for (int x = 0; x < w; x++){
                    Console.Write(hiddenGrid[x, y] + " ");
                }
            }

        }
        

        static void Renderer(int w, int h, char[,] displayedGrid, int bombTotal, int flagsPlaced, bool loss, int turns){
            Console.Clear();
            Console.Write("MINESWEEPER - C# CONSOLE EDITION\n");
            Console.Write("? = Hidden, ! = Flagged, # = Exploded\n");

            // Draws the bar of X coordinates
            string topBar = "% A B C D E F G H I J K L M N O P Q R S T U V W X Y ";
            Console.Write(topBar.Substring(0, ((w + 1) * 2)));

            // Draws the Displayed Grid itself
            for (int y = 0; y < (h); y++){
                Console.Write("\n" + y + " ");
                for (int x = 0; x < w; x++){
                    Console.Write(displayedGrid[x, y] + " ");
                }
            }

            Console.Write("\nFlags Used: " + flagsPlaced + "/" + bombTotal + ", Turns Used: " + turns + "\n");

            if (loss){
                Console.Write("\nYOU BLEW TO SMITHEREENS - Game Over\n\n");
            }
        }

        static void GameLoop(int w, int h, char[,] displayedGrid, int[,] hiddenGrid, int bombTotal){
            // Establishes the reused variables
            bool playing = true;
            bool loss = false;
            int selectedY, selectedX, turns = 0;
            int flagsPlaced = 0;
            char type, selectedXLetter;
            string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXY";


            do{
                // Draws to console
                Renderer(w, h, displayedGrid, bombTotal, flagsPlaced, loss, turns);

                // X Input
                Console.Write("\nInput X: ");
                selectedXLetter = Convert.ToChar((Console.ReadLine()).ToUpper());
                selectedX = Alphabet.IndexOf(selectedXLetter);
                
                // Y Input
                Console.Write("Input Y: ");
                selectedY = Convert.ToInt32(Console.ReadLine());

                // Command Type input
                // F = Flag, U = Unveil, R = Remove
                Console.Write("Input Type(F = Flag, U = Unveil, R = Remove): ");
                type = Convert.ToChar((Console.ReadLine()).ToUpper());

                switch (type){
                    case 'F':
                        if (hiddenGrid[selectedX, selectedY] != '?'){
                            displayedGrid[selectedX,selectedY] = '!'; 
                        }
                        break;

                    case 'U':
                        if (hiddenGrid[selectedX, selectedY] == 9){
                            displayedGrid[selectedX, selectedY] = '#';
                            loss = true;
                            Renderer(w, h, displayedGrid, bombTotal, flagsPlaced, loss, turns);
                            playing = false;
                        }
                        else{
                            displayedGrid[selectedX, selectedY] = Convert.ToChar($"{hiddenGrid[selectedX, selectedY]}");
                        }
                        break;

                    case 'R':
                        if (displayedGrid[selectedX,selectedY] == '!'){
                            displayedGrid[selectedX, selectedY] = '?';
                        }
                        break;

                    case 'D':
                        DeveloperRenderer(w, h, displayedGrid, hiddenGrid, bombTotal, flagsPlaced, loss, turns);
                        Console.ReadLine();
                        break;
                }
                turns++;
            } while (playing);
        } 
    }
}
