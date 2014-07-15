using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace LPI2_Presenças
{
    public partial class Form1 : Form
    {
        string RxString, ComPort, Baudrate; 
        string ReFresh = "0";
        int x = 0;
        
        public Form1()
        {
            InitializeComponent();  //Inicializar interface
            Control.CheckForIllegalCrossThreadCalls = false;    //Permitar alterar características da interface
            this.textBox2.AppendText("Welcome!" + System.Environment.NewLine); //Escre na caixa de texto
        }

        private void button1_Click(object sender, EventArgs e) //Acção do botão Start
        {
            if (ComPort != null && Baudrate != null) //Não Inicia se a Porta e o Baudrate não estiveram definidos
            {
                serialPort1.PortName = ComPort; //Define porta
                serialPort1.BaudRate = System.Convert.ToInt32(Baudrate); // Converte para inteiro e define baudrate 
                serialPort1.Open(); //Inicia porta serie
                if (serialPort1.IsOpen)
                {
                    button1.Enabled = false; //Desactiva botão start
                    button2.Enabled = true; //Activa botão stop
                    serialPort1.WriteLine(ReFresh); //Envia caracter via porta serie para actualização do numero de pessoas
                }                
            }
            else
            {
                if (ComPort == null && Baudrate == null)    // Se não foram definidos a porta e o baudrate mostra mensagem de erro 
                    MessageBox.Show("Choose a COM Port and Baud Rate");
                else if (ComPort == null)
                    MessageBox.Show("Choose a COM Port");   // Se não foi definida a porta mostra mensagem de erro
                else if (Baudrate == null)
                    MessageBox.Show("Choose Baud Rate");    // Se não foi definido o baudrate mostra mensagem de erro
            }
        }
        
        private void button2_Click(object sender, EventArgs e)  //Acção do botão Stop
        {
            if (serialPort1.IsOpen) //Se porta série está activa
            {
                serialPort1.Close(); // Desactiva porta série
                button1.Enabled = true; //Activa botão start
                button2.Enabled = false; //Desactiva botão stop
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e) // Acção porta série
        {
            x = serialPort1.ReadByte(); //Lê linha
            //x = System.Convert.ToInt32(x); //Converte para inteiro 
            
            //Apresenta na caixa de texto a hora em que ocorreu um movimento e o número actual de pessoas na divisão
            if (x == 0)//Se numero de pessoas for 0
            {
                this.textBox2.AppendText(DateTime.Now.ToString("HH:mm:ss tt") + " ---> " + "No One Inside" + System.Environment.NewLine);
                this.textBox1.BackColor = System.Drawing.Color.Red;     //Fundo da caixa de texto fica vermelho
                this.textBox1.Text = System.Convert.ToString(x);    // Apresenta numero 0
            }
            else //Senão
            {
                if (x == 1)
                {
                    this.textBox2.AppendText(DateTime.Now.ToString("HH:mm:ss tt") + " ---> " + System.Convert.ToString(x) + " Person Inside " + System.Environment.NewLine);
                }
                else
                {
                    this.textBox2.AppendText(DateTime.Now.ToString("HH:mm:ss tt") + " ---> " + System.Convert.ToString(x) + " People Inside " + System.Environment.NewLine);
                }
                this.textBox1.BackColor = System.Drawing.Color.Green;   //Fundo da caixa de texto fica verde   
                this.textBox1.Text = System.Convert.ToString(x);   //Apresenta numero de pessoas
            }
        }
        
        private void comboBox1_Click(object sender, EventArgs e) //Acçao da caixa de selecção da porta
        {
            comboBox1.Items.Clear();
            foreach (string s in SerialPort.GetPortNames()) //Obtem portas
            {
                comboBox1.Items.Add(s); //Adiciona porta à lista
            }
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //Acçao para a porta seleccionada
        {
            ComPort = comboBox1.SelectedItem.ToString();    //Guarda na variável ComPort a porta seleccionada 
        }

        private void comboBox2_Click(object sender, EventArgs e)    //Acçao da caixa de selecção do baudrate
        {
            comboBox2.Items.Clear();            
            comboBox2.Items.Add("9600");//Adiciona item
            comboBox2.Items.Add("38400");
            comboBox2.Items.Add("115200");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) //Acção para o baudrate selecionado 
        {
            Baudrate = comboBox2.SelectedItem.ToString();   //Guarda na variável Baudrate o valor escolhido
        }   
    }
}
