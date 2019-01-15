using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoint.RWIntegration.Commands
{
    class CommandDeleteEmployee : Command
    {
        private Tcp tcp;

        public CommandDeleteEmployee(Tcp tcp)
        {
            this.tcp = tcp;
        }

        public ErrorCommand execute(String ip, String porta, String cpf, String pis)
        {
            Boolean finalizarConexao = false;
            ErrorCommand errorCommand = new ErrorCommand();
            if (tcp == null)
            {
                try
                {
                    finalizarConexao = true;
                    tcp = new Tcp(ip, Convert.ToInt16(porta));
                }
                catch (IOException e) {
                    errorCommand.setErro(ErrorCommand.COMUNICACAO_NAO_ESTABELECIDA);
                    return errorCommand;
                }
                }

                try
                {
                    try
                    {
                        byte[] bytesCpf = Conversor.cpfToByte(cpf);
                        byte[] cabecalhoDadosGravaFuncionario = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.ENVIAR_FUNCIONARIO,
                                                                                             new byte[] { 0x00, 0x00, 0x00, 0x01 }, new byte[4], bytesCpf,
                                                                                             (byte)ErrorCommand.EXCLUIR, CommandCodes.END);
                        // Envia o cabeçalho
                        sendBuffer(cabecalhoDadosGravaFuncionario, true, tcp);
                        // Lê 1ª resposta do REP
                        byte[] retornoReal = readAnswer(tcp, Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                        int qtdBytesRecebidos = -1;
                        if (retornoReal != null)
                        {
                            qtdBytesRecebidos = retornoReal.Length;
                        }
                        // Trata a 1ª resposta do REP
                        errorCommand = tratarResposta(CommandCodes.ENVIAR_FUNCIONARIO, retornoReal, qtdBytesRecebidos, Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                        if (errorCommand.getErro() == ErrorCommand.SUCESSO)
                        {
                            cabecalhoDadosGravaFuncionario = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.ENVIAR_FUNCIONARIO,
                                                                                          new byte[] { 0x00, 0x01, 0x00, 0x01 }, new byte[] { 0x00, 0x00, 0x00, 0x61 },
                                                                                          bytesCpf, (byte)ErrorCommand.DADOS_OK, CommandCodes.END);

                            byte checkSumCabecalho = cabecalhoDadosGravaFuncionario[cabecalhoDadosGravaFuncionario.Length - 2];
                            byte[] dadosGravaFuncionario = criaPacoteDados(checkSumCabecalho, bytesCpf, pis);

                            // Envia os dados do funcionário
                            sendBuffer(ProtocolUtils.merge(cabecalhoDadosGravaFuncionario, dadosGravaFuncionario), true, tcp);
                            retornoReal = new byte[Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO];
                            // Lê 2ª resposta do REP
                            retornoReal = readAnswer(tcp, Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                            qtdBytesRecebidos = -1;
                            if (retornoReal != null)
                            {
                                qtdBytesRecebidos = retornoReal.Length;
                            }
                            // Trata a 2ª resposta do REP
                            errorCommand = tratarResposta(CommandCodes.ENVIAR_FUNCIONARIO, retornoReal, qtdBytesRecebidos, Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                        }
                    }
                    catch (Exception e)
                    {
                        errorCommand.setErro(ErrorCommand.OCORREU_EXCECAO);
                    }
                    if (finalizarConexao)
                    {
                        tcp.finalizaConexao();
                    }
                }
                catch (Exception e)
                {
                    errorCommand.setErro(ErrorCommand.ERRO_FINALIZAR_CONEXAO);
                }

                return errorCommand;
            }


        private byte[] criaPacoteDados(byte checkSumCabecalho, byte[] cpf, String pisString)
        {
            byte[] nome = Conversor.stringToByteArray("nome", 52); // Campo Nome 52 bytes;
            byte[] pis = Conversor.pisToByte(Conversor.SomenteNumeros(pisString)); // Campo PIS 6 bytes;
            byte[] reservado = new byte[4]; // Campo Reservado 4 bytes; 
            byte[] cartao = Conversor.stringParaBytes("0", 20); // Campo Cartão 20 bytes;
            byte[] codigo = Conversor.stringParaBytes("0", 3); // Campo Código 3 bytes;
            byte[] mestre = Conversor.stringParaBytes("0", 1); // Campo Mestre 1 byte;
            byte[] senha = Conversor.stringParaBytes("0", 3); // Campo Senha 3 bytes;
            byte[] verifica1ToN = new byte[1]; // Campo Verifica 1 byte;
            byte[] cmp = new byte[1]; // Campo cmp 1 byte;
            byte[] requisicao = new byte[] { };
            requisicao = ProtocolUtils.merge(nome, pis, reservado, cartao, codigo, mestre, senha, verifica1ToN, cmp, cpf);
            byte checksum = ProtocolUtils.getChecksum(ProtocolUtils.merge(requisicao, new byte[] { checkSumCabecalho, Convert.ToByte(CommandCodes.END) }));
            requisicao = ProtocolUtils.merge(requisicao, new byte[] { checksum });
            return requisicao;
        }

    }
}
