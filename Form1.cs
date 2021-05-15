using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;

//namespace rgb_app{
    public partial class Form1 : Form    {
        
        private System.Windows.Forms.NotifyIcon iconoNot;
        //private System.Windows.Forms.ContextMenu contextMenu1;
        //private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem itemAnimacion;
        private System.Windows.Forms.ToolStripMenuItem itemVelocidad;
        private System.Windows.Forms.ToolStripMenuItem itemColor;
        private System.Windows.Forms.ToolStripMenuItem itemBrillo;
        private System.Windows.Forms.ToolStripMenuItem itemSalir;
        private SerialPort  sp;
        private ColorDialog dlg;
        private String ruta;
        public Form1()        {
            InitializeComponent();    
            sp = new SerialPort ();  

            dlg = new ColorDialog();      
            
            iconoNot = new System.Windows.Forms.NotifyIcon();
            iconoNot.Visible = true;
            iconoNot.Icon = new Icon("rgb_icon.ico");

            contextMenu = new System.Windows.Forms.ContextMenuStrip();
            itemAnimacion = new System.Windows.Forms.ToolStripMenuItem();
            itemVelocidad = new System.Windows.Forms.ToolStripMenuItem();
            itemColor = new System.Windows.Forms.ToolStripMenuItem();
            itemBrillo = new System.Windows.Forms.ToolStripMenuItem();
            itemSalir = new System.Windows.Forms.ToolStripMenuItem();
            //contextMenu.SuspendLayout();
            ruta ="bd.txt";

            this.itemSalir.Click += new EventHandler(this.salirClick);
            this.itemSalir.Text = "Salir";

            this.contextMenu.Items.AddRange(new ToolStripItem[] {
                this.itemAnimacion,this.itemVelocidad,this.itemColor, this.itemBrillo,this.itemSalir});
            this.contextMenu.Name = "Menu";

            ////Animacion
            this.itemAnimacion.Text = "Animación";
            //this.itemAnimacion.Click += new EventHandler(this.animacionClick);

            String[] anims ={
                "Rainbow","FadeInOut","CylonBounce","TwinkleRandom","Sparkle","SnowSparkle",
                "RunningLights","ColorWipe","ColorWipeInv","MeteorRain","Circunferencia",
                "RandomColorFill","Circunferencia2","ColorWipeUni","ColorWipeUniInv","RandomColorFillUni","SolidColor"
            };
            int i=1;
            foreach (String anim in anims){
                System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem(anim);
                item.Click += new EventHandler(this.animacionClick);
                item.Tag = i;
                this.itemAnimacion.DropDownItems.Add(item);
                i++;
            }

            ///Velocidad
            this.itemVelocidad.Text = "Velocidad";
            //this.itemVelocidad.Click += new EventHandler(this.velocidadClick);

            String[] vels ={
                "Muy Rápido","Rapido","Normal","Lento","Muy Lento"
            };

            
            i=0;
            foreach (String vel in vels){
                System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem(vel);
                item.Click += new EventHandler(this.velocidadClick);
                item.Tag = i;
                this.itemVelocidad.DropDownItems.Add(item);
                i++;
            }

            ///Color
            this.itemColor.Text = "Color";

            String[] cols ={
                "Blanco","Rojo","Azul","Verde","Morado","Amarillo","Naranja","Rosado","Cafe","Custom"
            };
            i=0;
            foreach (String col in cols){
                System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem(col);
                item.Click += new EventHandler(this.colorClick);
                item.Tag = i;
                this.itemColor.DropDownItems.Add(item);
                i++;
            }

            ///Brillo
            this.itemBrillo.Text = "Brillo";

            for (int j=0;j<=10;j++){
                System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem(""+j);
                if(j==0) item.Text = "Off";
                item.Click += new EventHandler(this.brilloClick);
                item.Tag = j;
                this.itemBrillo.DropDownItems.Add(item);
                i++;
            }
            
            
            iconoNot.ContextMenuStrip = contextMenu;

            conectSerial();
            
        }

        private void saveCom(String com){/*
            StreamWriter file = new StreamWriter(ruta, false);
            file.WriteLine(Convert.ToString(com));
            file.Flush();
            file.Close();*/
        }

        private void conectSerial(){
            String com ="COM0";
            try{
                StreamReader sr = File.OpenText(ruta);
                com = sr.ReadLine();
                sr.Close();
            }catch(System.Exception ex ){
                MessageBox.Show("ERROR txt: "+ex.Message, "Form Closing",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
            }

            try{
                sp.BaudRate=4800;
                sp.PortName = com; 
                sp.Open();
            }catch (System.Exception ex){
                MessageBox.Show("ERROR SERIAL: "+ex.Message, "Form Closing",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
            }
        }
        
        private void animacionClick(object sender, EventArgs e){
            System.Windows.Forms.ToolStripMenuItem item = sender as System.Windows.Forms.ToolStripMenuItem;
            try{                
                sp.Write("a."+item.Tag);
            }catch (System.Exception ex){
                Console.Write("ERROR SEIAL: "+ex.Message);
                MessageBox.Show("ERROR SEIAL: "+ex.Message, "Form Closing",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
            }
            
        }

        private void velocidadClick(object sender, EventArgs e){
            System.Windows.Forms.ToolStripMenuItem item = sender as System.Windows.Forms.ToolStripMenuItem;
            int ret = 2*int.Parse(item.Tag.ToString())+1;
            try{                
                sp.Write("r."+ret);
            }catch (System.Exception ex){
                Console.Write("ERROR SEIAL: "+ex.Message);
                MessageBox.Show("ERROR SEIAL: "+ex.Message, "Form Closing",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
            }

        }

        private Color[] colors ={
                Color.White,Color.Red, Color.Blue,Color.DarkGreen,Color.Purple,Color.Yellow,Color.Orange,Color.Pink,Color.Brown
            };
        private void colorClick(object sender, EventArgs e){
            System.Windows.Forms.ToolStripMenuItem item = sender as System.Windows.Forms.ToolStripMenuItem;
            int i = (int)int.Parse(item.Tag.ToString());
            
            if (i==colors.Length){
                ColorDialog dlg = new ColorDialog();
                dlg.Color=Color.Blue;
                if(dlg.ShowDialog() == DialogResult.OK) {
                    Color c  = dlg.Color;
                    try{                
                        sp.Write("c."+c.R+"."+c.G+"."+c.B);
                    }catch (System.Exception ex){
                        Console.Write("ERROR SEIAL: "+ex.Message);
                        MessageBox.Show("ERROR SEIAL: "+ex.Message, "Form Closing",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question);
                    }
                    return;
                }
            }
            //MessageBox.Show ("R: "+colors[i].R+". G: "+colors[i].G+". B: "+colors[i].B);
            try{                
                sp.Write("c."+colors[i].R+"."+colors[i].G+"."+colors[i].B);
            }catch (System.Exception ex){
                Console.Write("ERROR SEIAL: "+ex.Message);
                MessageBox.Show("ERROR SEIAL: "+ex.Message, "Form Closing",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
            }
        }

        private void brilloClick(object sender, EventArgs e){
            System.Windows.Forms.ToolStripMenuItem item = sender as System.Windows.Forms.ToolStripMenuItem;
            int brillo = (int)25.5*int.Parse(item.Tag.ToString());
            try{                
                sp.Write("b."+brillo);
            }catch (System.Exception ex){
                Console.Write("ERROR SEIAL: "+ex.Message);
                MessageBox.Show("ERROR SEIAL: "+ex.Message, "Form Closing",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
            }
        }

        private void salirClick(object sender, EventArgs e){
            if(sp.IsOpen)
                sp.Close();
            iconoNot.Visible = false;
            Application.Exit();
        }

        protected override void OnVisibleChanged(EventArgs e){
            base.OnVisibleChanged(e);
            this.Visible = false;
        }
          

    }
//}
