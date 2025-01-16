using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gomoku_Demo
{
    public partial class Form1 : Form // Form = 視窗(與使用者互動的部分)
    {
        private Game game = new Game();

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {                                           //用像素去偵測
            if (game.CanBePlaced(e.X, e.Y)) //回傳布林值
            {
                this.Cursor = Cursors.Hand;    //手指圖案(ture 該座標可下棋)
            }
            else
            {
                this.Cursor = Cursors.Default; //預設(false 該座標不能下棋)
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Piece piece = game.PlaceAPiece(e.X, e.Y);
            if (piece != null)
            {
                this.Controls.Add(piece); // 放一張圖片的程式碼 

                // 檢查是否有人獲勝
                if(game.Winner == PieceType.BLACK)
                {
                    MessageBox.Show("黑棋獲勝");
                } else if (game.Winner == PieceType.WHITE)
                {
                    MessageBox.Show("白棋獲勝");
                }


            }
        }

        /*
        除錯
        1.private 寫成 public 
        2.兩個判斷裡面參數都寫(x)，應該要一個(x)一個(y)
        3.FindTheClosetNode 寫成 FindTheCloseNode
        4.    int remainder = pos % NODE_DISTRANCE;
            寫成int remainder = pos % NODE_RADIUS;
        耗時1小時成功除錯，最後是靠Claude幫助 感謝

        倒數第2及超男一直傳來傳去，宣告新的變數跟函式，頭昏眼花

        {
        最後一集超硬，短短半小時的影片
        寫3個多小時還沒寫完

        Board.NODE_COUNT 不懂??? (影片17:00)

        迴圈開始(影片27:30)不懂???

        這集還要多複習幾次
        }

        小山沒教的bug尚未修復(以下為網友成功的程式碼)
        https://github.com/oscarsun72/c-sharp-course-sample-code/tree/master/class-40-50/Class46Ex_1
        https://github.com/oscarsun72/c-sharp-course-sample-code/tree/master/class-40-50/Class46Ex_2
        */
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

