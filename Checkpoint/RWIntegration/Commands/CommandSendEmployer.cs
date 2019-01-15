 using Checkpoint.Model;
using System;
using System.IO;

namespace Checkpoint.RWIntegration
{
    class CommandSendEmployer : Command
    {

        public ErrorCommand execute(String ip, String porta, String cpf, Company company)
        {
            ErrorCommand errorCommand = new ErrorCommand();
            Tcp tcp = null;
            try
            {
                tcp = new Tcp(ip, Convert.ToInt16(porta));
            }
            catch (IOException e) {
                errorCommand.setErro(ErrorCommand.SUCESSO);
                return errorCommand;
            }

            try
            {
                try
                {
                    byte[] bytesCpf = Conversor.cpfToByte(cpf);
                    byte[] cabecalhoDadosGravaEmpregador = Command.criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.ENVIAR_EMPREGADOR, new byte[] { 0x00, 0x00, 0x00, 0x01 }, new byte[4], bytesCpf, (byte)ErrorCommand.ADICIONAR_SUBSTITUIR, CommandCodes.END);
                    // Envia o cabeçalho
                    sendBuffer(cabecalhoDadosGravaEmpregador, true, tcp);
                    // LÃª 1ª resposta do REP
                    byte[] retornoReal = readAnswer(tcp, Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                    int qtdBytesRecebidos = -1;
                    if (retornoReal != null)
                    {
                        qtdBytesRecebidos = retornoReal.Length;
                    }
                    // Trata a 1ª resposta do REP
                    errorCommand = tratarResposta(CommandCodes.ENVIAR_EMPREGADOR, retornoReal, qtdBytesRecebidos, Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                    if (errorCommand.getErro() == ErrorCommand.SUCESSO)
                    {
                        cabecalhoDadosGravaEmpregador = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.ENVIAR_EMPREGADOR,
                                                                                      new byte[] { 0x00, 0x01, 0x00, 0x01 }, new byte[] { 0x00, 0x00, 0x01, 0x0C },
                                                                                      bytesCpf, (byte)ErrorCommand.DADOS_OK, CommandCodes.END);

                        byte checkSumCabecalho = cabecalhoDadosGravaEmpregador[cabecalhoDadosGravaEmpregador.Length - 2];
                        byte[] dadosGravaEmpregador = criaPacoteDados(checkSumCabecalho, bytesCpf, company);

                        // Envia os dados do empregador
                        sendBuffer(ProtocolUtils.merge(cabecalhoDadosGravaEmpregador, dadosGravaEmpregador), true, tcp);
                        retornoReal = new byte[Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO];
                        // LÃª 2ª resposta do REP
                        retornoReal = readAnswer(tcp, Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                        qtdBytesRecebidos = -1;
                        if (retornoReal != null)
                        {
                            qtdBytesRecebidos = retornoReal.Length;
                        }
                        // Trata a 2ª resposta do REP
                        errorCommand = tratarResposta(CommandCodes.ENVIAR_EMPREGADOR, retornoReal, qtdBytesRecebidos, Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                    }
                }
                catch (Exception e)
                {
                    errorCommand.setErro(ErrorCommand.OCORREU_EXCECAO);
                }
                tcp.finalizaConexao();
            }
            catch (Exception e)
            {
                errorCommand.setErro(ErrorCommand.ERRO_FINALIZAR_CONEXAO);
            }

            return errorCommand;
        }

        private byte[] criaPacoteDados(byte checkSumCabecalho, byte[] cpf, Company company)
        {
            byte tipoId;
            byte[] identificador;

            tipoId = 0x01;
            identificador = Conversor.cnpjToByte(Conversor.SomenteNumeros(Conversor.SomenteNumeros(company.cnpj))); // Campo Identificador 6 bytes

            byte[] requisicao = { tipoId }; // Campo Tipo de Identificador 1 byte
            byte[] cei = Conversor.ceiToByte(Conversor.SomenteNumeros(company.cei)); // Campo CEI 5 bytes
            byte[] razaoSocial = Conversor.stringToByteArray(company.companyName, 150); // Campo Razão Social 150 bytes
            byte[] localPrestServ = Conversor.stringToByteArray(company.address + " " + company.city, 100); // Campo Local 100 bytes
            requisicao = ProtocolUtils.merge(requisicao, identificador, cei, razaoSocial, localPrestServ, cpf);
            byte checksum = ProtocolUtils.getChecksum(ProtocolUtils.merge(requisicao, new byte[] { checkSumCabecalho, Convert.ToByte(CommandCodes.END) }));
            requisicao = ProtocolUtils.merge(requisicao, new byte[] { checksum });
            return requisicao;
        }

    }
}
