using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Demo
{
    internal class Game //遊戲邏輯的程式碼
    {
        private Board board = new Board();

        // 目前的棋子顏色 (預設為黑色)
        private PieceType currentPlayer = PieceType.BLACK;

        // 贏家棋子的顏色 (預設為NONE)
        private PieceType winner = PieceType.NONE;

        // ↑用get存取器獲得checkWinner()裡面回傳winner現在的結果 ，給Form1_MouseDown用
        public PieceType Winner { get { return winner; } }

        //-----------------------------------------------------------------------------------

        // Form1_MouseMove用
        public bool CanBePlaced(int x, int y)
        {
            return board.CanBePlaced(x, y);
        }



        // Form1_MouseDown時用PlaceAPiece回傳是放什麼顏色的棋子座標給電腦，失敗回傳null
        //                        (回傳值為Piece物件)
        // Piece因為是物件所以是reference type 
        public Piece PlaceAPiece(int x, int y)
        {
            Piece piece = board.PlaceAPiece(x, y, currentPlayer);
            if (piece != null)
            {
                // 在交換選手之前就要先判斷遊戲是否結束(找到贏家)
                checkWinner();

                // 交換選手(顏色)
                if (currentPlayer == PieceType.BLACK)
                    currentPlayer = PieceType.WHITE;
                else if (currentPlayer == PieceType.WHITE)
                    currentPlayer = PieceType.BLACK;

                return piece;
            }
            return null;
        }


        // Form1_MouseDown時，PlaceAPiece() 裡面會判斷，這邊獨立寫出來怎麼贏的函式
        private void checkWinner()
        {
            // 最後一顆棋子為中心點座標，判斷八個方向是否有連續4顆相同顏色的棋子(影片6:11)
            int centerX = board.LastPlaceNode.X;
            int centerY = board.LastPlaceNode.Y;

            // ToDo：
            // 大方向就是2個判斷
            // 判斷1 紀錄現在看到幾顆連續相同的棋子              (檢查顏色是否相同)
            // 判斷2 如果某個方向有連續4顆，代表該顏色的玩家贏了 (檢查是否看到5顆棋子)

            // 用迴圈檢查8個不同方向(原本9個扣掉中心1個，所以8個)
            for(int xDir = -1; xDir <= 1; xDir++)
            {
                for(int yDir = -1; yDir <=1; yDir++)
                {
                    // 先排除中心的那一個
                    if (xDir == 0 && yDir == 0)
                        continue;

                    // 判斷1 紀錄現在看到幾顆連續相同的棋子 (檢查顏色是否相同)
                    // 預設為1顆(LastPlaceNode的那顆開始)
                    int count = 1;
                    while (count < 5)
                    {
                        // 先以一維陣列為例，target為中心點向右的第1、2、3、4顆
                        int targetX = centerX + count * xDir;
                        int targetY = centerY + count * yDir;
                        // 判斷2個
                        // 1.為了避免靠近邊界的旗子向邊界外去判斷顏色是否相同，所以要限制判斷的邊界
                        // 2.中心向右的那些棋子是否跟"中心那顆(currentPlayer)"顏色相同(影片12:15)
                        if (targetX < 0 || targetX >= Board.NODE_COUNT || //9 = NODE_COUNT 
                            targetY < 0 || targetY >= Board.NODE_COUNT ||
                            board.GetPieceType(targetX, targetY) != currentPlayer)
                            break;  // 接續交換選手的函式

                        count++;

                    }

                    // 判斷2 如果某個方向有連續4顆，代表該顏色的玩家贏了(檢查是否看到5顆棋子)
                    if (count == 5)
                    {
                        winner = currentPlayer;
                    }

                }
            }

        }

    }
}
