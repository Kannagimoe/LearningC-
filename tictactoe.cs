using System;
using System.Data;
using System.Runtime.ExceptionServices;
using System.Xml.Serialization;

class TicTacToe
{
    static string[,] board = new string[3,3];
    static string PlayerName = "default";
    static Random rnd = new Random();
    static int turn = 1; //turn based game. 9 possible slots on 3 by 3 grid so only 9 turns a game. 
    static string move = "A1"; //A1 = 0,0, C2 = 2,1
    static string botname = "TicTacToeMachine"; //future level of difficult different bot names
    static bool bot_first;

       static List<string> insults = new List<string>
        {
            "Nooo! You cheated!",
            "It Doesn't count! I mistyped!",
            "Beginners Luck! Cringe!"
        };


static void Main(string[] args)
{


    Console.Clear();
    DisplayBoard();
           
    Console.WriteLine("Please enter your name:");
    PlayerName = Console.ReadLine();
    Console.WriteLine(" ");
    Console.WriteLine($"Welcome {PlayerName}");
    Console.WriteLine(" ");
    Console.WriteLine("Determining who goes first!");
    int first = rnd.Next(1,3);  // coinflip basically, either 1 or 2.

    if (first == 1) //if 1 then player goes first
    {
       Console.WriteLine($"{PlayerName} goes First"); 
       bot_first = false; 
    }
    else{ // else player goes second.
        Console.WriteLine($"{PlayerName} goes Second");
        bot_first = true;
    }
    Console.WriteLine(" ");
    Console.WriteLine($"{botname}: Hello! So you think you can beat me! >:C"); // bots gotta taunt :3
    Console.WriteLine(" ");
 GameLoop();

}

static void DisplayBoard() //displays current board state
{
    Console.WriteLine(" ");
    Console.WriteLine($"A: {FormatCell(board[0, 0])} | {FormatCell(board[0, 1])} | {FormatCell(board[0, 2])}");
    Console.WriteLine("   --|---|--");
    Console.WriteLine($"B: {FormatCell(board[1, 0])} | {FormatCell(board[1, 1])} | {FormatCell(board[1, 2])}");
    Console.WriteLine("   --|---|--");
    Console.WriteLine($"C: {FormatCell(board[2, 0])} | {FormatCell(board[2, 1])} | {FormatCell(board[2, 2])}");
    Console.WriteLine(" ");
}

static void GameLoop() //main game logic loop
{
       while(turn < 10) // 9 turns
        {
            
            if ((bot_first && turn % 2 == 1) || (!bot_first && turn % 2 == 0))
            {
                //bot's turn
                Console.WriteLine($"Move #{turn}, Player {botname}");
                BotMove(); //bot logic 
                if (VictoryCheck("Bot"))
                {
                    Console.WriteLine(" ");
                    Console.WriteLine($"{botname} wins!");
                    Console.WriteLine(" ");
                    Console.WriteLine($"{botname}: Smell you later loser!");
                    DisplayBoard();
                    return;
                }
            }
            else
            {
                //player's turn
                 Console.WriteLine($"Move #{turn}, Player {PlayerName}");
                PlayerMove();
                if (VictoryCheck("player"))
                {
                    Console.WriteLine(" ");
                    Console.WriteLine($"{PlayerName} wins!");
                    Console.WriteLine(" ");
                    Console.WriteLine($"{botname}: {insults[rnd.Next(insults.Count)]}");
                    Console.WriteLine(" ");
                    DisplayBoard();
                    return;
                }
            }
            turn++;
            DisplayBoard();

    
        }
        Console.WriteLine("Draw!");
}

static void BotMove() // No complex AI just random selection.
{
 bool move_taken = false;
    while(!move_taken)
    {
    int row = rnd.Next(3);
    int col = rnd.Next(3);
   
        if(board[row,col] == null){ // Has player already selected this spot?
            move_taken = true; // escape while loop condition met when row,col is null
            if(bot_first)
            {
                board[row,col] = "O"; 
                    Console.WriteLine(" ");
                    Console.WriteLine($"{botname} has placed an O at {translate_coord(row, col)}");
            }
            else{
                board[row,col] = "X";
                Console.WriteLine(" ");
                Console.WriteLine($"{botname} has placed an X at {translate_coord(row, col)}");
            }
            
    }
    }

}

static void PlayerMove(){
    Console.WriteLine($"Enter a coordinate:");
    Console.WriteLine("(e.g A1O, A1 is top left corner. A2 is top middle ) "); 
    Console.WriteLine(" ");
    
    string choice = Console.ReadLine().ToUpper();
    string Rowchar = choice[0].ToString();
    int col = int.Parse(choice[1].ToString()) - 1;
    var row = coord_translate_to(Rowchar);

            if(bot_first)
            {
                board[row,col] = "X"; 
                    Console.WriteLine($"{PlayerName} has placed an X at {row},{col}");
            }
            else{
                board[row,col] = "O";
                Console.WriteLine($"{PlayerName} has placed an O at {row},{col}");
            }

}

  static bool VictoryCheck(string player)
    {
        string symbol;

    if (player == "Bot")
    {
        if (bot_first)
        {
            symbol = "O";
        }
        else
        {
            symbol = "X";
        }
    }
    else
    {
        if (bot_first)
        {
            symbol = "X";
        }
        else
        {
        symbol = "O";
        }
    }

        // Checks rows, columns and diagonals against symbol for each player symbol being O or X. probably a better way to do this lol
        return (CheckLine(symbol, 0, 0, 0, 1, 0, 2) ||
                CheckLine(symbol, 1, 0, 1, 1, 1, 2) ||
                CheckLine(symbol, 2, 0, 2, 1, 2, 2) ||
                CheckLine(symbol, 0, 0, 1, 0, 2, 0) ||
                CheckLine(symbol, 0, 1, 1, 1, 2, 1) ||
                CheckLine(symbol, 0, 2, 1, 2, 2, 2) ||
                CheckLine(symbol, 0, 0, 1, 1, 2, 2) ||
                CheckLine(symbol, 0, 2, 1, 1, 2, 0)); //diagonals are last two
    } 

    static bool CheckLine(string symbol, int r1, int c1, int r2, int c2, int r3, int c3) // Return bool for if for win con row1, row2, row3, col3
    {
        return (board[r1, c1] == symbol && board[r2, c2] == symbol && board[r3, c3] == symbol);
    }

static string translate_coord(int row, int col)
{
    string rowChar;

    if (row == 0)
    {
        rowChar = "A";
    }
    else if (row == 1)
    {
        rowChar = "B";
    }
    else if (row == 2)
    {
        rowChar = "C";
    }
    else
    {
        throw new ArgumentOutOfRangeException();
    }

    return $"{rowChar}{col + 1}";
}

static int coord_translate_to(string row)
{
    if (row == "A")
    {
        return 0;
    }
    else if (row == "B")
    {
        return 1;
    }
    else if (row == "C")
    {
        return 2;
    }
    else
    {
        throw new ArgumentException("Invalid row character");
    }
}

static string FormatCell(string cell)
{
    return string.IsNullOrEmpty(cell) ? " " : cell;
}

}
