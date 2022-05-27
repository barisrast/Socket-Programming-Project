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
        Dictionary<string, Socket> socketDictionary = new Dictionary<string, Socket>();
        

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
                server_logs.AppendText("Please check port number.\n");
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

                    // Check wheter the username exist in database
                    bool usernameExists = false;
                    foreach( string line in File.ReadLines(@"../../user-db.txt", Encoding.UTF8))
                    {
                        if( line == incomingUsername )
                        {
                            usernameExists = true;
                        }
                    }


                    Byte[] usernameResponseBuffer = new Byte[64];
                    string usernameResponseString = "";

                    if (usernameExists)
                    {
                        if (!socketDictionary.ContainsKey(incomingUsername))
                        {

                            usernameResponseString = "yes";
                            usernameResponseBuffer = Encoding.Default.GetBytes(usernameResponseString);
                            newClient.Send(usernameResponseBuffer);

                            server_logs.AppendText(incomingUsername + " has connected!\n");


                            socketDictionary.Add(incomingUsername, newClient);
                            clientSockets.Add(newClient);

                            Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                            receiveThread.Start();
                        }
                        else
                        {
                            server_logs.AppendText(incomingUsername + " is already connected but tried to connect again.\n");

                            usernameResponseString = "no";
                            usernameResponseBuffer = Encoding.Default.GetBytes(usernameResponseString);
                            newClient.Send(usernameResponseBuffer);

                            newClient.Close();
                        }

                    }
                    else
                    {
                        server_logs.AppendText(incomingUsername + " tried to connect to the server but cannot!\n");

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
            Dictionary<Socket, string> revDictionary = socketDictionary.ToDictionary(pair => pair.Value, pair => pair.Key);
            string UsernameVar = revDictionary[thisClient];

            while (connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[64];
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
                        foreach (string line in File.ReadLines("../../post-db.txt", Encoding.UTF8))
                        {
                            char[] delimeters = { '|', '|' };
                            string[] lineWords = line.Split(delimeters);
                            string postID = lineWords[2];
                            int postIDNum = int.Parse(postID);

                            if (postIDNum > max)
                            {
                                max = postIDNum;
                            } 
                        }

                        int postIDVar = max + 1;
                        string postIDVarString = postIDVar.ToString();

                        string finalLine = usernameString + "||" + postIDVarString + "||" + postString + "||" + now;

                        server_logs.AppendText(usernameString+" has sent a post:\n");
                        server_logs.AppendText(postString + "\n");

                        using (StreamWriter file = new StreamWriter("../../post-db.txt", append: true))
                        {
                            file.WriteLine(finalLine);
                        }

                    }

                    else if (incomingMessage == "ALL_POST" || incomingMessage == "MY_POST")
                    {
                        Byte[] usernamePostBuffer = new Byte[1024];
                        thisClient.Receive(usernamePostBuffer);
                        string incomingPostUsername = Encoding.Default.GetString(usernamePostBuffer);
                        incomingPostUsername = incomingPostUsername.Substring(0, incomingPostUsername.IndexOf("\0"));


                        string infoMessageString = incomingMessage == "ALL_POST" ? "\nShowing all posts from clients:\n" : "\nShowing your posts:\n";           
                        Byte[] infoMessageBuffer = new Byte[1024];
                        infoMessageBuffer = Encoding.Default.GetBytes(infoMessageString);
                        thisClient.Send(infoMessageBuffer);

                        foreach (string line in File.ReadLines("../../post-db.txt", Encoding.UTF8))
                        {
                            char[] delimeters = { '|', '|' };
                            string[] lineWords = line.Split(delimeters);

                            string usernameToken = lineWords[0];
                            string postIdToken = lineWords[2];
                            string postTextToken = lineWords[4];
                            string postTimeToken = lineWords[6];


                            if (incomingMessage == "ALL_POST" && usernameToken != incomingPostUsername)
                            {
                                string postMessageString = "Username: " + usernameToken + "\n" + "PostID: " + postIdToken + "\n" + "Post: " + postTextToken + "\n" + "Time: " + postTimeToken + "\n\n";

                                Byte[] sendBuffer = new Byte[1000000];
                                sendBuffer = Encoding.Default.GetBytes(postMessageString);
                                thisClient.Send(sendBuffer);
                            }

                            else if (incomingMessage == "MY_POST" && usernameToken == incomingPostUsername) 
                            {
                                string postMessageString = "Username: " + usernameToken + "\n" + "PostID: " + postIdToken + "\n" + "Post: " + postTextToken + "\n" + "Time: " + postTimeToken + "\n\n";

                                Byte[] sendBuffer = new Byte[1000000];
                                sendBuffer = Encoding.Default.GetBytes(postMessageString);
                                thisClient.Send(sendBuffer);
                            }

                        }
                        if(incomingMessage == "ALL_POST")
                            server_logs.AppendText("Showed all posts for " + incomingPostUsername + ".\n");
                        else if(incomingMessage == "MY_POST")
                            server_logs.AppendText(incomingPostUsername + " looked his/her own posts.\n");
                    }

                    else if(incomingMessage == "DELETE_POST")
                    {
                        Byte[] usernamAndPostIDBuffer = new Byte[1024];
                        thisClient.Receive(usernamAndPostIDBuffer);
                        string usernameAndPostID = Encoding.Default.GetString(usernamAndPostIDBuffer);
                        usernameAndPostID = usernameAndPostID.Substring(0, usernameAndPostID.IndexOf("\0"));

                        string username = usernameAndPostID.Substring(0, usernameAndPostID.IndexOf("|"));
                        string postId = usernameAndPostID.Substring(usernameAndPostID.IndexOf("|") + 1);

                        bool doesIDExist = false;
                        // Rewriting the file, skip the line if there is a Username & Post ID match
                        string[] Lines = File.ReadAllLines("../../post-db.txt");
                        File.Delete("../../post-db.txt");// Deleting the file
                        using (StreamWriter sw = File.AppendText("../../post-db.txt")) {
                            foreach (string line in Lines) {

                                char[] delimeters = { '|', '|' };
                                string[] lineWords = line.Split(delimeters);

                                string usernameToken = lineWords[0];
                                string postIdToken = lineWords[2];
                                string postTextToken = lineWords[4];
                                string postTimeToken = lineWords[6];

                                // Username & Post ID match, skip the line
                                if (postIdToken == postId && usernameToken == username) {                               
                                    string infoMessageString = "Post with ID " + postId + " is deleted successfully!\n";
                                    Byte[] infoMessageBuffer = new Byte[1024];
                                    infoMessageBuffer = Encoding.Default.GetBytes(infoMessageString);
                                    thisClient.Send(infoMessageBuffer);

                                    server_logs.AppendText("Post with ID " + postId + " is deleted.\n");

                                    doesIDExist = true;
                                    continue;
                                }
                                // No match, rewrite the line
                                else {
                                    sw.WriteLine(line);

                                    // Post ID match, Username does not match
                                    if(postIdToken == postId && usernameToken != username) {
                                        string infoMessageString = "Post with ID " + postId + " is not yours!\n";
                                        Byte[] infoMessageBuffer = new Byte[1024];
                                        infoMessageBuffer = Encoding.Default.GetBytes(infoMessageString);
                                        thisClient.Send(infoMessageBuffer);

                                        server_logs.AppendText("Post with ID " + postId + " is not " + username + "'s!\n");
                                        doesIDExist = true;
                                    }
                                }
                            }

                            // Post ID does not exist in the file
                            if (!doesIDExist) {
                                string infoMessageString = "There is no post with ID: " + postId + ".\n";
                                Byte[] infoMessageBuffer = new Byte[1024];
                                infoMessageBuffer = Encoding.Default.GetBytes(infoMessageString);
                                thisClient.Send(infoMessageBuffer);

                                server_logs.AppendText("Post with ID " + postId + " does not exist!\n");
                            }
                        }
                    }
                }

                catch
                {
                    if (!terminating)
                    {
                        server_logs.AppendText(UsernameVar + " has disconnected.\n");
                    }
                    thisClient.Close();
                   
                    socketDictionary.Remove(UsernameVar);
                    revDictionary.Remove(thisClient);
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
