﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GameServerTCP
{
    public class GameServer
    {
        public static int port ;
        public static int maxPlayer = 20 ; 
        private static TcpListener tcpListener;
        private static UdpClient udpListener; 
        public static Dictionary<int, Client> clients;
        public delegate void PackHandler(int fromClient, Packet packet);
        public static Dictionary<int, PackHandler> packetHandlers;
        
        public static void Start(int port)
        {
            InitServerData(); 
            GameServer.port = port; 
            tcpListener = new TcpListener(IPAddress.Any, port); 
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);

            udpListener = new UdpClient(port);
            udpListener.BeginReceive(OnUDPReceived, null); 

            Console.WriteLine("Server started on port {0}", port); 
        }


        private static void OnUDPReceived(IAsyncResult ar)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(ar, ref _clientEndPoint);
                udpListener.BeginReceive(OnUDPReceived, null);

                if (_data.Length < 4)
                {
                    return;
                }

                using (Packet _packet = new Packet(_data))
                {
                    int _clientId = _packet.ReadInt();

                    if (_clientId == 0)
                    {
                        return;
                    }

                    if (clients[_clientId].udp.endPoint == null)
                    {
                        clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        clients[_clientId].udp.HandleData(_packet);
                    }
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving UDP data: {_ex}");
            }
        }

        private static void OnClientConnect(IAsyncResult ar)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(ar);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
            Console.WriteLine("Incoming connection from : " + client.Client.RemoteEndPoint + " ...");

            for (int i = 1; i <= maxPlayer; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(client); 
                    return; 
                }
            }
        }
        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
            }
        }

        private static void InitServerData()
        {
            clients = new Dictionary<int, Client>(); 
            for (int i = 0; i <= maxPlayer; i++)
            {
                clients.Add(i, new Client(i));
            }
            packetHandlers = new Dictionary<int, PackHandler>()
            {
                {(int)ClientPackets.WelcomeReceived, ServerHandle.WelcomeReceived },
                {(int)ClientPackets.UdpTestReceived, ServerHandle.UDPTestReceived },
                {(int)ClientPackets.TcpIssuedCommand, ServerHandle.TcpCommandReceived },
                {(int)ClientPackets.UdpUpdatePlayer, ServerHandle.UdpUpdatePlayer },
            };
        }
    }
}
