using Godot;
using System;

public static class Res 
{
    public const int PlaygoundSize = 7;


    public const float CardWidth = 140;
    public const float CardHeight = 196;

    public const float HandCardX_Gap = CardWidth * -0.1f;
    public const float PlaygoundCardX_Gap = CardWidth * 0.8f;

    public const float MOVE_INTERVAL = 0.13f;

    public const float SCALE_NORMAL = 1f;
    public const float SCALE_HAND = 1f;
    public const float SCALE_DRAG = 1.2f;
    public const float SCALE_PLAYGROUNG = 0.6f;


    public const int ZIndex_Card_To_Select = 200;
    public const int ZIndex_Mask = 100;
    public const int ZIndex_Drag = -100;
    public const int ZIndex_Hand = -200;
    public const int ZIndex_Board = -300;
}
