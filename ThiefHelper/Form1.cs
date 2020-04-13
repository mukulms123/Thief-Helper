using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThiefHelper
{

    public class KnapsackDP
    {
        int knapsack_weight;
        int numberOfItems, totalValue;
        int[] weight;
        int[] value;
        int[,] memoTable;
 
        public KnapsackDP(int knapsack_weight1, int N, int[] weight1, int[] value1)
        {
            this.numberOfItems = N;
            this.totalValue = 0;
            this.knapsack_weight = knapsack_weight1;
            this.weight = weight1;
            this.value = value1;
            this.memoTable = new int[numberOfItems + 1,knapsack_weight1+1];
        }

        public int solve()
        {
            int i, j;

            for (i = 1; i < (numberOfItems + 1); i++)
            {

                for (j = 1; j < (knapsack_weight + 1); j++)
                {

                    int not_taking_item = memoTable[i - 1,j];
                    int taking_item = 0;

                    if (weight[i-1] <= knapsack_weight)
                    {
                        if (j - weight[i-1] < 0)
                            taking_item = memoTable[i - 1,j];
                        else
                            taking_item = value[i-1] + memoTable[i - 1,j - weight[i-1]];
                    }
                    // choosing which is larger
                    int which_to_take =Math.Max(not_taking_item, taking_item);

                    // storing in the memo table
                    memoTable[i,j] = which_to_take;
                }
            }

            totalValue = memoTable[numberOfItems,knapsack_weight];
            return totalValue;
        }

        public String selected_items()
        {

            /*
            //Uncomment to print knapsack table
            for(int i=0; i<=numberOfItems; i++){
                for(int j=0; j<=knapsack_weight; j++){
                    System.out.print(memoTable[i][j]+ "\t");
                }
                System.out.println();
            }

            */
            String test = "";
            //Checking which item is taken
            for (int i = numberOfItems, j = knapsack_weight; i > 0; i--)
            {
                if (memoTable[i,j] != memoTable[i - 1,j])
                {
                    test=test+("Item: " + i + " Selected.\n");
                    j = j - weight[i-1];
                    //j represent weight column, since item i is taken
                    //So subtraction the weight of ith item to reach
                    //next weight column
                }
            }
            
            return test;
        }
    }


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {   if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Please Fill all the values!!...");

            }
            else
            {
                int n = Convert.ToInt32(textBox2.Text);
                int W = Convert.ToInt32(textBox1.Text);
                String text3 = textBox3.Text;
                String[] wght = text3.Split(',');
                int l1 = wght.Length;
                int i, j = 0;
                int[] weight = new int[n];
                string test = "";
                int Tweight = 0;
                for (i = 0; i < l1; i++)
                {
                    if (wght[i] != " " && j < n)
                    {
                        weight[j] = Convert.ToInt32(wght[i]);
                        Tweight = Tweight + weight[j];
                        j = j + 1;
                        
                    }
                }
                String text4 = textBox4.Text;
                String[] prft = text4.Split(',');
                int l2 = prft.Length;
                int[] profit = new int[n];
                j = 0;
                for (i = 0; i < l2; i++)
                {
                    if (prft[i] != " " && j < n)
                    {
                        profit[j] = Convert.ToInt32(prft[i]);

                        j = j + 1;

                    }
                }
     
                if (!radioButton1.Checked && !radioButton2.Checked)
                {
                    MessageBox.Show("Please select one type of Filling.");
                } 
                else if (radioButton1.Checked)
                {
                    int cur_w;
                    float tot_v;
                    int maxi;
                    int[] used=new int[10];
                    if (Tweight < W)
                    {
                        MessageBox.Show("You can add all items to your Bag. Enjoy" + Tweight);
                    }
                    else
                    {
                        for (i = 0; i < n; ++i)
                            used[i] = 0; /* I have not used the ith object yet */

                        cur_w = W;
                        tot_v = 0;
                        while (cur_w > 0)
                        { /* while there's still room*/
                            /* Find the best object */
                            maxi = -1;
                            for (i = 0; i < n; ++i)
                            {
                                if ((used[i] == 0) && ((maxi == -1) || ((float)profit[i] / weight[i] > (float)profit[maxi] / weight[maxi])))
                                {
                                    maxi = i;
                                }
                            }

                            used[maxi] = 1; /* mark the maxi-th object as used */
                            cur_w -= weight[maxi]; /* with the object in the bag, I can carry less */
                            tot_v += profit[maxi];
                            if (cur_w >= 0)
                                test = test + ("\nAdd object " + (maxi + 1) + " (" + profit[maxi] + "$, " + weight[maxi] + "Kg) completely in the bag.");
                            else
                            {
                                int ratio = (int)((1 + (float)cur_w / weight[maxi]) * 100);
                                test=test+("\nAdd "+ratio+"% ("+ profit[maxi]+"$, "+ weight[maxi] + "Kg) of object "+(maxi + 1)+" in the bag.\n");
                                tot_v -= profit[maxi];
                                tot_v += (1 + (float)cur_w / weight[maxi]) * profit[maxi];
                            }
                        }
                        test = test + ("\nFilled the bag with objects worth " + tot_v + "$.\n");
                        MessageBox.Show(test);
                        
                    }
                }
                else if (radioButton2.Checked)
                {
                    KnapsackDP kdp = new KnapsackDP(W, n, weight, profit);
                    int prf= kdp.solve();
                    test= kdp.selected_items();
                    MessageBox.Show(test+"With Profit: "+prf);

                }
            }

        }
    }
}
