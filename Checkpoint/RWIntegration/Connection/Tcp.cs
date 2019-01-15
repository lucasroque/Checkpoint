using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Checkpoint.RWIntegration
{
    class Tcp
    {
        private String ip = "localhost";
        private int porta = 1001;
        private Socket socket = null;
        //private DataOutputStream saida = null;
        //private DataInputStream entrada = null;
        private int qdeUltimoEnvio = 0;

        public Tcp(String ip, int porta)
        {
            this.ip = ip;
            this.porta = porta;
            atualizaSocket(ip, porta);
        }

        public Tcp()
        {
            atualizaSocket(ip, porta);
        }

        private void atualizaSocket(String ip, int porta)
        {
            if ((socket != null))
            {
                socket.Close();
            }

            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(ip), porta);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            socket.Connect(ipEnd);
            atualizaSaida();
            atualizaEntrada();
        }

        public void preparaNovoEnvio()
        {
            atualizaSocket(ip, porta);
        }

        private void atualizaSaida()
        {
            //saida = new DataOutputStream(socket.se);
        }

        private void atualizaEntrada()
        {
            //entrada = new DataInputStream(socket.getInputStream());
        }

        public String getIp()
        {
            return ip;
        }

        public void setIp(String ip)
        {
            this.ip = ip;
            atualizaSocket(ip, porta);
        }

        public int getPorta()
        {
            return porta;
        }

        public void setPorta(int porta)
        {
            this.porta = porta;
            atualizaSocket(ip, porta);
        }

        public void enviaBytes(byte[] bytes, int ini, int qde)
        {
            if (qde < 1) {
                String s = "Quantidade não permitida";
            }

            if ((ini < 0) || ((ini + qde) > bytes.Length)) {
                String s = "Fora das margens: ini(" + ini + "), qde(" + qde + ")";
            }

            qdeUltimoEnvio = qde;

            socket.Send(bytes, qde, SocketFlags.None);

            //saida.write(bytes, ini, qde);
            //saida.flush();
        }

        public void enviaBytes(byte[] bytes)
        {
            enviaBytes(bytes, 0, bytes.Length);
        }

        public int recebeBytes(byte[] bytes, int ini, int qde, int timeout)
        {
            int bytesRecebidos = 0;
            if ((qde < 1))
            {
                String s = "Quantidade não permitida";
            }

            if (((ini < 0) || ((ini + qde) > bytes.Length)))
            {
                String s = "Fora das margens: ini(" + ini + "), qde(" + qde + ")";
            }

            try
            {
                //socket.setSoTimeout(timeout);

                //bytesRecebidos = entrada.read(bytes, ini, qde);
                bytesRecebidos = socket.Receive(bytes, qde, SocketFlags.None);
                try
                {
                    Thread.Sleep(250);
                }
                catch (Exception ex)
                {
                    
                }
            } catch (Exception e) {
                String s = "Timeout ocorrido!";
                bytesRecebidos = -1;
            }

            return bytesRecebidos;
        }

        public bool envioFinalizado()
        {
            return ((qdeUltimoEnvio - bytesEnviados()) <= 0);
        }

        public int bytesEnviados()
        {
            //return saida.size();
            return 0;
        }

        public void finalizaConexao()
        {
            if (socket.Connected)
            {
                //entrada.close();
                //saida.close();
                socket.Close();
            }

        }

        ~Tcp()
        {
            finalizaConexao();
        }
    }
}
