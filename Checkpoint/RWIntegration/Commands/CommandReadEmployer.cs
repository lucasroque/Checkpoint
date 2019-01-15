using Checkpoint.Model;
using System;
using System.IO;

namespace Checkpoint.RWIntegration
{
    class CommandReadEmployer : Command
    {
        private Company company = new Company();

        public Company getCompany()
        {
            return company;
        }

        public ErrorCommand execute(String ip, String porta, String cpf, String hash)
        {
            
            ErrorCommand errorCommand = new ErrorCommand();
            Tcp tcp = null;
            try
            {
                tcp = new Tcp(ip, Convert.ToInt16(porta));
            }
            catch (IOException e1) {
                errorCommand.setErro(ErrorCommand.COMUNICACAO_NAO_ESTABELECIDA);
                return errorCommand;
            }

            try
            {
                try
                {
                    byte[] bytesCpf = Conversor.cpfToByte(cpf);
                    byte[] buffer = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.LER_EMPREGADOR,
                                                                  new byte[4], new byte[4], bytesCpf,
                                                                  (byte)0x00, CommandCodes.END);
                    sendBuffer(buffer, true, tcp);
                    byte[] respostaREP = new byte[Protocol.QTD_BYTES_RETORNO_LEITURA_EMPREGADOR];
                    // Lê resposta do REP
                    respostaREP = readAnswer(tcp, Protocol.QTD_BYTES_RETORNO_LEITURA_EMPREGADOR);
                    int qtdBytesRecebidos = -1;
                    if (respostaREP != null)
                    {
                        qtdBytesRecebidos = respostaREP.Length;
                    }
                    // Trata a resposta do REP
                    errorCommand = tratarResposta(CommandCodes.LER_EMPREGADOR, respostaREP, qtdBytesRecebidos, Protocol.QTD_BYTES_RETORNO_LEITURA_EMPREGADOR);
                    if (errorCommand.getErro() == ErrorCommand.SUCESSO)
                    {
                        errorCommand = this.tratarResposta(0, respostaREP, 0, 0);
                        //					debug(errorCommand); // testes
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

        public override ErrorCommand tratarResposta(int codigoComandoEsperado, byte[] dados, int qdeRecebida, int qdeEsperada)
        {
            ErrorCommand errorCommand = new ErrorCommand(ErrorCommand.SUCESSO);

            try
            {
                // Conversão do Identificador (CNPJ/CPF)
                byte[] bytesIdentificador = new byte[Protocol.TAMANHO_IDENTIFICADOR_EMPREGADOR];
                for (int i = 0, j = Protocol.INDICE_IDENTIFICADOR_EMPREGADOR;
                         i < Protocol.TAMANHO_IDENTIFICADOR_EMPREGADOR &&
                         j < Protocol.INDICE_IDENTIFICADOR_EMPREGADOR + Protocol.TAMANHO_IDENTIFICADOR_EMPREGADOR;
                         i++, j++)
                {
                    bytesIdentificador[i] = dados[j];
                }
                String identificador = Conversor.bytesIdentificadorToString(bytesIdentificador);

                int tipoIdentificador = Conversor.byteToInt(dados[Protocol.INDICE_TIPO_IDENTIFICADOR_EMPREGADOR]);

                company.cnpj = identificador;

                // Conversão do CEI
                byte[] bytesCEI = new byte[Protocol.TAMANHO_CEI_EMPREGADOR];
                for (int i = 0, j = Protocol.INDICE_CEI_EMPREGADOR;
                         i < Protocol.TAMANHO_CEI_EMPREGADOR &&
                         j < Protocol.INDICE_CEI_EMPREGADOR + Protocol.TAMANHO_CEI_EMPREGADOR;
                         i++, j++)
                {
                    bytesCEI[i] = dados[j];
                }
                company.cei = Conversor.bytesCEIToString(bytesCEI);

                // Conversão da Razão Social
                byte[] bytesRazaoSocial = new byte[Protocol.TAMANHO_RAZAO_SOCIAL_EMPREGADOR];
                for (int i = 0, j = Protocol.INDICE_RAZAO_SOCIAL_EMPREGADOR;
                         i < Protocol.TAMANHO_RAZAO_SOCIAL_EMPREGADOR &&
                         j < Protocol.INDICE_RAZAO_SOCIAL_EMPREGADOR + Protocol.TAMANHO_RAZAO_SOCIAL_EMPREGADOR;
                         i++, j++)
                {
                    bytesRazaoSocial[i] = dados[j];
                }
                company.companyName = Conversor.bytesASCIIToString(bytesRazaoSocial);

                // Conversão do Local da Prestação de Serviço
                byte[] bytesLocalPrestacaoServico = new byte[Protocol.TAMANHO_LOCAL_EMPREGADOR];
                for (int i = 0, j = Protocol.INDICE_LOCAL_EMPREGADOR;
                         i < Protocol.TAMANHO_LOCAL_EMPREGADOR &&
                         j < Protocol.INDICE_LOCAL_EMPREGADOR + Protocol.TAMANHO_LOCAL_EMPREGADOR;
                         i++, j++)
                {
                    bytesLocalPrestacaoServico[i] = dados[j];
                }
                company.address = Conversor.bytesASCIIToString(bytesLocalPrestacaoServico);
            }
            catch (Exception e)
            {
                errorCommand.setErro(ErrorCommand.RETORNO_INCONSISTENTE);
            }

            return errorCommand;
        }

    }

}
