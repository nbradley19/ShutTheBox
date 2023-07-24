using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace ShutTheBox
{
    public partial class Form1 : Form
    {
        private const string INSTRUCTIONS = "The goal of this game is to get all of the tiles down. After rolling the dice, you select any number of tiles that adds up to that number." +
            "If the only available tiles left do not add up to your rolled number, you lose. If you get all of the tiles down, you win. Your score will be the sum of the remaining tiles." +
            "Start by pressing Roll.";
        private int tileTotal = 45;
        private int rollTotal = 0;
        private int selectedTotal = 0;
        private Tile[] tileArr;
        private ArrayList unusedTiles;
        private StreamWriter sw;
        private Random ran;
        private Die die1;
        private Die die2;
        private bool restarting;
        private bool rolled;
        private bool allTilesLargerThan6;
        public Form1()
        {
            /*try
            {
                sw = new StreamWriter("scores.txt");
            }
            catch(Exception e)
            {
                Console.WriteLine("file borked " + e.Message);
            }
            finally
            {
                Console.WriteLine("file not borked");
            }*/
            InitializeComponent();
            play();
        }
        private void play()
        {
            ran = new Random();
            die1 = new Die(ran);
            die2 = new Die(ran);
            restarting = false;
            rolled = false;
            allTilesLargerThan6 = true;
            tileTotal = 45;
            selectedTotal = 0;
            textBoxInstructions.Text += System.Environment.NewLine + INSTRUCTIONS;
            tileArr = new Tile[9];
            for(int i = 0; i < tileArr.Length; i++)
            {
                tileArr[i] = new Tile(i + 1);
            }
            foreach (Tile t in tileArr)
            {
                t.IsUp = true;
            }
            pictureBoxTile1.Image = tileArr[0].CurrentImage;
            pictureBoxTile2.Image = tileArr[1].CurrentImage;
            pictureBoxTile3.Image = tileArr[2].CurrentImage;
            pictureBoxTile4.Image = tileArr[3].CurrentImage;
            pictureBoxTile5.Image = tileArr[4].CurrentImage;
            pictureBoxTile6.Image = tileArr[5].CurrentImage;
            pictureBoxTile7.Image = tileArr[6].CurrentImage;
            pictureBoxTile8.Image = tileArr[7].CurrentImage;
            pictureBoxTile9.Image = tileArr[8].CurrentImage;
            unusedTiles = new ArrayList(tileArr);
            writeScores();
            
        }
        /* This method changes the picture that the pictureboxes hold to make 
         * the tiles have the appearance of being "down", but only if they are already in the up state.
         * 
         */
        private Bitmap onClick(Tile tile)
        {
            if (rolled)
            {
                if (tile.IsUp)
                {
                    if (tile.Value + selectedTotal <= rollTotal)
                    {
                        selectedTotal += tile.Value;
                        Console.WriteLine("Selected Total: " + selectedTotal);
                        tile.toggleIsUp();
                        unusedTiles.Remove(tile);
                    }
                    else
                    {
                        textBoxInstructions.Text += System.Environment.NewLine + "Selection too large";
                    }

                }
                else
                {
                    //selectedTotal -= tile.Value;
                    textBoxInstructions.Text += System.Environment.NewLine + "Tile already down";
                }
                return tile.CurrentImage;
            }
            else
            {
                textBoxInstructions.Text += System.Environment.NewLine + "Please Roll";
                return tile.CurrentImage;
            }
        }
        private void pictureBoxTile1_Click(object sender, EventArgs e)
        {
            pictureBoxTile1.Image = onClick(tileArr[0]);
        }
        private void pictureBoxTile2_Click(object sender, EventArgs e)
        {
            pictureBoxTile2.Image = onClick(tileArr[1]);
        }
        private void pictureBoxTile3_Click(object sender, EventArgs e)
        {
            pictureBoxTile3.Image = onClick(tileArr[2]);
        }
        private void pictureBoxTile4_Click(object sender, EventArgs e)
        {
            pictureBoxTile4.Image = onClick(tileArr[3]);
        }
        private void pictureBoxTile5_Click(object sender, EventArgs e)
        {
            pictureBoxTile5.Image = onClick(tileArr[4]);
        }
        private void pictureBoxTile6_Click(object sender, EventArgs e)
        {
            pictureBoxTile6.Image = onClick(tileArr[5]);
        }
        private void pictureBoxTile7_Click(object sender, EventArgs e)
        {
            pictureBoxTile7.Image = onClick(tileArr[6]);
        }
        private void pictureBoxTile8_Click(object sender, EventArgs e)
        {
            pictureBoxTile8.Image = onClick(tileArr[7]);
        }
        private void pictureBoxTile9_Click(object sender, EventArgs e)
        {
            pictureBoxTile9.Image = onClick(tileArr[8]);
        }

        private void pictureBoxBox_Click(object sender, EventArgs e)
        {

        }
        private void textBoxInstructions_TextChanged(object sender, EventArgs e)
        {

        }
        private void pictureBoxDice1_Click(object sender, EventArgs e)
        {

        }
        private void pictureBoxDice2_Click(object sender, EventArgs e)
        {

        }
        /*this method makes sure that there is a possible choice of tiles that will add up
         * to the rolled total. If not, the game ends.
         */
        private void buttonRoll_Click(object sender, EventArgs e)
        {
            if (rolled == false)
            {
                if (allTilesLargerThan6)
                {
                    foreach (Tile t in unusedTiles)
                    {
                        if (t.Value > 6)
                        {
                            allTilesLargerThan6 = true;
                            break;
                        }
                        else
                        {
                            allTilesLargerThan6 = false;
                        }
                    }
                }
                rolled = true;
                bool goodRoll = false;
                if (allTilesLargerThan6 == true)
                {
                    pictureBoxDice1.Image = die1.roll();
                    pictureBoxDice2.Image = die2.roll();
                    rollTotal = die1.Value + die2.Value;
                }
                else
                {
                    textBoxInstructions.Text += System.Environment.NewLine + "All remaining tiles are 6 or less! One dice used.";
                    pictureBoxDice1.Image = die1.roll();
                    pictureBoxDice2.Image = die2.NullImg;
                    rollTotal = die1.Value;
                }

                foreach (Tile tile1 in unusedTiles)
                {
                    
                    if (tile1.Value == rollTotal)
                    {
                        goodRoll = true;
                    }
                    else if (tile1.Value < rollTotal)
                    {
                        foreach (Tile tile2 in unusedTiles)
                        {
                            if (tile2.Value != tile1.Value)
                            {
                                if (tile1.Value + tile2.Value == rollTotal)
                                {
                                    goodRoll = true;
                                }
                                else if (tile1.Value + tile2.Value < rollTotal)
                                {
                                    foreach (Tile tile3 in unusedTiles)
                                    {
                                        if (tile3.Value != tile2.Value && tile3.Value != tile1.Value)
                                        {
                                            if (tile1.Value + tile2.Value + tile3.Value == rollTotal)
                                            {
                                                goodRoll = true;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }

                }
                if (!goodRoll)
                {
                    endgame(false);
                }
                Console.WriteLine("Roll Total: " + rollTotal);
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            rolled = false;
            if (selectedTotal < tileTotal)
            {
                tileTotal -= selectedTotal;
                selectedTotal = 0;
            }
            else if (selectedTotal == tileTotal)
            {
                endgame(true);
            }
            else
            {
                endgame(false);
            }
            if (restarting)
            {
                restarting = false;
                play();
            }
        }
            
        
            /*Endgame ends the game by displaying the final score,
             *writing it to file, and resetting it to play again.
             */
        private void endgame(bool endCase)
        {
            if (endCase)
            {
                textBoxInstructions.Text += System.Environment.NewLine + "You Win!";
            }
            else
            {
                textBoxInstructions.Text += System.Environment.NewLine + "You Lose!" + System.Environment.NewLine + "Score: " + tileTotal;
            }
            writeScores("" + tileTotal);
            textBoxInstructions.Text += System.Environment.NewLine + "Press submit to start again!";
            restarting = true;
        }
        private void writeScores()
        {
            textBoxScores.Text = "";
            ArrayList previousScores = new ArrayList();
            using (StreamReader sr = new StreamReader("scores.txt"))
            {
                string temp = "";
                while ((temp = sr.ReadLine()) != null)
                {
                    previousScores.Add(temp);
                }
                sr.Close();
            }
            using (sw = new StreamWriter("scores.txt"))
            {
                textBoxScores.Text += System.Environment.NewLine + "Previous scores: ";
                foreach (string q in previousScores)
                {
                    textBoxScores.Text += System.Environment.NewLine + q;
                    sw.WriteLine(q);
                }
                sw.Close();
            }
        }
        private void writeScores(string s)
        {
            textBoxScores.Text = "";
            ArrayList previousScores = new ArrayList();
            using (StreamReader sr = new StreamReader("scores.txt"))
            {
                string temp = "";
                while ((temp = sr.ReadLine()) != null)
                {
                    previousScores.Add(temp);
                }
                sr.Close();
            }
            using (sw = new StreamWriter("scores.txt"))
            {
                previousScores.Add("Score: " + s);
                textBoxScores.Text += System.Environment.NewLine + "Previous scores: ";
                foreach (string q in previousScores)
                {
                    textBoxScores.Text += System.Environment.NewLine + q;
                    sw.WriteLine(q);
                }
                sw.Close();
            }
        }

        private void textBoxScores_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class Tile
    {
        private System.Resources.ResourceManager rm = ShutTheBox.Properties.Resources.ResourceManager;
        private int val;
        private Bitmap upImage, downImage;
        private bool isUp = true;
        public Bitmap UpImage
        {
            get { return upImage; }
        }
        public Bitmap CurrentImage
        {
            get
            {
                if (isUp)
                {
                    return upImage;
                }
                else
                {
                    return downImage;
                }
            }
        }
        public bool Equals(Tile t)
        {
            if(this.Value == t.Value)
            {
                return true;
            }
            return false;
        }
        public Bitmap DownImage
        {
            get { return downImage; }
        }
        public bool IsUp
        {
            get { return isUp; }
            set { this.isUp = value;  }
        }
        public void toggleIsUp()
        {
            isUp = !isUp;
        }
        public int Value
        {
            get { return val; }
        }
        //isUp is for position of tile, true if up, false if down
        
        public Tile(int val)
        {
            this.val = val;
            upImage = (Bitmap)rm.GetObject("uptile" + val);
            downImage = (Bitmap)rm.GetObject("downtile" + val);
        }
        
        
    }
    public class Die
    {
        private System.Resources.ResourceManager rm = ShutTheBox.Properties.Resources.ResourceManager;
        private int val;
        private Bitmap nullImg;
        Random ran;
        public int Value
        {
            get { return val;  }
        }
        public Die(Random ran)
        {
            nullImg = (Bitmap)rm.GetObject("nullDie");
            this.ran = ran;
        }
        public Bitmap NullImg
        {
            get { return nullImg; }
        }
        public Bitmap roll()
        {
            val = ran.Next(1, 7);
            return (Bitmap)rm.GetObject("dice" + val);
        }
    }

}
