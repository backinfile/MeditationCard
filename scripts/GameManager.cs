using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager 
{
    public static Board board;

    public static void StartGame()
    {

        board = new Board();
    }

    public static async Task Update()
    {
        if (board != null)
        {
            await board.Update();
        }
    }
}
