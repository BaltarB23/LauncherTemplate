using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BasicLaunchTemplate
{
    internal class LoginServerProvider
    {
        private string loginIpAddress;

        public LoginServerProvider(string loginIpAddress)
        {
            this.loginIpAddress = loginIpAddress;
        }


        public void Play(string userName, string password)
        {
            try
            {
                TcpClient tcpClient = new TcpClient(loginIpAddress, 27053);

                NetworkStream networkStream = tcpClient.GetStream();
                var valid = CheckName(userName);
                if (!valid)
                {
                    Console.WriteLine("Name not valid");
                    return;
                }
                byte[] buffer = GetSendBuffer(RequestMsgType.Login, userName, password);
                networkStream.Write(buffer, 0, buffer.Length);

                int lenToRead = networkStream.ReadByte();
                if (lenToRead == -1)
                {
                    networkStream.Close();
                    tcpClient.Close();
                    return;
                }
                byte[] recvBuffer = new byte[lenToRead];
                int readSize = networkStream.Read(recvBuffer, 0, recvBuffer.Length);
                DecodeLoginServerMsg(recvBuffer);

                networkStream.Close();
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Register(string userName, string password)
        {
            try
            {
                TcpClient tcpClient = new TcpClient(loginIpAddress, 27053);

                NetworkStream networkStream = tcpClient.GetStream();

                var valid = CheckName(userName);
                if (!valid)
                {
                    Console.WriteLine("Name not valid");
                    return;
                }

                byte[] buffer = GetSendBuffer(RequestMsgType.Register, userName, password);
                networkStream.Write(buffer, 0, buffer.Length);

                int lenToRead = networkStream.ReadByte();
                if (lenToRead == -1)
                {
                    networkStream.Close();
                    tcpClient.Close();
                    return;
                }
                byte[] recvBuffer = new byte[lenToRead];
                int readSize = networkStream.Read(recvBuffer, 0, recvBuffer.Length);
                DecodeLoginServerMsg(recvBuffer);


                networkStream.Close();
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private byte[] GetSendBuffer(RequestMsgType requestMsgType, string userName, string password)
        {

            ProtocolWriter bw = new ProtocolWriter();
            bw.Write((byte)requestMsgType); //msgType
            bw.Write(userName);
            bw.Write(password);

            byte[] buffer = bw.GetBuffer();

            return buffer;
        }

        private bool CheckName(string name)
        {
            bool lenFlag = name.Length > 20;
            if (lenFlag)
            {
                return false;
            }

            foreach (char c in name.ToCharArray())
            {
                bool isOk = CheckChar(c);
                if (!isOk)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckChar(char c)
        {
            bool digitFlag = char.IsDigit(c);
            bool isAlphabet = char.IsLetter(c);
            return digitFlag || isAlphabet;
        }

        private void DecodeLoginServerMsg(byte[] buffer)
        {
            //received userId and session
            ProtocolReader br = new ProtocolReader(buffer);
            int msgType = br.ReadByte();
            LoginServerResponseType loginServerResponseType = (LoginServerResponseType)msgType;
            switch (loginServerResponseType)
            {
                case LoginServerResponseType.UserLoginCorrect:
                    string ipAddress = br.ReadString();
                    string session = br.ReadString();


                    CallClient(session, ipAddress);
                    break;
                case LoginServerResponseType.CannotFindNamePwCombination:
                    Console.WriteLine("Cannot find Name-Password combination!");
                    break;
                case LoginServerResponseType.UserAlreadyExists:
                    Console.WriteLine("UserName already exists!");
                    break;
                case LoginServerResponseType.UserSuccessfullyCreated:
                    Console.WriteLine("User successfully created");
                    break;
                case LoginServerResponseType.UserBanned:
                    Console.WriteLine("User is banned");
                    break;
                case LoginServerResponseType.RateLimit:
                    Console.WriteLine("Rate limit!");
                    break;
                default:
                    Console.WriteLine("unknown messageType");
                    break;
            }
        }
        private string GetClientArgs(string session, string ip)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            IPAddress address = IPAddress.Parse(ip);
            dictionary.Add("+projectID", 547);
            dictionary.Add("+userID", 1);
            dictionary.Add("+sessionID", "1");
            dictionary.Add("+trackingID", "1");
            dictionary.Add("+gameServer", address.ToString());
            dictionary.Add("+cdn", ".\\client\\live\\bsgo.exe");
            dictionary.Add("+language", "en");
            dictionary.Add("+session", session);
            dictionary.Add("+version", "3b27980a3b7dd77e597872106ca98000");
            string text = string.Empty;
            foreach (KeyValuePair<string, object> keyValuePair in dictionary)
            {
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    keyValuePair.Key,
                    " ",
                    keyValuePair.Value,
                    " "
                });
            }
            return text;
        }

        private void CallClient(string session, string ip)
        {
            string args = GetClientArgs(session, ip);
            string path = ".\\client\\live\\bsgo.exe";

            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(path);
                processStartInfo.Arguments = args;
                Process p = new Process();
                p.StartInfo = processStartInfo;
                p.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
