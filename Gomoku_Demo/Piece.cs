using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;    // 給PictureBox用的
using System.Drawing;          // 給PictureBox的color用

namespace Gomoku_Demo
{
    // 有關棋子的程式碼都寫在這裡
    // abstract 防止被new出來
    abstract class Piece : PictureBox
    {
        private static readonly int IMAGE_WIDTH = 50; 
                //確保他是常數      // 圖片邊長為50

        // 建構子
        // 圖片預設的樣子
        public Piece(int x, int y)
        {
            //背景顯示為透明
            this.BackColor = Color.Transparent;
            //放置位子剛好在滑鼠的尖端
            this.Location = new Point(x - IMAGE_WIDTH/2, y - IMAGE_WIDTH/2);
            //圖片大小
            this.Size = new Size(50, 50);
        }

        public abstract PieceType GetPieceType();

    }
}
