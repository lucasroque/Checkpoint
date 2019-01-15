using System;

namespace Checkpoint.RWIntegration
{
    class Command
    {

        public static void sendBuffer(byte[] buffer, bool sendAll, Tcp tcp)
        {
            byte[] bufferCriptografado = AES.Encrypt(buffer);
            tcp.enviaBytes(bufferCriptografado);
        }

        public byte[] readAnswer(Tcp tcp, int qtdBytesBuffer)
        {
            byte[] retorno = new byte[qtdBytesBuffer];

            int qdeRecebida = tcp.recebeBytes(retorno, 0, qtdBytesBuffer, Protocol.TIMEOUT);

            if ((qdeRecebida == -1))
            {
                return null;
            }
            else if ((qdeRecebida != qtdBytesBuffer))
            {
                return new byte[0];
            }

            retorno = AES.Decrypt(retorno);

            return retorno;
        }

        public virtual ErrorCommand tratarResposta(int codigoComandoEsperado, byte[] retornoReal, int qdeRecebida, int qdeEsperada)
        {
            ErrorCommand flagErroComando = new ErrorCommand();

            if ((qdeEsperada == qdeRecebida))
            {
                if (!validarComando(retornoReal, codigoComandoEsperado))
                {
                    return new ErrorCommand(ErrorCommand.RETORNO_INCONSISTENTE);
                }

                if (!validarBCC(retornoReal))
                {
                    return new ErrorCommand(ErrorCommand.ERRO_BCC);
                }

                flagErroComando.setErro(retornoReal[Protocol.INDICE_FLAG_RETORNO]);
            }
            else if ((-1 == qdeRecebida))
            {
                flagErroComando.setErro(ErrorCommand.COMUNICACAO_NAO_ESTABELECIDA);
            }
            else
            {
                flagErroComando.setErro(ErrorCommand.RETORNO_INCONSISTENTE);
            }

            return flagErroComando;
        }

        private static bool validarComando(byte[] pacote, int codigoComandoEsperado)
        {
            if ((Conversor.byteToInt(pacote[Protocol.INDICE_COMANDO_RETORNO]) != codigoComandoEsperado))
            {
                return false;
            }

            return true;
        }

        private static bool validarBCC(byte[] pacote)
        {
            byte bccPacote = 0;
            byte bccRecebido = 0;
            bccRecebido = pacote[(Protocol.QTD_BYTES_CABECALHO_DADOS - 2)];
            for (int i = 0; i < (Protocol.QTD_BYTES_CABECALHO_DADOS - 2); i++)
            {
                bccPacote ^= pacote[i];
            }

            if ((bccRecebido == bccPacote))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static byte[] criarPacoteCabecalho(int start, int codigoComando, byte[] parametro, byte[] tamanho, byte[] cpfPis, byte flag, int end)
        {
            try
            {
                byte[] requisicao = new byte[] {
                    ((byte)(start)),
                    0,
                    0,
                    0,
                    0,
                    ((byte)(codigoComando))};
                //  Campo Comando 1 byte
                requisicao = ProtocolUtils.merge(requisicao, parametro);
                //  Campo Par�metro 4 bytes
                requisicao = ProtocolUtils.merge(requisicao, tamanho);
                //  Campo Tamanho 4 bytes
                requisicao = ProtocolUtils.merge(requisicao, cpfPis);
                //  Campo CPF/PIS 6 bytes
                requisicao = ProtocolUtils.merge(requisicao, new byte[] {
                        flag});
                //  Campo Flag/Erro 1 byte                                                                    
                requisicao = ProtocolUtils.calcularChecksum(requisicao);
                //  Campo BCC 1 byte
                requisicao = ProtocolUtils.merge(requisicao, new byte[] {
                        ((byte)(end))});
                //  Campo Flag/Erro 1 byte
                return requisicao;
            }
            catch (Exception e)
            {
                return null;
            }

        }

    }
}
