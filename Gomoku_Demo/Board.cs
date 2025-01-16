using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; // 給public "Point" findTheClosetNode()用 (座標)

namespace Gomoku_Demo
{
    internal class Board // 棋盤的程式碼都寫在這裡
    {
        // 侷限那9個交叉點
        public static readonly int NODE_COUNT = 9;
        // 沒有符合的點(不存在的點，-1,-1很適合)
        private static readonly Point NO_MATCH_NODE = new Point(-1, -1);
        // 棋盤邊界距離
        private static readonly int OFFSET = 75;
        // 滑鼠偵測點的半徑
        private static readonly int NODE_RADIUS = 10;
        // 交叉點跟交叉點之間的距離
        private static readonly int NODE_DISTRANCE = 75;
        // 圖片剛好就是9*9的二維陣列
        private Piece[,] pieces = new Piece[NODE_COUNT, NODE_COUNT];
        // 最後一顆棋子的座標(預設是NO_MATCH_NODE)
        private Point lastPlaceNode = NO_MATCH_NODE;
        // 利用get存取器，回傳lastPlaceNode座標位置
        public Point LastPlaceNode { get {return lastPlaceNode; } }

        // --------------------------------------------------------

        // 目的:給棋盤任何一個座標，回傳那個座標放什麼顏色的棋子
        public PieceType GetPieceType(int nodeIdX, int nodeIdY)
        {
            if (pieces[nodeIdX, nodeIdY] == null)
                return PieceType.NONE;
            else
                return pieces[nodeIdX, nodeIdY].GetPieceType();
        }

        //------1
        // 用CanBePlaced判斷MouseMove時，
        // "顯示"是否可以放棋子(回傳值為bool "T" or "F" )
        // 回傳T為手指圖案；F為箭頭圖案
        public bool CanBePlaced(int x, int y)
        {
            // ToDo:找出滑鼠游標最近的節點(Node)給nodeId
            // 回傳值引用(回傳一個十字節點座標 or NO_MATCH_NOD 給 nodeId)
            Point nodeId = findTheClosetNode(x, y);
            // Point型態後面都可以接.X、.Y

            // ToDo:如果回傳NO_MATCH_NOD(不是十字節點)的話，
            // 回傳false(游標顯示箭頭)
            if (nodeId == NO_MATCH_NODE)
                return false;

            // ToDo:如果不是NO_MATCH_NODE的話，檢查棋子是否已經存在
            // ToDo:如果節點有棋子(!= null)，依舊或馬上顯示預設箭頭圖案
            if (pieces[nodeId.X, nodeId.Y] != null)
                return false;

            // ToDo:如果節點沒有棋子就顯示手指圖案
            return true;
        }

        //------2
        // 目的:讓交叉節點以外的地方不能下棋子
        // 回傳要馬是"物件"，要馬是"null"
        //                                      ↓限制使用者回傳2種type(BLACK、WHITE)
        public Piece PlaceAPiece(int x,  int y , PieceType type)
        {
            // 找出滑鼠游標最近的節點(Node)
            // 回傳值引用(回傳一個十字節點座標 or NO_MATCH_NOD 給 nodeId)
            Point nodeId = findTheClosetNode(x, y);


            // 如果回傳NO_MATCH_NOD(不是十字節點)的話，回傳null
            // null代表沒有指向任何的物件(可以return空的東西，不執行任何事)
            if (nodeId == NO_MATCH_NODE)
                return null;

            // ToDo:如果有十字節點的話，檢查該十字節點座標是否有棋子存在
            // 有棋子代表 != null，所以回傳null
            // 十字節點座標給pieces當(x,y)的位置去判斷
            if (pieces[nodeId.X, nodeId.Y] != null)
                return null;

            // 沒有棋子的話，即可執行以下
            // 先用convertToFormPosition置中nodeId的位置座標
            Point formPos = convertToFormPosition(nodeId);

            // 根據 type 產生對應的棋子
            // 將formPos指向回傳formPosition的資訊(座標)            
            // 這裡的 type 是指 PieceType "type"(BLACK、WHITE)
            if (type == PieceType.BLACK) //用 "PieceType."讓電腦知道這是enum裡面的東西
                pieces[nodeId.X, nodeId.Y] = new BlackPiece(formPos.X, formPos.Y);
            else if (type == PieceType.WHITE)
                pieces[nodeId.X, nodeId.Y] = new WhitePiece(formPos.X, formPos.Y);

            // 紀錄最後下棋子的位置
            lastPlaceNode = nodeId;

            return pieces[nodeId.X, nodeId.Y];
        }


        //------3
        // Todo:讓棋子擺放不要歪七扭八(棋子置中)
        // 把「棋盤座標的(x,y)」轉換成「視窗座標(x,y)」
        // 因為算出棋盤座標的(x,y)，所以推回去較簡單
        private Point convertToFormPosition(Point nodeId)
        {
            Point formPosition = new Point();
            //  棋盤座標賦予給名叫formPosition的視窗座標
            //  視窗座標   =  棋盤座標
            formPosition.X = nodeId.X * NODE_DISTRANCE + OFFSET; // nodeId.X(原nodeIdX回傳商的整數)
            formPosition.Y = nodeId.Y * NODE_DISTRANCE + OFFSET;
            return formPosition;
        }


        //------4+5 "把「視窗座標(x,y)」轉換成「棋盤座標的(x,y)」"
        //------4
        // 找到交叉點，回傳點的位置(座標)(二維程式碼)
        // ToDo:如果可以的話會出現手指的圖案
        // ToDo:如果不行的話依舊箭頭
        private Point findTheClosetNode(int x, int y)
        {
            int nodeIdX = findTheClosetNode(x);    //9
            if (nodeIdX == -1 || nodeIdX >= NODE_COUNT)
                return NO_MATCH_NODE;

            int nodeIdY = findTheClosetNode(y);    //9
            if (nodeIdY == -1 || nodeIdX >= NODE_COUNT)
                return NO_MATCH_NODE;

            return new Point(nodeIdX, nodeIdY);
        }

        //------5
        // 回傳節點距離(一維程式碼)
        private int findTheClosetNode(int pos) //放入X or Y
        {
            // 這是我自己想的~
            // 假如游標座標像素小於邊界的寬度(75)，
            // 那就先回傳-1排除它的可能性了! (通常都用-1形容錯誤)
            if (pos < OFFSET)
                return -1;

            // 先把邊界長度去掉再存進pos(去掉邊界)
            pos -= OFFSET;
            // 取商(quotient) (介於誰跟誰之間)
            int quotient = pos / NODE_DISTRANCE;
            // 取餘數(remainder)
            int remainder = pos % NODE_DISTRANCE;
            // 左右邊判斷
            if (remainder <= NODE_RADIUS)
                return quotient;        //左邊那顆
            else if (remainder >= NODE_DISTRANCE - NODE_RADIUS)
                return quotient + 1;    //右邊那顆
            else
                return -1; // 代表沒有任何一邊符合
        }

    }
}
