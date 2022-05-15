using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Server
{
    public partial class Form1 : Form
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>();

        bool terminating = false;
        bool listening = false;
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
            server_logs.Enabled = false;
        }


        private void listen_button_Click(object sender, EventArgs e)
        {
            int serverPort;

            if (Int32.TryParse(listenport_textbox.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(3);

                listening = true;
                listen_button.Enabled = false;
                server_logs.Enabled = true;

                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                server_logs.AppendText("Started listening on port: " + serverPort + ".\n");
                listenport_textbox.Enabled = false;

            }
            else
            {
                server_logs.AppendText("Please check port number \n");
            }
        }

        private void Accept()
        {
            while (listening)
            {
                try
                {
                    Socket newClient = serverSocket.Accept();

                    //Before Accepting the connection, the server need to check if username is OK
                    Byte[] usernameBuffer = new byte[1024];
                    newClient.Receive(usernameBuffer);
                    string incomingUsername = Encoding.Default.GetString(usernameBuffer);
                    incomingUsername = incomingUsername.Substring(0, incomingUsername.IndexOf("\0"));

                    bool usernameExists = File.ReadAllText(@"../../database.txt").Contains(incomingUsername);

                    Byte[] usernameResponseBuffer = new Byte[64];
                    string usernameResponseString = "";

                    if (usernameExists)
                    {

                        
                        usernameResponseString = "yes";
                        usernameResponseBuffer = Encoding.Default.GetBytes(usernameResponseString);
                        newClient.Send(usernameResponseBuffer);

                        server_logs.AppendText("A client has connected!\n");

                        clientSockets.Add(newClient);

                        Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                        receiveThread.Start();

                    }
                    else
                    {
                        server_logs.AppendText(incomingUsername + "Tried to connect to the server but cannot!\n");

                        usernameResponseString = "no";
                        usernameResponseBuffer = Encoding.Default.GetBytes(usernameResponseString);
                        newClient.Send(usernameResponseBuffer);

                        newClient.Close();
                    }
                    
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        server_logs.AppendText("The socket stopped working.\n");
                        listening = false;
                    }

                }
            }
        }

        private void Receive(Socket thisClient) // updated
        {
            bool connected = true;

            while (connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[1024];
                    thisClient.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));


                    if (incomingMessage == "SEND")
                    {
                        Byte[] sendBuffer = new Byte[1024];
                        thisClient.Receive(sendBuffer);

                        string incomingPostMessage = Encoding.Default.GetString(sendBuffer);
                        incomingPostMessage = incomingPostMessage.Substring(0, incomingPostMessage.IndexOf("\0"));



                        string[] incomingWords = incomingPostMessage.Split('$');
                        string usernameString = incomingWords[0];
                        string postString = incomingWords[1];

                        DateTime now = DateTime.Now;
                        now.ToString("F");

                        



                        int max = 0; 
                        foreach (string line in File.ReadLines("../../postsDatabase.txt", Encoding.UTF8))
                        {
                            char[] delimeters = { '|', '|' };
                            string[] lineWords = line.Split(delimeters);
                            string postID = lineWords[1];
                            int postIDNum = int.Parse(postID);

                            if (postIDNum > max)
                            {
                                max = postIDNum;
                            } 
                        }

                        int postIDVar = max + 1;
                        string postIDVarString = postIDVar.ToString();


                        string finalLine = usernameString + "||" + postIDVarString + "||" + postString + "||" + now;


                        server_logs.AppendText(finalLine);

                        using (StreamWriter file = new StreamWriter("../../postsDatabase.txt", append: true))
                        {
                            file.WriteLine(finalLine);
                        }





                    }
                    else if (incomingMessage == "ALLP")
                    {
                        foreach (string line in File.ReadLines("../../postsDatabase.txt", Encoding.UTF8))
                        {
                            char[] delimeters = { '|', '|' };
                            string[] lineWords = line.Split(delimeters);
                            string postID = lineWords[1];
                            int postIDNum = int.Parse(postID);

                            
                        }





                        Byte[] sendBuffer = new Byte[1000000];
                        thisClient.Send(sendBuffer);

                    }


                }
                catch
                {
                    if (!terminating)
                    {
                        server_logs.AppendText("A client has disconnected\n");
                    }
                    thisClient.Close();
                    clientSockets.Remove(thisClient);
                    connected = false;
                }
            }
        }
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }



    }
}
